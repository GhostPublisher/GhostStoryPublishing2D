using UnityEngine;

using Foundations.Architecture.EventObserver;

namespace GameSystems.PlayerSystem.PlayerSpawnSystem
{
    public class PlayerUnitSpawnControllerEventLinstenr : MonoBehaviour, IEventObserverListener
    {
        private IEventObserverLinker EventObserverLinker;

        [SerializeField] private PlayerUnitSpawnController PlayerUnitSpawnController;

        private void Awake()
        {
            this.EventObserverLinker = new EventObserverLinker(this);
        }

        private void OnEnable()
        {
            this.EventObserverLinker.RegisterSubscriberListener<PlayerUnitSpawnEvent_StageSetting>();
            this.EventObserverLinker.RegisterSubscriberListener<PlayerUnitSpawnEvent_Trigger>();

            this.EventObserverLinker.RegisterSubscriberListener<PlayerUnitSpawnEvent_Continue>();

            this.EventObserverLinker.RegisterSubscriberListener<PlayerUnitClearEvent>();

            this.EventObserverLinker.RegisterSubscriberListener<PlayerUnitSpawnEvent_Test>();
        }

        private void OnDisable()
        {
            this.EventObserverLinker.Dispose();
        }


        public void UpdateData(IEventData eventData)
        {
            switch (eventData)
            {
                // Stage Setting 흐름에서 Player Unit 생성.
                case PlayerUnitSpawnEvent_StageSetting:
                    var playerUnitSpawnEvent_StageSetting = (PlayerUnitSpawnEvent_StageSetting)eventData;
                    this.PlayerUnitSpawnController.InitialSetting(playerUnitSpawnEvent_StageSetting.StageID);
                    break;
                // Trigger 기반 Player Unit 생성.
                case PlayerUnitSpawnEvent_Trigger:
                    var playerUnitSpawnEvent_Trigger = (PlayerUnitSpawnEvent_Trigger)eventData;
                    this.PlayerUnitSpawnController.AllocatePlayerUnitSpawnData_Trigger(playerUnitSpawnEvent_Trigger.TriggerID);
                    this.PlayerUnitSpawnController.GeneratePlayerUnit();
                    break;
                case PlayerUnitSpawnEvent_Continue:
                    this.PlayerUnitSpawnController.GeneratePlayerUnit();
                    break;
                case PlayerUnitClearEvent:
                    this.PlayerUnitSpawnController.ClearGameObjectAndDatas();
                    break;
                case PlayerUnitSpawnEvent_Test:
                    var testData = (PlayerUnitSpawnEvent_Test)eventData;
                    this.PlayerUnitSpawnController.GeneratePlayerUnit(testData.UnitID, testData.SpawnPosition);
                    break;
                default:
                    break;
            }
        }
    }
}