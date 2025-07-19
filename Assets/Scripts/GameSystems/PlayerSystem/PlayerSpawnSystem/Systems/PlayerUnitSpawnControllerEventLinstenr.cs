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
            this.EventObserverLinker.RegisterSubscriberListener<PlayerUnitSpawnEvent_Clear>();

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
                case PlayerUnitSpawnEvent_StageSetting:
                    var data01 = (PlayerUnitSpawnEvent_StageSetting)eventData;
                    this.PlayerUnitSpawnController.AllocateStagePlayerSpawnData(data01.StageID);
                    break;
                case PlayerUnitSpawnEvent_Trigger:
                    var data02 = (PlayerUnitSpawnEvent_Trigger)eventData;
                    this.PlayerUnitSpawnController.AllocatePlayerSpawnData_Trigger(data02.TriggerID);
                    break;
                case PlayerUnitSpawnEvent_Continue:
                    this.PlayerUnitSpawnController.GeneratePlayerUnit();
                    break;
                case PlayerUnitSpawnEvent_Clear:
                    this.PlayerUnitSpawnController.ClearGameObjectAndDatas();
                    break;
                case PlayerUnitSpawnEvent_Test:
                    var data03 = (PlayerUnitSpawnEvent_Test)eventData;
                    this.PlayerUnitSpawnController.GeneratePlayerUnit(data03.UnitID, data03.SpawnPosition);
                    break;
                default:
                    break;
            }
        }
    }
}