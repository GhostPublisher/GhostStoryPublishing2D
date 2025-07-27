using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Foundations.Architecture.ReferencesHandler;
using Foundations.Architecture.EventObserver;

using GameSystems.UtilitySystem;

using GameSystems.TerrainSystem;
using GameSystems.StageVisualSystem;
using GameSystems.TilemapSystem.FogTilemap;
using GameSystems.TilemapSystem.MovementTilemap;

using GameSystems.EnemySystem.EnemyVisibilitySystem;
using GameSystems.EnemySystem.EnemySpawnSystem;

using GameSystems.PlayerSystem.PlayerSpawnSystem;
using GameSystems.PlayerSystem.PlayerUnitSystem;

using GameSystems.UIUXSystem;

namespace Example00
{
    public class TestCase_PlayerMove : MonoBehaviour
    {
        private IEventObserverNotifier EventObserverNotifier;

        [Header("Stage 크기")]
        [SerializeField] private int Width;
        [SerializeField] private int Height;

        [Header("지형 Tilemap")]
        [SerializeField] private InitialSetStageVisualResourcesData_EventTest GenerateGroundTilemapEvent_Test;

        [Header("Terrain Data 가중치 설정")]
        [SerializeField] private InitialSetGeneratedTerrainDataEvent_Test InitialSetGeneratedTerrainDataEvent_Test;

        [Header("Enemy 위치 설정")]
        [SerializeField] private List<EnemyUnitSpawnEvent_Test> EnemySpawnEvent_Tests;
        [Header("Player 위치 설정")]
        [SerializeField] private List<PlayerUnitSpawnEvent_Test> PlayerUnitSpawnEvent_Tests;

        [Header("스케너 설정")]
        [SerializeField] private ScannerTestData ScannerTestData;
        [Header("스케너 설정 - Scanner가 움직일 위치지정.")]
        [SerializeField] private List<Vector2Int> TargetMoves;

        [SerializeField] private int testEnemyUniqueID;


        private void Awake()
        {
            this.EventObserverNotifier = new EventObserverNotifier();
        }

        // 지형 Tilemap 생성 / 삭제
        public void Notify_Generate_GroundTilemap()
        {
            this.EventObserverNotifier.NotifyEvent(this.GenerateGroundTilemapEvent_Test);
        }
        public void Notify_Clear_GroundTilemap()
        {
            ClearStageVisualResourcesDataEvent ClearGroundTilemapEvent = new();
            this.EventObserverNotifier.NotifyEvent(ClearGroundTilemapEvent);
        }


        // Terrain Data 할당 / 삭제
        public void Notify_Allocate_GeneratedTerrainData()
        {
            this.EventObserverNotifier.NotifyEvent(this.InitialSetGeneratedTerrainDataEvent_Test);
        }
        public void Notify_Clear_GeneratedTerrainData()
        {
            ClearTrerrainData ClearTrerrainData = new();
            this.EventObserverNotifier.NotifyEvent(ClearTrerrainData);
        }


        // Fog Tilemap 할당 / 삭제 / Hide / Show
        public void Notify_Allocate_FogTilemap()
        {
            InitialSetFogTilemapEvent_Raw InitialSetFogTilemapEvent_Raw = new();
            InitialSetFogTilemapEvent_Raw.Width = this.Width;
            InitialSetFogTilemapEvent_Raw.Height = this.Height;

            this.EventObserverNotifier.NotifyEvent(InitialSetFogTilemapEvent_Raw);
        }
        public void Notify_Clear_FogTilemap()
        {
            ClearFogTilemapEvent ClearFogTilemapEvent = new();

            this.EventObserverNotifier.NotifyEvent(ClearFogTilemapEvent);            
        }
        public void Notify_Hide_FogTilemap()
        {
            HideAllFogTilemap HideAllFogTilemap = new();

            this.EventObserverNotifier.NotifyEvent(HideAllFogTilemap);
        }
        public void Notify_Show_FogTilemap()
        {
            ShowAllFogTilemap ShowAllFogTilemap = new();

            this.EventObserverNotifier.NotifyEvent(ShowAllFogTilemap);
        }


        // Movement Tilemap 할당.
        public void Notify_Allocate_MovementTilemap()
        {
            InitialSetMovementTilemapEvent_Raw InitialSetMovementTilemapEvent_Raw = new();
            InitialSetMovementTilemapEvent_Raw.Width = this.Width;
            InitialSetMovementTilemapEvent_Raw.Height = this.Height;

            this.EventObserverNotifier.NotifyEvent(InitialSetMovementTilemapEvent_Raw);
        }


        // 아군 생성
        public void Notify_SpawnPlayerDatas()
        {
            foreach (var data in this.PlayerUnitSpawnEvent_Tests)
            {
                this.EventObserverNotifier.NotifyEvent(data);
            }
        }
        // 아군 클리어.
        public void Notify_ClearPlayerData()
        {
            PlayerUnitClearEvent playerUnitSpawnEvent_Clear = new();

            this.EventObserverNotifier.NotifyEvent(playerUnitSpawnEvent_Clear);
        }


        // enemy 생성.
        public void Notify_SpawnEnemyDatas()
        {
            foreach(var data in this.EnemySpawnEvent_Tests)
            {
                this.EventObserverNotifier.NotifyEvent(data);
            }
        }
        // Enemy Clear
        public void Notify_ClearEnemyData()
        {
            EnemyUnitClearEvent enemyClearEvent = new();
            this.EventObserverNotifier.NotifyEvent(enemyClearEvent);
        }


