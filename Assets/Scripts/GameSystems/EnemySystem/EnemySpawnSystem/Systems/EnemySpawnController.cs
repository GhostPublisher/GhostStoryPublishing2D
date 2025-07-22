using System.Collections.Generic;
using UnityEngine;

using Foundations.Architecture.ReferencesHandler;

using GameSystems.TerrainSystem;
using GameSystems.EnemySystem.EnemyUnitSystem;
using GameSystems.PlayerSystem;

namespace GameSystems.EnemySystem.EnemySpawnSystem
{
    public enum EnemySpawnType { None, Turn, Trigger }

    public class EnemySpawnController : MonoBehaviour
    {
        private EnemySpawnDataDBHandler EnemySpawnDataDBHandler;
        private EnemyUnitDataDBHandler EnemyUnitDataDBHandler;

        private GeneratedTerrainDataDBHandler GeneratedTileDataGroupHandler;
        private EnemyUnitManagerDataDBHandler EnemyUnitManagerDataDBHandler;
        private PlayerUnitManagerDataDBHandler PlayerUnitManagerDataDBHandler;

        private UtilitySystem.IsometricCoordinateConvertor IsometricCoordinateConvertor;

        private EnemySpawnData_Turn[] EnemySpawnData_Turns;
        private EnemySpawnData_Trigger[] EnemySpawnData_Triggers;

        private Queue<EnemySpawnData> enemySpawnQueue_Turn = new();
        private Queue<EnemySpawnData> enemySpawnQueue_Trigger = new();

        [SerializeField] private Transform EnemyUnitObjectParent;

        private EnemySpawnType EnemySpawnType;

        private void Awake()
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;

            this.EnemySpawnDataDBHandler = HandlerManager.GetStaticDataHandler<EnemySpawnDataDBHandler>();
            this.EnemyUnitDataDBHandler = HandlerManager.GetStaticDataHandler<EnemyUnitDataDBHandler>();

            this.GeneratedTileDataGroupHandler = HandlerManager.GetDynamicDataHandler<GeneratedTerrainDataDBHandler>();
            this.EnemyUnitManagerDataDBHandler = HandlerManager.GetDynamicDataHandler<EnemyUnitManagerDataDBHandler>();
            this.PlayerUnitManagerDataDBHandler = HandlerManager.GetDynamicDataHandler<PlayerUnitManagerDataDBHandler>();

            this.IsometricCoordinateConvertor = HandlerManager.GetUtilityHandler<UtilitySystem.IsometricCoordinateConvertor>();
        }

        public void AllocateStageEnemySpawnData(int stageID)
        {
            if (this.EnemySpawnDataDBHandler.TryGetEnemySpawnDatas_TurnID(stageID, out var enemySpawnData_Turns))
            {
                this.EnemySpawnData_Turns = enemySpawnData_Turns;
            }

            if (this.EnemySpawnDataDBHandler.TryGetEnemySpawnDatas_TriggerID(stageID, out var enemySpawnData_Positions))
            {
                this.EnemySpawnData_Triggers = enemySpawnData_Positions;
            }
        }

        public void AllocateTriggerEnemySpawnData_Turn(int triggerTurnID)
        {
            EnemySpawnData_Turn enemySpawnData_Turn = null;

            foreach (EnemySpawnData_Turn turnData in EnemySpawnData_Turns)
            {
                if (turnData.TurnID == triggerTurnID)
                {
                    enemySpawnData_Turn = turnData;
                }
            }

            foreach (EnemySpawnData spawnData in enemySpawnData_Turn.EnemySpawnDatas)
            {
                this.enemySpawnQueue_Turn.Enqueue(spawnData);
                Debug.Log($"[Queue] 추가됨: {spawnData.UnitID}");
            }

            this.EnemySpawnType = EnemySpawnType.Turn;
        }
        public void AllocateTriggerEnemySpawnData_Trigger(int triggerPositionID)
        {
            EnemySpawnData_Trigger enemySpawnData_Trigger = null;

            foreach (EnemySpawnData_Trigger turnData in EnemySpawnData_Triggers)
            {
                if (turnData.TriggerID == triggerPositionID)
                {
                    enemySpawnData_Trigger = turnData;
                }
            }

            foreach (EnemySpawnData spawnData in enemySpawnData_Trigger.EnemySpawnDatas)
            {
                this.enemySpawnQueue_Trigger.Enqueue(spawnData);
                Debug.Log($"[Queue] 추가됨: {spawnData.UnitID}");
            }

            this.EnemySpawnType = EnemySpawnType.Trigger;
        }

        public void GenerateEnemyUnit()
        {
            if (this.EnemySpawnType == EnemySpawnType.Turn)
            {
                this.GenerateEnemy_Turn();
            }
            else if (this.EnemySpawnType == EnemySpawnType.Trigger)
            {
                this.GenerateEnemy_Trigger();
            }
            else
            {

            }
        }

