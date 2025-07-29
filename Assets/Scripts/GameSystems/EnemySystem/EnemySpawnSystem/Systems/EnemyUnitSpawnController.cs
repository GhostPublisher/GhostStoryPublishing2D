using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Foundations.Architecture.ReferencesHandler;

namespace GameSystems.EnemySystem.EnemySpawnSystem
{
    public interface IEnemyUnitSpawnController
    {
        // 초기 셋팅 ( 필드 멤버 셋팅 등 )
        public void InitialSetting(int stageID);

        // stageID 기반 Enemy Unit 생성 데이터 할당
        public void AllocateEnemyUnitSpawnData_Stage();
        // turnID 기반 Enemy Unit 생성 데이터 할당
        public bool TryAllocateEnemyUnitSpawnData_Turn(int turnID);
        // triggerID 기반 Enemy Unit 생성 데이터 할당
        public bool TryAllocateEnemyUnitSpawnData_Trigger(int triggerID);

        // Queue를 사용한 EnemyUnit 생성 요청
        public void GenerateEnemyUnit_Queue();
        // Queue를 사용한 EnemyUnit 생성 요청 ( 내부 코루틴 대기 )
        public IEnumerator GenerateEnemyUnit_Queue_Coroutine();

        // EnemyUnit 생성 요청.
        public void GenerateEnemyUnit(int unitID, Vector2Int spawnPosition);
        // EnemyUnit 생성 요청 ( 내부 코루틴 대기 )
        public IEnumerator GenerateEnemyUnit_Coroutine(int unitID, Vector2Int spawnPosition);

        public void StopAllCoroutines_Refer();

        // 모든 EnemyUnit 객체 삭제 요청.
        public void ClearEnemyUnitAndDatas();
    }

    public class EnemyUnitSpawnController : MonoBehaviour, IEnemyUnitSpawnController
    {
        [SerializeField] private Transform EnemyUnitObjectParent;

        private EnemyUnitSpawnData_Stage EnemyUnitSpawnData_Stage;
        private EnemyUnitSpawnData_Turn[] EnemySpawnData_Turns;
        private EnemyUnitSpawnData_Trigger[] EnemySpawnData_Triggers;

        private Queue<UnitSpawnData> enemySpawnQueue = new();

        private void Awake()
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            HandlerManager.GetDynamicDataHandler<EnemySystemHandler>().IEnemyUnitSpawnController = this;
        }

        // 초기 셋팅 ( 필드 멤버 셋팅 등 )
        public void InitialSetting(int stageID)
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            var EnemyUnitSpawnDataDBHandler = HandlerManager.GetStaticDataHandler<EnemyUnitSpawnDataDBHandler>();

            // 특정 StageID에 해당되는 StageID 기반 적 유닛 생성 Table을 가져옵니다.
            if (EnemyUnitSpawnDataDBHandler.TryGetEnemySpawnData_Stage(stageID, out var enemyUnitSpawnData_Stage))
                this.EnemyUnitSpawnData_Stage = enemyUnitSpawnData_Stage;

            // 특정 StageID에 해당되는 Turn 기반 적 유닛 생성 Table을 가져옵니다.
            if (EnemyUnitSpawnDataDBHandler.TryGetEnemyUnitSpawnDataGroup_Turn(stageID, out var enemySpawnData_Turns))
                this.EnemySpawnData_Turns = enemySpawnData_Turns;

