using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Foundations.Architecture.ReferencesHandler;

using GameSystems.PlayerSystem.PlayerUnitSystem;

namespace GameSystems.PlayerSystem.PlayerSpawnSystem
{
    public interface IPlayerUnitSpawnController
    {
        // 초기 셋팅 ( 필드 멤버 셋팅 등 )
        public void InitialSetting(int stageID);

        // stageID 기반 Player Unit 생성 데이터 할당
        public void AllocatePlayerUnitSpawnData_Stage();
        // triggerID 기반 Player Unit 생성 데이터 할당
        public bool TryAllocatePlayerUnitSpawnData_Trigger(int triggerID);

        // Queue를 사용한 PlayerUnit 생성 요청
        public void GeneratePlayerUnit_Queue();
        // Queue를 사용한 PlayerUnit 생성 요청 ( 내부 코루틴 대기 )
        public IEnumerator GeneratePlayerUnit_Queue_Coroutine();

        // PlayerUnit 생성 요청.
        public void GeneratePlayerUnit(int unitID, Vector2Int spawnPosition);
        // PlayerUnit 생성 요청 ( 내부 코루틴 대기 )
        public IEnumerator GeneratePlayerUnit_Coroutine(int unitID, Vector2Int spawnPosition);

        public void StopAllCoroutines_Refer();

        // 모든 PlayerUnit 객체 삭제 요청.
        public void ClearPlayerUnitAndDatas();
    }

    public class PlayerUnitSpawnController : MonoBehaviour, IPlayerUnitSpawnController
    {
        [SerializeField] private Transform PlayerUnitObjectParent;

        private PlayerUnitSpawnData_Stage PlayerUnitSpawnData_Stage;
        private PlayerUnitSpawnData_Trigger[] PlayerUnitSpawnData_Triggers;

        private Queue<UnitSpawnData> playerSpawnQueue = new();

        private void Awake()
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            HandlerManager.GetDynamicDataHandler<PlayerSystemHandler>().IPlayerUnitSpawnController = this;
        }

        public void InitialSetting(int stageID)
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            var PlayerUnitSpawnDataDBHandler = HandlerManager.GetStaticDataHandler<PlayerUnitSpawnDataDBHandler>();

            if (PlayerUnitSpawnDataDBHandler.TryGetPlayerUnitSpawnData_Stage(stageID, out var playerUnitSpawnData_Stage))
                this.PlayerUnitSpawnData_Stage = playerUnitSpawnData_Stage;