        // Scanner를 통한 시야 갱신
        public void Notify_UpdateScanner()
        {
            this.StopAllCoroutines();
            this.StartCoroutine(OperateScannerMove());
        }
        public IEnumerator OperateScannerMove()
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            var FogVisibleRangeCalculator = HandlerManager.GetUtilityHandler<VisibilityRangeCalculator>();

            foreach (Vector2Int target in this.TargetMoves)
            {
                var newVisibleRange = FogVisibleRangeCalculator.GetFilteredVisibleRange_Player(target, this.ScannerTestData.VisibleSize, this.ScannerTestData.VisibleOvercomeWeight);

                UpdateScanerVisibleData_ForFogVisibility data01 = new();
                data01.ScannerUniqueID = this.ScannerTestData.ScannerID;
                data01.VisibleRange = newVisibleRange;

                UpdateScanerVisibleData_ForEnemyVisibility data02 = new();
                data02.ScannerUniqueID = this.ScannerTestData.ScannerID;
                data02.VisibleRange = newVisibleRange;
                data02.PhysicalVisionOvercomeWeight = this.ScannerTestData.PhysicalVisionOvercomeWeight;
                data02.SpiritualVisionOvercomeWeight = this.ScannerTestData.SpiritualVisionOvercomeWeight;

                this.EventObserverNotifier.NotifyEvent(data01);
                this.EventObserverNotifier.NotifyEvent(data02);

                yield return new WaitForSeconds(1f);
            }
        }
        public void Notify_ClearScanner()
        {
            RemoveScanerVisibleData_ForFogVisibility removeScanerVisibleData_ForFogVisibility = new();
            removeScanerVisibleData_ForFogVisibility.ScannerUniqueID = this.ScannerTestData.ScannerID;

            this.EventObserverNotifier.NotifyEvent(removeScanerVisibleData_ForFogVisibility);
        }


        // Player Unit을 통한 시야 갱신
        public void Notify_UpdatePlayerVisible()
        {
            UpdatePlayerUnitVisibleRange UpdatePlayerUnitVisibleRange = new();

            this.EventObserverNotifier.NotifyEvent(UpdatePlayerUnitVisibleRange);
        }

        // Player Unit Action Panel 활성화 / 갱신 / 초기화
        public void Notify_DisplayPlayerUnitActionPanelUIUX()
        {
            ActivatePlayerUnitActionPanelUI DisplayPlayerUnitActionPanelUI = new();

            this.EventObserverNotifier.NotifyEvent(DisplayPlayerUnitActionPanelUI);
        }
        // Player Unit Action Panel 갱신
        public void Notify_UpdatePlayerUnitActionPanelUIUX()
        {
            UpdatePlayerUnitAcitionPanelUI UpdatePlayerUnitAcitionPanelUI = new();

            this.EventObserverNotifier.NotifyEvent(UpdatePlayerUnitAcitionPanelUI);
        }
        // Player Unit Action Panel 초기화
        public void Notify_ClearPlayerUnitAcitionPanelUI()
        {
            ClearPlayerUnitAcitionPanelUI ClearPlayerUnitAcitionPanelUI = new();

            this.EventObserverNotifier.NotifyEvent(ClearPlayerUnitAcitionPanelUI);
        }

        // 단일 AI 실행 테스트
        public void Notify_OperateAI()
        {
            GameSystems.EnemySystem.EnemyUnitSystem.OperateEnemyAI_Raw OperateEnemyAI_Raw = new();
            OperateEnemyAI_Raw.UniqueID = this.testEnemyUniqueID;

            this.EventObserverNotifier.NotifyEvent(OperateEnemyAI_Raw);
        }
        // Enemy AI 들만, 새로운 턴이 시작되었을 때의 값으로 초기화.
        public void Notify_OperateNewTurnSetting()
        {
            GameSystems.EnemySystem.EnemyUnitSystem.OperateNewTurnSetting OperateNewTurnSetting = new();

            this.EventObserverNotifier.NotifyEvent(OperateNewTurnSetting);
        }

        // 모든 AI 순차 실행.
        public void Notify_OperateAISequencer()
        {
            GameSystems.EnemySystem.EnemyAISequenceSystem.AllocateEnemyAISequence AllocateEnemyAISequence = new();
            GameSystems.EnemySystem.EnemyAISequenceSystem.ExecuteEnemyAI ExecuteEnemyAI = new();

            this.EventObserverNotifier.NotifyEvent(AllocateEnemyAISequence);
            this.EventObserverNotifier.NotifyEvent(ExecuteEnemyAI);
        }
        public void Notify_OperateNewTurnSetting_Sequencer()
        {
            GameSystems.EnemySystem.EnemyAISequenceSystem.OperateNewTurnSetting OperateNewTurnSetting = new();

            this.EventObserverNotifier.NotifyEvent(OperateNewTurnSetting);
        }
    }

    [System.Serializable]
    public class ScannerTestData
    {
        [SerializeField] public int ScannerID;
        [SerializeField] public int VisibleSize;
        [SerializeField] public int VisibleOvercomeWeight;
        [SerializeField] public int PhysicalVisionOvercomeWeight;
        [SerializeField] public int SpiritualVisionOvercomeWeight;
    }
}