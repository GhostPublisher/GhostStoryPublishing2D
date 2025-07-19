using System.Collections.Generic;

using UnityEngine;

using Foundations.Architecture.ReferencesHandler;
using GameSystems.EnemySystem.EnemyUnitSystem;

namespace GameSystems.EnemySystem.EnemyVisibilitySystem
{
    // Enemy의 visible, InVisible의 갱신은 언제 들어오는가?
    // 1. Player의 이동.
    // 1-1. Player의 시야 범위 갱신 시.
    // 1-2. Player의 OvercomeWeight 변경 시.

    // 2. Enemy의 이동.
    // 2-1. Enemy의 BlockWeight 변경 시.

    // 3. Scaner에 의한 특정 지점 활성화.

    // 1. 일단은 전체 비활성 -> 필요한 것만 활성.
    // 애니메이션 코루틴 부분에서 걸리는 것 해결 아이디어
    //  - HideFromPlayerView() 가 들어가면 IEnemyVisualController을 상속한 코드에서 isHideRequested = true; 가 됨.
    //  - 이후 ShowFromPlayerView() 가 들어오면 isHideRequested = false; 가 됨.
    //  - 이후 PlayerView() 가 들어가면, isHideRequested 가 true이면 hide 작업을 수행.

    //  - 마찬가지로 
    //  - ShowFromPlayerView() -> isShowRequested = true;
    //  - HideFromPlayerView() -> isShowRequested = false; 가 됨.
    //  - 이후 PlayerView() 가 들어가면, isShowRequested 가 true이면 show 작업을 수행.

    //  - 즉, 뒤에 나오는 것을 통해 작업을 진행하겠다는 거임.
    //  - ShowFromPlayerView() -> isShowRequested = true; isHideRequested = false;
    //  - HideFromPlayerView() -> isShowRequested = false; isHideRequested = true;
    //  - PlayerView() -> if(isShowRequested) OperateShow(); if(isHideRequested) OperateHide();

    //  - 추가로 현재 상태를 나타내서, 작업을 호출을 무시하자.
    //  - PlayerView() -> if(!isShowed && isShowRequested) OperateShow(); if(!isHide && isHideRequested) OperateHide();

    // 2. 이래도 과부화가 걸리기 시작하면, EnemyVisibilityController 코드에서 비교구문 후 필요한거만 비활성 & 활성.

    public class EnemyVisibilityController : MonoBehaviour
    {
        private Dictionary<int, ObjectVisibleData> playerVisibilityDatas = new();
        private Dictionary<int, ObjectVisibleData> scannerRevealedEnemyIDs = new();

        private EnemyUnitManagerDataDBHandler myEnemyUnitManagerDataDBHandler;

        private void Awake()
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            this.myEnemyUnitManagerDataDBHandler = HandlerManager.GetDynamicDataHandler<EnemyUnitManagerDataDBHandler>();
        }

