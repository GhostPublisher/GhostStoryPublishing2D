using System.Collections.Generic;

using UnityEngine;

using Foundations.Architecture.ReferencesHandler;

using GameSystems.PlayerSystem.PlayerUnitSystem;

namespace GameSystems.PlayerSystem.PlayerSpawnSystem
{
    public class PlayerUnitSpawnController : MonoBehaviour
    {
        private PlayerSpawnDataDBHandler PlayerSpawnDataDBHandler;
        private PlayerUnitDataDBHandler PlayerUnitDataDBHandler;

        private PlayerUnitManagerDataDBHandler GeneratedPlayerUnitDataGroupHandler;

        [SerializeField] private Transform PlayerUnitObjectParent;

        private Queue<PlayerSpawnData> playerSpawnDatas = new();

        private void Awake()
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;

            this.PlayerSpawnDataDBHandler = HandlerManager.GetStaticDataHandler<PlayerSpawnDataDBHandler>();
            this.PlayerUnitDataDBHandler = HandlerManager.GetStaticDataHandler<PlayerUnitDataDBHandler>();

            this.GeneratedPlayerUnitDataGroupHandler = HandlerManager.GetDynamicDataHandler<PlayerUnitManagerDataDBHandler>();
        }

        // 특정 StageID에 의해 생성되는 PlayerSpawnData를 할당 ( 최초 한번 실행 )
        public void AllocateStagePlayerSpawnData(int stageID)
        {
            this.playerSpawnDatas.Clear();
            if (this.PlayerSpawnDataDBHandler.TryGetStagePlayerSpawnData(stageID, out var datas))
            {
                foreach (var data in datas)
                {
                    this.playerSpawnDatas.Enqueue(data);
                }
            }
        }
        // 특정 TriggerID에 의해 생성되는 PlayerSpawnData를 할당. ( 조건하에 N번 호출 )
        public void AllocatePlayerSpawnData_Trigger(int triggerID)
        {
            this.playerSpawnDatas.Clear();
            if (this.PlayerSpawnDataDBHandler.TryGetTriggerPlayerSpawnData(triggerID, out var datas))
            {
                foreach (var data in datas)
                {
                    this.playerSpawnDatas.Enqueue(data);
                }
            }
        }

        // 할당되어 있는 PlayerSpawnData을 순서대로 호출.
        public void GeneratePlayerUnit()
        {
            if (this.playerSpawnDatas.Count == 0) return;

            // Prefab 가져오기.
            PlayerSpawnData playerSpawnData = this.playerSpawnDatas.Dequeue();
            this.GeneratePlayerUnit(playerSpawnData.UnitID, new Vector2Int(playerSpawnData.PlayerSpawnPositionX, playerSpawnData.PlayerSpawnPositionY));
        }
        // 매개변수로 받은 ID와 Position에 대응되는 PlayerUnit을 배치.
        public void GeneratePlayerUnit(int unitID, Vector2Int spawnPosition)
        {
            if (!this.PlayerUnitDataDBHandler.TryGetPlayerUnitResourceData(unitID, out var prefabData)) return;

            // 객체 생성
            GameObject createdPlayerUnit = Object.Instantiate(prefabData.UnitPrefab, this.PlayerUnitObjectParent);
            // 객체 위치 지정.
            createdPlayerUnit.transform.position = this.ConvertGridToWorld(spawnPosition);

            // Player Unit 초기 할당.
            if (createdPlayerUnit.GetComponent<IPlayerUnitManager>().TryInitialSetting(out var newPlayerUnitManagerData))
            {
                // Player 관리 데이터에 할당.
                this.GeneratedPlayerUnitDataGroupHandler.AddPlayerUnitManagerData(newPlayerUnitManagerData);
            }
            // 이거 오류 크게 난거임. 다시 삭제해야됨.
            else
            {
                Debug.Log($"Player 생성 오류 남");

                GameObject.Destroy(createdPlayerUnit);
            }
        }

        // 생성된 모든 PlayerUnit GameObject를 삭제 및 관련 데이터 삭제.
        public void ClearGameObjectAndDatas()
        {
            this.GeneratedPlayerUnitDataGroupHandler.ClearPlayerUnitManagerDataGroup();
            this.playerSpawnDatas.Clear();

            // enemyGameObject 삭제.
            foreach (Transform child in PlayerUnitObjectParent)
            {
                GameObject.Destroy(child.gameObject);
            }
        }
        // Convert 작업.
        private Vector3 ConvertGridToWorld(Vector2Int girdPosition)
        {
            return new Vector3(girdPosition.x * 1f, girdPosition.y * 1f, 0);
        }
    }
}