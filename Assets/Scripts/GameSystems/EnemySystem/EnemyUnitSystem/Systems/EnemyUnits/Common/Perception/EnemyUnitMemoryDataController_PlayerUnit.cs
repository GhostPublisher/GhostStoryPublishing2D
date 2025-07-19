using System.Collections.Generic;

using UnityEngine;

using Foundations.Architecture.ReferencesHandler;

using GameSystems.PlayerSystem;

namespace GameSystems.EnemySystem.EnemyUnitSystem
{
    public class EnemyUnitMemoryDataController_PlayerUnit
    {
        private PlayerUnitManagerDataDBHandler PlayerUnitManagerDataDBHandler;
        private EnemyUnitManagerData myEnemyUnitManagerData;

        // RangeData
        private EnemyUnitRangeData_Visible myEnemyUnitRangeData_Visible;
        // CurrentDetectedData
        private EnemyUnitCurrentMemoryData_PlayerUnit myEnemyUnitCurrentMemoryData_PlayerUnit;
        // MemoryData
        private EnemyUnitMemoryData_PlayerUnit myEnemyUnitMemoryData_PlayerUnit;

        public EnemyUnitMemoryDataController_PlayerUnit(EnemyUnitManagerData enemyUnitManagerData)
        {
            this.myEnemyUnitManagerData = enemyUnitManagerData;

            // GEt
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            this.PlayerUnitManagerDataDBHandler = HandlerManager.GetDynamicDataHandler<PlayerUnitManagerDataDBHandler>();

            if (enemyUnitManagerData.EnemyUnitDynamicData.TryGetEnemyUnitRangeData<EnemyUnitRangeData_Visible>(out var data))
                this.myEnemyUnitRangeData_Visible = data;
            this.myEnemyUnitCurrentMemoryData_PlayerUnit = new();
            this.myEnemyUnitMemoryData_PlayerUnit = new();

            // Set
            this.myEnemyUnitManagerData.EnemyUnitDynamicData.SetEnemyUnitCurrentDetectedData<EnemyUnitCurrentMemoryData_PlayerUnit>(this.myEnemyUnitCurrentMemoryData_PlayerUnit);
            this.myEnemyUnitManagerData.EnemyUnitDynamicData.SetEnemyUnitMemoryData<EnemyUnitMemoryData_PlayerUnit>(this.myEnemyUnitMemoryData_PlayerUnit);
        }

        // 시야 범위 값을 탐색하여, 해당 위치에 PlayerUnit이 있는지 확인.
        public void UpdateCurrentMemoryData_PlayerUnit()
        {
            HashSet<Vector2Int> temp = new();
            foreach (Vector2Int pos in this.myEnemyUnitRangeData_Visible.VisibleRanges)
            {
                // 해당 지역에 Player Unit 이 없으면 넘어감.
                if (!this.PlayerUnitManagerDataDBHandler.TryGetPlayerUnitManagerData(pos, out var _)) continue;

                temp.Add(pos);
            }

            if (temp.Count <= 0)
                this.myEnemyUnitCurrentMemoryData_PlayerUnit.DetectedPositions = null;
            else
                this.myEnemyUnitCurrentMemoryData_PlayerUnit.DetectedPositions = temp;
        }

        // 현재 탐지한 Player Unit 데이터를 사용하여 Memory Player Unit 데이터를 갱신.
        public void UpdateMemoryData_PlayerUnit_WithCurrentDetectedData()
        {
            // 현재 탐지한 Player Unit 데이터가 없으면 리턴.
            if (this.myEnemyUnitCurrentMemoryData_PlayerUnit.DetectedPositions == null) return;

            // 탐지한 Player Unit 위치값들을 사용하여, 기억 속 Player Unit 데이터 갱신.
            foreach (Vector2Int pos in this.myEnemyUnitCurrentMemoryData_PlayerUnit.DetectedPositions)
            {
                // 위치값에 존재하는 Unit 정보 가져옴. ( UniqueID가 필요해서 그럼 => 위에서 null 체크한 것이라 없을 경우는 ㄹㅇ 없음. )
                if (!this.PlayerUnitManagerDataDBHandler.TryGetPlayerUnitManagerData(pos, out var playerUnitManagerData)) continue;

                // 덮어쓰기를 통해 현재 탐지한 정보 바로 기억 데이터에 등록.
                this.myEnemyUnitMemoryData_PlayerUnit.SetDetectedPlayerUnitData(playerUnitManagerData.UniqueID, pos);
            }
        }

        public void UpdateMemoryData_PlayerUnit_TurnFlow()
        {

        }
    }

    public class EnemyUnitCurrentMemoryData_PlayerUnit : IEnemyUnitCurrentMemoryData
    {
        public HashSet<Vector2Int> DetectedPositions { get; set; }
    }

    public class EnemyUnitMemoryData_PlayerUnit : IEnemyUnitMemoryData
    {
        private HashSet<DetectedPlayerUnitData> DetectedPlayerUnitDatas;

        public EnemyUnitMemoryData_PlayerUnit()
        {
            this.DetectedPlayerUnitDatas = new();
        }

        public bool TryGetDetectedPlayerUnitData(int uniqueID, out DetectedPlayerUnitData detectedPlayerUnitData)
        {
            foreach (var data in this.DetectedPlayerUnitDatas)
            {
                if (data.UniqueID == uniqueID)
                {
                    detectedPlayerUnitData = data;
                    return true;
                }
            }

            detectedPlayerUnitData = null;
            return false;
        }
        // 덮어쓰기.
        public void SetDetectedPlayerUnitData(int uniqueID, Vector2Int detectedPosition)
        {
            foreach (var data in this.DetectedPlayerUnitDatas)
            {
                // 이미 있는 경우 값 갱신.
                if (data.UniqueID == uniqueID)
                {
                    data.DetectedPosition = detectedPosition;
                    data.RemainMemoryCount = 3;
                    return;
                }
            }

            // 새로운 값인 경우 그냥 추가.
            this.DetectedPlayerUnitDatas.Add(new DetectedPlayerUnitData(uniqueID, detectedPosition));
        }
    }

    [System.Serializable]
    public class DetectedPlayerUnitData
    {
        public int UniqueID;
        public Vector2Int DetectedPosition;
        public int RemainMemoryCount;

        public DetectedPlayerUnitData(int uniqueID, Vector2Int detectedPosition)
        {
            this.UniqueID = uniqueID;
            this.DetectedPosition = detectedPosition;
            this.RemainMemoryCount = 3;
        }
    }
}