        // 이건 Turn 기반 생성, 생성 지점 막혔으면 다음 생성 호출에서 호출.
        public void GenerateEnemy_Turn()
        {
            if (this.enemySpawnQueue_Turn.Count == 0) return;

            EnemySpawnData newSpawnData = null;
            Queue<EnemySpawnData> tmepEnemySpawnQueue = new();
            while (this.enemySpawnQueue_Turn.Count > 0)
            {
                // 생성할 Unit의 ID 및 Position 가져오기.
                newSpawnData = enemySpawnQueue_Turn.Dequeue();

                // 객체가 생성될 위치값이 비어있지 않으면 제외
                if (!this.IsEmpty(new Vector2Int(newSpawnData.EnemySpawnPositionX, newSpawnData.EnemySpawnPositionY)))
                {
                    Debug.Log($"해당 위치값 비어있지 않음.");
                    // 생성 못한 데이터 임시 기록. ( 다음 Turn에 생성할거임 )
                    tmepEnemySpawnQueue.Enqueue(newSpawnData);

                    // 더 이상 생성될 Enemy 데이터가 없으면,
                    if (this.enemySpawnQueue_Turn.Count == 0)
                    {
                        // 생성못한 데이터들 재기입.
                        while (tmepEnemySpawnQueue.Count > 0)
                        {
                            this.enemySpawnQueue_Turn.Enqueue(tmepEnemySpawnQueue.Dequeue());
                        }

                        // 메소드 종료.
                        return;
                    }
                }
                // 객체가 생성될 위치가 비어있음.
                else
                {
                    // 해당 데이터로 결정. 및 반복문 나가기.
                    break;
                }
            }

            this.GenerateEnemyUnit(newSpawnData.UnitID, new Vector2Int(newSpawnData.EnemySpawnPositionX, newSpawnData.EnemySpawnPositionY));
        }
        // 이건 Trigger 기반 생성, 지점 막혔으면 해당 유닛 생성 그냥 넘어감.
        public void GenerateEnemy_Trigger()
        {
            if (this.enemySpawnQueue_Trigger.Count == 0) return;

            EnemySpawnData newSpawnData = null;
            while (this.enemySpawnQueue_Trigger.Count > 0)
            {
                // 생성할 Unit의 ID 및 Position 가져오기.
                newSpawnData = enemySpawnQueue_Trigger.Dequeue();

                // 객체가 생성될 위치값이 비어있으면 생성.
                if (this.IsEmpty(new Vector2Int(newSpawnData.EnemySpawnPositionX, newSpawnData.EnemySpawnPositionY))) break;
            }

            this.GenerateEnemyUnit(newSpawnData.UnitID, new Vector2Int(newSpawnData.EnemySpawnPositionX, newSpawnData.EnemySpawnPositionY));
        }


        public void GenerateEnemyUnit(int unitID, Vector2Int spawnPosition)
        {
            // 생성할 Enemy Prefab 가져오기.
            GameObject prefab = this.EnemyUnitDataDBHandler.GetEnemyPrefabResourceData(unitID).EnemyPrefab;

            // Enemy Unit 생성 및 Hierarchy 배치.
            GameObject createdEnemyUnit = Object.Instantiate(prefab, this.EnemyUnitObjectParent);
            // 위치 배치.
            createdEnemyUnit.transform.position = this.IsometricCoordinateConvertor.ConvertGridToWorld(spawnPosition);

            // Enemy Unit 초기 할당 성공.
            if(createdEnemyUnit.GetComponent<IEnemyUnitManager>().TryInitialSetting(out var data))
            {
                // Enemy 관리 데이터에 할당.
                this.EnemyUnitManagerDataDBHandler.AddEnemyUnitManagerData(data);
            }
            // 적 생성 오류 남.
            else
            {
                GameObject.Destroy(createdEnemyUnit);
            }
        }

        public void ClearGameObjectAndDatas()
        {
            this.EnemyUnitManagerDataDBHandler.ClearEnemyUnitManagerDataGroup();
            this.enemySpawnQueue_Turn.Clear();
            this.enemySpawnQueue_Trigger.Clear();

            // enemyGameObject 삭제.
            foreach (Transform child in EnemyUnitObjectParent)
            {
                GameObject.Destroy(child.gameObject);
            }
        }
        
        // 없으면 True, 있으면 false
        private bool IsEmpty(Vector2Int pos)
        {
            // 존재할 수 없는 위치값인지 확인. ( 해당 위치의 tile 값을 가져올 수 없으면, 해당 위치는 배치 불가능한 곳임 )
            if (this.GeneratedTileDataGroupHandler.TryGetGeneratedTerrainData(pos, out var _))
            {
                return false;
            }

            if(this.PlayerUnitManagerDataDBHandler.TryGetPlayerUnitManagerData(pos, out var _))
            {
                return false;
            }

            if (this.EnemyUnitManagerDataDBHandler.TryGetEnemyUnitManagerData(pos, out var _))
            {
                return false;
            }

            return true;
        }
    }
}