            // 특정 StageID에 해당되는 Trigger 기반 적 유닛 생성 Table을 가져옵니다.
            if (PlayerUnitSpawnDataDBHandler.TryGetPlayerUnitSpawnDataGroup_Trigger(stageID, out var playerUnitSpawnData_Triggers))
                this.PlayerUnitSpawnData_Triggers = playerUnitSpawnData_Triggers;
        }


        // StageID에 대응되는 PlayerUnitSpawnData를 Queue에 할당합니다.
        public void AllocatePlayerUnitSpawnData_Stage()
        {
            this.playerSpawnQueue.Clear();
            foreach (UnitSpawnData spawnData in this.PlayerUnitSpawnData_Stage.UnitSpawnDatas)
            {
                this.playerSpawnQueue.Enqueue(spawnData);
            }
        }
        // 특정 TrrigerID에 대응되는 PlayerUnitSpawnData를 Queue에 할당합니다.
        public bool TryAllocatePlayerUnitSpawnData_Trigger(int triggerID)
        {
            if (this.PlayerUnitSpawnData_Triggers == null) return false;

            // DB에 TriggerID에 대응되는 '아군 유닛 생성 데이터'가 있는지 탐색.
            foreach (var spawnDataGroup in this.PlayerUnitSpawnData_Triggers)
            {
                if (spawnDataGroup.TriggerID == triggerID)
                {
                    this.playerSpawnQueue.Clear();  

                    foreach (UnitSpawnData spawnData in spawnDataGroup.UnitSpawnDatas)
                    {
                        this.playerSpawnQueue.Enqueue(spawnData);
                    }
                }

                return true;
            }

            // 없으면, TriggerID에 대응되는 데이터가 없다는 뜻을 리턴.
            return false;
        }


        // Queue를 사용한 PlayerUnit 생성 요청
        public void GeneratePlayerUnit_Queue()
        {
            while (this.playerSpawnQueue.Count > 0)
            {
                // 생성할 Unit의 ID 및 Position 가져오기.
                var newSpawnData = this.playerSpawnQueue.Dequeue();
                Vector2Int spawnPosition = new Vector2Int(newSpawnData.SpawnPositionX, newSpawnData.SpawnPositionY);

                // 객체가 생성될 위치값이 비어있지 않으면 넘어감..
                if (!this.IsEmpty(spawnPosition)) continue;

                this.GeneratePlayerUnit(newSpawnData.UnitID, spawnPosition);
            }
        }
        // Queue를 사용한 PlayerUnit 생성 요청 ( 내부 코루틴 대기 )
        public IEnumerator GeneratePlayerUnit_Queue_Coroutine()
        {
            while (this.playerSpawnQueue.Count > 0)
            {
                // 생성할 Unit의 ID 및 Position 가져오기.
                var newSpawnData = this.playerSpawnQueue.Dequeue();
                Vector2Int spawnPosition = new Vector2Int(newSpawnData.SpawnPositionX, newSpawnData.SpawnPositionY);

                // 객체가 생성될 위치값이 비어있지 않으면 넘어감..
                if (!this.IsEmpty(spawnPosition)) continue;

                yield return this.GeneratePlayerUnit_Coroutine(newSpawnData.UnitID, spawnPosition);

                // 잠시 대기.
                yield return new WaitForSeconds(0.5f);
            }
        }


        // 매개변수로 받은 ID와 Position에 대응되는 PlayerUnit을 배치.
        public void GeneratePlayerUnit(int unitID, Vector2Int spawnPosition)
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            var PlayerUnitDataDBHandler = HandlerManager.GetStaticDataHandler<PlayerUnitDataDBHandler>();
            var IsometricCoordinateConvertor = HandlerManager.GetUtilityHandler<UtilitySystem.IsometricCoordinateConvertor>();

            if (!PlayerUnitDataDBHandler.TryGetPlayerUnitResourceData(unitID, out var prefabData)) return;

            // 객체 생성
            GameObject createdPlayerUnit = MonoBehaviour.Instantiate(prefabData.UnitPrefab, this.PlayerUnitObjectParent);
            // 객체 위치 지정.
            createdPlayerUnit.transform.localPosition = IsometricCoordinateConvertor.ConvertGridToWorld(spawnPosition);

            // 초기 셋팅 호출 후 끝.
            createdPlayerUnit.GetComponent<PlayerUnitSystem.IPlayerUnitManager>().OperatePlayerUnitInitialSetting();
        }
        public IEnumerator GeneratePlayerUnit_Coroutine(int unitID, Vector2Int spawnPosition)
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            var PlayerUnitDataDBHandler = HandlerManager.GetStaticDataHandler<PlayerUnitDataDBHandler>();
            var IsometricCoordinateConvertor = HandlerManager.GetUtilityHandler<UtilitySystem.IsometricCoordinateConvertor>();

            if (!PlayerUnitDataDBHandler.TryGetPlayerUnitResourceData(unitID, out var prefabData)) yield break;

            // 객체 생성
            GameObject createdPlayerUnit = MonoBehaviour.Instantiate(prefabData.UnitPrefab, this.PlayerUnitObjectParent);
            var playerUnitManager =  createdPlayerUnit.GetComponent<PlayerUnitSystem.IPlayerUnitManager>();
            // 인터페이스 가져오기.
            // 객체 위치 지정.
            createdPlayerUnit.transform.localPosition = IsometricCoordinateConvertor.ConvertGridToWorld(spawnPosition);

            // 1프레임 대기. ( 객체 생성되는 시점까지 대기 후, 생성 Coroutine 호출. ) 
            yield return null;

            // PlayerUnit 초기 셋팅 요청. -> 생성 작업 대기.
            playerUnitManager.StopAllCoroutines();
            yield return StartCoroutine(playerUnitManager.OperatePlayerUnitInitialSetting_Coroutine());

            // 1프레임 대기. ( 그냥 안전성 )
            yield return null;
        }


        public void StopAllCoroutines_Refer()
        {
            this.StopAllCoroutines();
        }

        // 생성된 모든 PlayerUnit GameObject를 삭제 및 관련 데이터 삭제.
        public void ClearPlayerUnitAndDatas()
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            var PlayerUnitManagerDataDBHandler = HandlerManager.GetDynamicDataHandler<PlayerUnitManagerDataDBHandler>();

            PlayerUnitManagerDataDBHandler.ClearPlayerUnitManagerDataGroup();
            this.playerSpawnQueue.Clear();

            // enemyGameObject 삭제.
            foreach (Transform child in PlayerUnitObjectParent)
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
            var PlayerUnitManagerDataDBHandler = HandlerManager.GetDynamicDataHandler<PlayerUnitManagerDataDBHandler>();
            var EnemyUnitManagerDataDBHandler = HandlerManager.GetDynamicDataHandler<EnemySystem.EnemyUnitSystem.EnemyUnitManagerDataDBHandler>();

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