        public void InitialSetting()
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            this.myEnemyUnitManagerDataDBHandler = HandlerManager.GetDynamicDataHandler<EnemyUnitManagerDataDBHandler>();
        }

        public void InitialSetting(EnemyUnitManagerDataDBHandler enemyUnitManagerDataDBHandler)
        {
            this.myEnemyUnitManagerDataDBHandler = enemyUnitManagerDataDBHandler;
        }

        // Player Unit의 시야에 의한 EnemyVisibility 갱신
        public void UpdatePlayerVisibleData_ForEnemyVisibility(int playerUniqueID,
            HashSet<Vector2Int> visibleRange, int physicalVisionOvercomeWeight, int spiritualVisionOvercomeWeight)
        {
            if(!this.playerVisibilityDatas.TryGetValue(playerUniqueID, out var data))
            {
                data = new ObjectVisibleData();
                this.playerVisibilityDatas[playerUniqueID] = data;
            }

            data.UniqueID = playerUniqueID;
            data.VisibleRange = visibleRange;
            data.PhysicalVisionOvercomeWeight = physicalVisionOvercomeWeight;
            data.SpiritualVisionOvercomeWeight = spiritualVisionOvercomeWeight;
            data.DetectedEnemyUniqueIDs = this.GetDetectedEnemyUnitIDs(visibleRange, physicalVisionOvercomeWeight, spiritualVisionOvercomeWeight);
        }
        public void RemovePlayerVisibleData_ForEnemyVisibility(int playerUniqueID)
        {
            this.playerVisibilityDatas.Remove(playerUniqueID);
        }

        // Scanner Unit의 시야에 의한 EnemyVisibility 갱신
        public void UpdateScanerVisibleData_ForEnemyVisibility(int scannerUniqueID,
            HashSet<Vector2Int> visibleRange, int physicalVisionOvercomeWeight, int spiritualVisionOvercomeWeight)
        {
            if (!this.scannerRevealedEnemyIDs.TryGetValue(scannerUniqueID, out var data))
            {
                data = new ObjectVisibleData();
                this.playerVisibilityDatas[scannerUniqueID] = data;
            }

            data.UniqueID = scannerUniqueID;
            data.VisibleRange = visibleRange;
            data.PhysicalVisionOvercomeWeight = physicalVisionOvercomeWeight;
            data.SpiritualVisionOvercomeWeight = spiritualVisionOvercomeWeight;
            data.DetectedEnemyUniqueIDs = this.GetDetectedEnemyUnitIDs(visibleRange, physicalVisionOvercomeWeight, spiritualVisionOvercomeWeight);

            Debug.Log($"ID : {scannerUniqueID} : ");
            foreach (int id in data.DetectedEnemyUniqueIDs)
            {
                Debug.Log($"Data - {id}");
            }

            foreach (int id in this.playerVisibilityDatas[scannerUniqueID].DetectedEnemyUniqueIDs)
            {
                Debug.Log($"DIc - {id}");
            }
        }
        public void RemoveScanerVisibleData_ForEnemyVisibility(int scannerUniqueID)
        {
            this.scannerRevealedEnemyIDs.Remove(scannerUniqueID);
        }

        // 시야 범위, 가중치 비교 후, 보이는 Enemy UnitIDs 리턴
        private HashSet<int> GetDetectedEnemyUnitIDs(HashSet<Vector2Int> visibleRange, int physicalVisionOvercomeWeight, int spiritualVisionOvercomeWeight)
        {
            HashSet<int> newDetecetedEnemyUnitIDs = new();

            foreach (Vector2Int pos in visibleRange)
            {
                if (this.myEnemyUnitManagerDataDBHandler.TryGetEnemyUnitManagerData(pos, out var data))
                {
                    if (physicalVisionOvercomeWeight >= data.EnemyUnitStaticData.PhysicalVisionBlockWeight
                        && spiritualVisionOvercomeWeight >= data.EnemyUnitStaticData.SpiritualVisionBlockWeight)

                        newDetecetedEnemyUnitIDs.Add(data.UniqueID);
                }
            }

            return newDetecetedEnemyUnitIDs;
        }

        // Enemy Show or Hide 수행.
        public void UpdateEnemyVisibility()
        {
            if (!this.myEnemyUnitManagerDataDBHandler.TryGetAll(out var datas)) return;

            // 전부 비활성화 요청.
            foreach(var data in datas)
            {
                data.EnemyUnitFeatureInterfaceGroup.EnemyUnitSpriteRendererController.HideRequestFromPlayerView();
            }

            HashSet<int> visibleAll = new();
            foreach (ObjectVisibleData objectVisibleData in this.playerVisibilityDatas.Values)
            {
                foreach (int id in objectVisibleData.DetectedEnemyUniqueIDs)
                {
                    visibleAll.Add(id);
                }
            }

            foreach (ObjectVisibleData objectVisibleData in this.scannerRevealedEnemyIDs.Values)
            {
                foreach (int id in objectVisibleData.DetectedEnemyUniqueIDs)
                {
                    visibleAll.Add(id);
                }
            }

            // 전체 유닛 돌면서, 작업 수행 요청.
            foreach (var id in visibleAll)
            {
                if (!this.myEnemyUnitManagerDataDBHandler.TryGetEnemyUnitManagerData(id, out var enemy)) continue;

                enemy.EnemyUnitFeatureInterfaceGroup.EnemyUnitSpriteRendererController.ShowRequestFromPlayerView();
            }

            // 전체 유닛 돌면서, 작업 수행 요청.
            foreach (var data in datas)
            {
                data.EnemyUnitFeatureInterfaceGroup.EnemyUnitSpriteRendererController.OperateShowAndHide();
            }
        }

        // 전체 Enemy Show
        public void ShowEnemyVisibilityAll()
        {
            if (!this.myEnemyUnitManagerDataDBHandler.TryGetAll(out var datas)) return;

            // 전부 활성화 요청.
            foreach (var data in datas)
            {
                data.EnemyUnitFeatureInterfaceGroup.EnemyUnitSpriteRendererController.ShowRequestFromPlayerView();
                data.EnemyUnitFeatureInterfaceGroup.EnemyUnitSpriteRendererController.OperateShowAndHide();
            }
        }
        // 전체 Enemy Hide
        public void HideEnemyVisibilityAll()
        {
            if (!this.myEnemyUnitManagerDataDBHandler.TryGetAll(out var datas)) return;

            // 전부 비활성화 요청.
            foreach (var data in datas)
            {
                data.EnemyUnitFeatureInterfaceGroup.EnemyUnitSpriteRendererController.HideRequestFromPlayerView();
                data.EnemyUnitFeatureInterfaceGroup.EnemyUnitSpriteRendererController.OperateShowAndHide();
            }
        }
        // Player Unit과 Scanner 시야 데이터 삭제.
        public void ClearEnemyVisibilityData()
        {
            this.playerVisibilityDatas.Clear();
            this.scannerRevealedEnemyIDs.Clear();
        }
    }

    public class ObjectVisibleData
    {
        public int UniqueID;

        public HashSet<Vector2Int> VisibleRange = new();
        public HashSet<int> DetectedEnemyUniqueIDs = new();

        public int PhysicalVisionOvercomeWeight;
        public int SpiritualVisionOvercomeWeight;
    }
}