using System.Collections.Generic;
using UnityEngine;

using Foundations.Architecture.ReferencesHandler;

using GameSystems.TerrainSystem;
using GameSystems.EnemySystem.EnemyUnitSystem;
using GameSystems.PlayerSystem;

namespace GameSystems.EnemySystem.EnemySpawnSystem
{
    public class EnemyUnitSpawnController : MonoBehaviour
    {
        private EnemyUnitSpawnDataDBHandler EnemyUnitSpawnDataDBHandler;
        private EnemyUnitDataDBHandler EnemyUnitDataDBHandler;

        private GeneratedTerrainDataDBHandler GeneratedTileDataGroupHandler;
        private EnemyUnitManagerDataDBHandler EnemyUnitManagerDataDBHandler;
        private PlayerUnitManagerDataDBHandler PlayerUnitManagerDataDBHandler;

        private UtilitySystem.IsometricCoordinateConvertor IsometricCoordinateConvertor;

        [SerializeField] private Transform EnemyUnitObjectParent;

        private EnemyUnitSpawnData_Turn[] EnemySpawnData_Turns;
        private EnemyUnitSpawnData_Trigger[] EnemySpawnData_Triggers;

        private Queue<UnitSpawnData> enemySpawnQueue = new();        

        private void Awake()
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;

            this.EnemyUnitSpawnDataDBHandler = HandlerManager.GetStaticDataHandler<EnemyUnitSpawnDataDBHandler>();
            this.EnemyUnitDataDBHandler = HandlerManager.GetStaticDataHandler<EnemyUnitDataDBHandler>();

            this.GeneratedTileDataGroupHandler = HandlerManager.GetDynamicDataHandler<GeneratedTerrainDataDBHandler>();
            this.EnemyUnitManagerDataDBHandler = HandlerManager.GetDynamicDataHandler<EnemyUnitManagerDataDBHandler>();
            this.PlayerUnitManagerDataDBHandler = HandlerManager.GetDynamicDataHandler<PlayerUnitManagerDataDBHandler>();

            this.IsometricCoordinateConvertor = HandlerManager.GetUtilityHandler<UtilitySystem.IsometricCoordinateConvertor>();
        }

        public void InitialSetting(int stageID)
        {
            // 특정 StageID에 해당되는 Turn Table을 가져옵니다.
            if (this.EnemyUnitSpawnDataDBHandler.TryGetEnemyUnitSpawnDataGroup_Turn(stageID, out var enemySpawnData_Turns))
                this.EnemySpawnData_Turns = enemySpawnData_Turns;

            // 특정 StageID에 해당되는 Trigger Table을 가져옵니다.
            if (this.EnemyUnitSpawnDataDBHandler.TryGetEnemyUnitSpawnDataGroup_Trigger(stageID, out var enemySpawnData_Triggers))
                this.EnemySpawnData_Triggers = enemySpawnData_Triggers;

            this.enemySpawnQueue.Clear();
            this.GenerateEnemyUnit_StageSetting(stageID);
        }
        // 특정 TrrigerID에 대응되는 EnemyUnitSpawnData를 Queue에 할당합니다.
        public void AllocateEnemyUnitSpawnData_Turn(int turnID)
        {
            this.enemySpawnQueue.Clear();
            EnemyUnitSpawnData_Turn enemyUnitSpawnData_Turn = null;

            foreach (var spawnDataGroup in this.EnemySpawnData_Turns)
            {
                if (spawnDataGroup.TurnID == turnID)
                {
                    enemyUnitSpawnData_Turn = spawnDataGroup;
                }
            }

            foreach (UnitSpawnData spawnData in enemyUnitSpawnData_Turn.UnitSpawnDatas)
            {
                this.enemySpawnQueue.Enqueue(spawnData);
            }
        }
        // 특정 TrrigerID에 대응되는 EnemyUnitSpawnData를 Queue에 할당합니다.
        public void AllocateEnemyUnitSpawnData_Trigger(int triggerID)
        {
            this.enemySpawnQueue.Clear();
            EnemyUnitSpawnData_Trigger enemyUnitSpawnData_Trigger = null;

            foreach (var spawnDataGroup in this.EnemySpawnData_Triggers)
            {
                if (spawnDataGroup.TriggerID == triggerID)
                {
                    enemyUnitSpawnData_Trigger = spawnDataGroup;
                }
            }

            foreach (UnitSpawnData spawnData in enemyUnitSpawnData_Trigger.UnitSpawnDatas)
            {
                this.enemySpawnQueue.Enqueue(spawnData);
            }
        }


        // Stage 시작에 의한 Stage 기반 Enemy 생성.
        public void GenerateEnemyUnit_StageSetting(int stageID)
        {
            if (!this.EnemyUnitSpawnDataDBHandler.TryGetEnemySpawnData_Stage(stageID, out var data_Stage)) return;

            // 한번에 전달 -> 초반은 생성 애니메이션 보여줄 필요 없음.
            foreach (var data in data_Stage.UnitSpawnDatas)
            {
                this.GenerateEnemyUnit(data.UnitID, new Vector2Int(data.SpawnPositionX, data.SpawnPositionY));
            }

        }
        // 해당 위치 좌표에 Enemy와 동일 좌표에 존재할 수 없는 객체가 있을 시, 생성 안함.
        public void GenerateEnemyUnit()
        {
            if (this.enemySpawnQueue.Count == 0) return;

            UnitSpawnData newSpawnData = null;
            while (this.enemySpawnQueue.Count > 0)
            {
                // 생성할 Unit의 ID 및 Position 가져오기.
                newSpawnData = enemySpawnQueue.Dequeue();

                // 객체가 생성될 위치값이 비어있으면 생성.
                if (this.IsEmpty(new Vector2Int(newSpawnData.SpawnPositionX, newSpawnData.SpawnPositionY))) break;
            }

            this.GenerateEnemyUnit(newSpawnData.UnitID, new Vector2Int(newSpawnData.SpawnPositionX, newSpawnData.SpawnPositionY));
        }
        public void GenerateEnemyUnit(int unitID, Vector2Int spawnPosition)
        {
            if (!this.EnemyUnitDataDBHandler.TryGetEnemyPrefabResourceData(unitID, out var prefabData)) return;

            // Enemy Unit 생성 및 Hierarchy 배치.
            GameObject createdEnemyUnit = Object.Instantiate(prefabData.EnemyPrefab, this.EnemyUnitObjectParent);
            // 위치 배치.
            createdEnemyUnit.transform.position = this.IsometricCoordinateConvertor.ConvertGridToWorld(spawnPosition);

            // Enemy Unit 초기 할당 성공.
            if(createdEnemyUnit.GetComponent<IEnemyUnitManager>().TryInitialSetting(out var data))
            {
                // Enemy 관리 데이터에 할당.
                this.EnemyUnitManagerDataDBHandler.AddEnemyUnitManagerData(data);
            }
            // 이거 오류 크게 난거임. 다시 삭제해야됨.
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