            // 특정 StageID에 해당되는 Trigger 기반 적 유닛 생성 Table을 가져옵니다.
            if (EnemyUnitSpawnDataDBHandler.TryGetEnemyUnitSpawnDataGroup_Trigger(stageID, out var enemySpawnData_Triggers))
                this.EnemySpawnData_Triggers = enemySpawnData_Triggers;
        }


        // StageID에 대응되는 EnemyUnitSpawnData를 Queue에 할당합니다.
        public void AllocateEnemyUnitSpawnData_Stage()
        {
            this.enemySpawnQueue.Clear();
            foreach (UnitSpawnData spawnData in this.EnemyUnitSpawnData_Stage.UnitSpawnDatas)
            {
                this.enemySpawnQueue.Enqueue(spawnData);
            }
        }
        // 특정 TrrigerID에 대응되는 EnemyUnitSpawnData를 Queue에 할당합니다.
        public bool TryAllocateEnemyUnitSpawnData_Turn(int turnID)
        {
            if (this.EnemySpawnData_Turns == null) return false;

            // DB에 TurnID에 대응되는 '적 유닛 생성 데이터'가 있는지 탐색.
            foreach (var spawnDataGroup in this.EnemySpawnData_Turns)
            {
                // 있으면, Queue에 데이터 할당.
                if (spawnDataGroup.TurnID == turnID)
                {
                    this.enemySpawnQueue.Clear();

                    foreach (UnitSpawnData spawnData in spawnDataGroup.UnitSpawnDatas)
                    {
                        this.enemySpawnQueue.Enqueue(spawnData);
                    }

                    return true;
                }
            }

            // 없으면, TurnID에 대응되는 데이터가 없다는 뜻을 리턴.
            return false;
        }
        // 특정 TrrigerID에 대응되는 EnemyUnitSpawnData를 Queue에 할당합니다.
        public bool TryAllocateEnemyUnitSpawnData_Trigger(int triggerID)
        {
            if (this.EnemySpawnData_Triggers == null) return false;

            // DB에 TriggerID에 대응되는 '적 유닛 생성 데이터'가 있는지 탐색.
            foreach (var spawnDataGroup in this.EnemySpawnData_Triggers)
            {
                // 있으면, Queue에 데이터 할당.
                if (spawnDataGroup.TriggerID == triggerID)
                {
                    this.enemySpawnQueue.Clear();   

                    foreach (UnitSpawnData spawnData in spawnDataGroup.UnitSpawnDatas)
                    {
                        this.enemySpawnQueue.Enqueue(spawnData);
                    }
                }

                return true;
            }

            // 없으면, TriggerID에 대응되는 데이터가 없다는 뜻을 리턴.
            return false;
        }


        public void GenerateEnemyUnit_Queue()
        {
            while (this.enemySpawnQueue.Count > 0)
            {
                // 생성할 Unit의 ID 및 Position 가져오기.
                var newSpawnData = this.enemySpawnQueue.Dequeue();
                Vector2Int spawnPosition = new Vector2Int(newSpawnData.SpawnPositionX, newSpawnData.SpawnPositionY);

                // 객체가 생성될 위치값이 비어있지 않으면 넘어감..
                if (!this.IsEmpty(spawnPosition)) continue;

                this.GenerateEnemyUnit(newSpawnData.UnitID, spawnPosition);
            }
        }
        public IEnumerator GenerateEnemyUnit_Queue_Coroutine()
        {
            while (this.enemySpawnQueue.Count > 0)
            {
                // 생성할 Unit의 ID 및 Position 가져오기.
                var newSpawnData = enemySpawnQueue.Dequeue();
                Vector2Int spawnPosition = new Vector2Int(newSpawnData.SpawnPositionX, newSpawnData.SpawnPositionY);

                // 객체가 생성될 위치값이 비어있지 않으면 넘어감..
                if (!this.IsEmpty(spawnPosition)) continue;

                yield return this.GenerateEnemyUnit_Coroutine(newSpawnData.UnitID, spawnPosition);
                
                // 잠시 대기.
                yield return new WaitForSeconds(0.5f);
            }
        }

        // 코루틴이 아닌 생성. ( 생성 Animation 무시 )
        public void GenerateEnemyUnit(int unitID, Vector2Int spawnPosition)
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            var EnemyUnitDataDBHandler = HandlerManager.GetStaticDataHandler<EnemyUnitSystem.EnemyUnitDataDBHandler>();
            var IsometricCoordinateConvertor = HandlerManager.GetUtilityHandler<UtilitySystem.IsometricCoordinateConvertor>();

            // UnitID에 대응되는 Prefab이 없으면 넘어감.
            if (!EnemyUnitDataDBHandler.TryGetEnemyPrefabResourceData(unitID, out var prefabData)) return;

            Debug.Log($"ID : {unitID}, pos : {spawnPosition}");

            // Enemy Unit 생성 및 Hierarchy 배치.
            GameObject createdEnemyUnit = Object.Instantiate(prefabData.EnemyPrefab, this.EnemyUnitObjectParent);
            // 위치 배치.
            createdEnemyUnit.transform.position = IsometricCoordinateConvertor.ConvertGridToWorld(spawnPosition);

            // 초기 셋팅 호출 후 끝.
            createdEnemyUnit.GetComponent<EnemyUnitSystem.IEnemyUnitManager>().OperateEnemyUnitInitialSetting();
        }
        // 코루틴 생성. ( 애니메이션 대기 )
        public IEnumerator GenerateEnemyUnit_Coroutine(int unitID, Vector2Int spawnPosition)
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            var EnemyUnitDataDBHandler = HandlerManager.GetStaticDataHandler<EnemyUnitSystem.EnemyUnitDataDBHandler>();
            var IsometricCoordinateConvertor = HandlerManager.GetUtilityHandler<UtilitySystem.IsometricCoordinateConvertor>();

            // UnitID에 대응되는 Prefab이 없으면 넘어감.
            if (!EnemyUnitDataDBHandler.TryGetEnemyPrefabResourceData(unitID, out var prefabData)) yield break;

            // Enemy Unit 생성 및 Hierarchy 배치.
            GameObject createdEnemyUnit = Object.Instantiate(prefabData.EnemyPrefab, this.EnemyUnitObjectParent);
            var enemyUnitManager = createdEnemyUnit.GetComponent<EnemyUnitSystem.IEnemyUnitManager>();
            // 위치 배치.
            createdEnemyUnit.transform.position = IsometricCoordinateConvertor.ConvertGridToWorld(spawnPosition);

            // 1프레임 대기. ( 객체 생성되는 시점까지 대기 후, 생성 Coroutine 호출. ) 
            yield return null;

            // Enemy 초기 셋팅 값 넘기기.
            // 여기서 Enemy 객체의 생성 코루틴 대기.
            enemyUnitManager.StopAllCoroutines();
            yield return StartCoroutine(enemyUnitManager.OperateEnemyUnitInitialSetting_Coroutine());

            // 1프레임 대기. ( 그냥 안전성 )
            yield return null;
        }


        public void StopAllCoroutines_Refer()
        {
            this.StopAllCoroutines();
        }

        // 생성된 모든 EnemyUnit GameObject를 삭제 및 관련 데이터 삭제.
        public void ClearEnemyUnitAndDatas()
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            var EnemyUnitManagerDataDBHandler = HandlerManager.GetDynamicDataHandler<EnemyUnitSystem.EnemyUnitManagerDataDBHandler>();

            EnemyUnitManagerDataDBHandler.ClearEnemyUnitManagerDataGroup();
            this.enemySpawnQueue.Clear();

            // enemyGameObject 삭제.
            foreach (Transform child in EnemyUnitObjectParent)
            {
                GameObject.Destroy(child.gameObject);
            }
        }


        // 해당 위치 좌표에 Enemy와 동일 좌표에 존재할 수 없는 객체가 있을 시, 생성 안함.
        // 없으면 True, 있으면 false
        private bool IsEmpty(Vector2Int pos)
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            var GeneratedTileDataGroupHandler = HandlerManager.GetDynamicDataHandler<TerrainSystem.GeneratedTerrainDataDBHandler>();
            var PlayerUnitManagerDataDBHandler = HandlerManager.GetDynamicDataHandler<PlayerSystem.PlayerUnitManagerDataDBHandler>();
            var EnemyUnitManagerDataDBHandler = HandlerManager.GetDynamicDataHandler<EnemyUnitSystem.EnemyUnitManagerDataDBHandler>();

            // 존재할 수 없는 위치값인지 확인. ( 해당 위치의 tile 값을 가져올 수 없으면, 해당 위치는 배치 불가능한 곳임 )
            if (!GeneratedTileDataGroupHandler.TryGetGeneratedTerrainData(pos, out var _))
            {
                return false;
            }

            if (PlayerUnitManagerDataDBHandler.TryGetPlayerUnitManagerData(pos, out var _))
            {
                return false;
            }

            if (EnemyUnitManagerDataDBHandler.TryGetEnemyUnitManagerData(pos, out var _))
            {
                return false;
            }

            return true;
        }

    }
}