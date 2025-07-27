using System.Collections.Generic;

using UnityEngine;

using Foundations.Architecture.ReferencesHandler;

using GameSystems.PlayerSystem.PlayerUnitSystem;

namespace GameSystems.PlayerSystem.PlayerSpawnSystem
{
    public class PlayerUnitSpawnController : MonoBehaviour
    {
        private PlayerUnitSpawnDataDBHandler PlayerUnitSpawnDataDBHandler;
        private PlayerUnitDataDBHandler PlayerUnitDataDBHandler;

        private PlayerUnitManagerDataDBHandler GeneratedPlayerUnitDataGroupHandler;

        private UtilitySystem.IsometricCoordinateConvertor IsometricCoordinateConvertor;

        [SerializeField] private Transform PlayerUnitObjectParent;

        private PlayerUnitSpawnData_Trigger[] PlayerUnitSpawnData_Triggers;

        private Queue<UnitSpawnData> playerSpawnQueue = new();

        private void Awake()
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;

            this.PlayerUnitSpawnDataDBHandler = HandlerManager.GetStaticDataHandler<PlayerUnitSpawnDataDBHandler>();
            this.PlayerUnitDataDBHandler = HandlerManager.GetStaticDataHandler<PlayerUnitDataDBHandler>();

            this.GeneratedPlayerUnitDataGroupHandler = HandlerManager.GetDynamicDataHandler<PlayerUnitManagerDataDBHandler>();

            this.IsometricCoordinateConvertor = HandlerManager.GetUtilityHandler<UtilitySystem.IsometricCoordinateConvertor>();
        }

        public void InitialSetting(int stageID)
        {
            // 특정 StageID에 해당되는 Trigger Table을 가져옵니다.
            if (this.PlayerUnitSpawnDataDBHandler.TryGetPlayerUnitSpawnDataGroup_Trigger(stageID, out var playerUnitSpawnData_Triggers))
                this.PlayerUnitSpawnData_Triggers = playerUnitSpawnData_Triggers;

            this.playerSpawnQueue.Clear();
            this.GeneratePlayerUnit_StageSetting(stageID);
        }

        // 특정 TrrigerID에 대응되는 PlayerUnitSpawnData를 Queue에 할당합니다.
        public void AllocatePlayerUnitSpawnData_Trigger(int triggerID)
        {
            this.playerSpawnQueue.Clear();
            PlayerUnitSpawnData_Trigger playerUnitSpawnData_Trigger = null;

            foreach (var spawnDataGroup in this.PlayerUnitSpawnData_Triggers)
            {
                if (spawnDataGroup.TriggerID == triggerID)
                {
                    playerUnitSpawnData_Trigger = spawnDataGroup;
                }
            }

            foreach (UnitSpawnData spawnData in playerUnitSpawnData_Trigger.UnitSpawnDatas)
            {
                this.playerSpawnQueue.Enqueue(spawnData);
            }
        }

        public void GeneratePlayerUnit_StageSetting(int stageID)
        {
            if (!this.PlayerUnitSpawnDataDBHandler.TryGetPlayerUnitSpawnData_Stage(stageID, out var data_Stage)) return;

            // 한번에 전달 -> 초반은 생성 애니메이션 보여줄 필요 없음.
            foreach (var data in data_Stage.UnitSpawnDatas)
            {
                this.GeneratePlayerUnit(data.UnitID, new Vector2Int(data.SpawnPositionX, data.SpawnPositionY));
            }
        }
        // 할당되어 있는 PlayerSpawnData을 순서대로 호출.
        public void GeneratePlayerUnit()
        {
            if (this.playerSpawnQueue.Count == 0) return;

            // Prefab 가져오기.
            UnitSpawnData playerSpawnData = this.playerSpawnQueue.Dequeue();
            this.GeneratePlayerUnit(playerSpawnData.UnitID, new Vector2Int(playerSpawnData.SpawnPositionX, playerSpawnData.SpawnPositionY));
        }
        // 매개변수로 받은 ID와 Position에 대응되는 PlayerUnit을 배치.
        public void GeneratePlayerUnit(int unitID, Vector2Int spawnPosition)
        {
            if (!this.PlayerUnitDataDBHandler.TryGetPlayerUnitResourceData(unitID, out var prefabData)) return;

            // 객체 생성
            GameObject createdPlayerUnit = Object.Instantiate(prefabData.UnitPrefab, this.PlayerUnitObjectParent);
            // 객체 위치 지정.
            createdPlayerUnit.transform.localPosition = this.IsometricCoordinateConvertor.ConvertGridToWorld(spawnPosition);

            // Player Unit 초기 할당.
            if (createdPlayerUnit.GetComponent<IPlayerUnitManager>().TryInitialSetting(out var newPlayerUnitManagerData))
            {
                // Player 관리 데이터에 할당.
                this.GeneratedPlayerUnitDataGroupHandler.AddPlayerUnitManagerData(newPlayerUnitManagerData);
            }
            // 이거 오류 크게 난거임. 다시 삭제해야됨.
            else
            {
                GameObject.Destroy(createdPlayerUnit);
            }
        }

        // 생성된 모든 PlayerUnit GameObject를 삭제 및 관련 데이터 삭제.
        public void ClearGameObjectAndDatas()
        {
            this.GeneratedPlayerUnitDataGroupHandler.ClearPlayerUnitManagerDataGroup();
            this.playerSpawnQueue.Clear();

            // enemyGameObject 삭제.
            foreach (Transform child in PlayerUnitObjectParent)
            {
                GameObject.Destroy(child.gameObject);
            }
        }
    }
}