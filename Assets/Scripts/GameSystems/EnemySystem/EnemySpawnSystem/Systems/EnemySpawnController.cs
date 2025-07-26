using System.Collections.Generic;
using UnityEngine;

using Foundations.Architecture.ReferencesHandler;

using GameSystems.TerrainSystem;
using GameSystems.EnemySystem.EnemyUnitSystem;
using GameSystems.PlayerSystem;

namespace GameSystems.EnemySystem.EnemySpawnSystem
{
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

        private Queue<EnemySpawnData> enemySpawnQueue = new();
        
        [SerializeField] private Transform EnemyUnitObjectParent;

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

        public void InitialSetting(int stageID)
        {
            if (this.EnemySpawnDataDBHandler.TryGetEnemySpawnData_Turn(stageID, out var enemySpawnData_Turns))
                this.EnemySpawnData_Turns = enemySpawnData_Turns;

            if (this.EnemySpawnDataDBHandler.TryGetEnemySpawnData_Trigger(stageID, out var enemySpawnData_Triggers))
                this.EnemySpawnData_Triggers = enemySpawnData_Triggers;

            this.GenerateEnemyUnit_StageSetting(stageID);
        }

        public void AllocateTriggerEnemySpawnData_Turn(int turnID)
        {
            EnemySpawnData_Turn enemySpawnData_Turn = null;

            foreach (EnemySpawnData_Turn turnData in EnemySpawnData_Turns)
            {
                if (turnData.TurnID == turnID)
                {
                    enemySpawnData_Turn = turnData;
                }
            }

            foreach (EnemySpawnData spawnData in enemySpawnData_Turn.EnemySpawnDatas)
            {
                this.enemySpawnQueue.Enqueue(spawnData);
            }
        }
        public void AllocateTriggerEnemySpawnData_Trigger(int triggerID)
        {
            EnemySpawnData_Trigger enemySpawnData_Trigger = null;

            foreach (EnemySpawnData_Trigger turnData in EnemySpawnData_Triggers)
            {
                if (turnData.TriggerID == triggerID)
                {
                    enemySpawnData_Trigger = turnData;
                }
            }

            foreach (EnemySpawnData spawnData in enemySpawnData_Trigger.EnemySpawnDatas)
            {
                this.enemySpawnQueue.Enqueue(spawnData);
            }
        }


        // Stage 시작에 의한 Stage 기반 Enemy 생성.
        public void GenerateEnemyUnit_StageSetting(int stageID)
        {
            if (!this.EnemySpawnDataDBHandler.TryGetEnemySpawnData_Stage(stageID, out var data_Stage)) return;

            // 한번에 전달 -> 초반은 생성 애니메이션 보여줄 필요 없음.
            foreach (var data in data_Stage.EnemySpawnDatas)
            {
                this.GenerateEnemyUnit(data.UnitID, new Vector2Int(data.EnemySpawnPositionX, data.EnemySpawnPositionY));
            }

        }
        // 해당 위치 좌표에 Enemy와 동일 좌표에 존재할 수 없는 객체가 있을 시, 생성 안함.
        public void GenerateEnemyUnit()
        {
            if (this.enemySpawnQueue.Count == 0) return;

            EnemySpawnData newSpawnData = null;
            while (this.enemySpawnQueue.Count > 0)
            {
                // 생성할 Unit의 ID 및 Position 가져오기.
                newSpawnData = enemySpawnQueue.Dequeue();

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
            this.enemySpawnQueue.Clear();

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