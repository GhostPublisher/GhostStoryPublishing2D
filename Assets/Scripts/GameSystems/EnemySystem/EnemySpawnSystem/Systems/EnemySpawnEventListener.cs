using UnityEngine;

using Foundations.Architecture.EventObserver;

namespace GameSystems.EnemySystem.EnemySpawnSystem
{
    public class EnemySpawnEventListener : MonoBehaviour, IEventObserverListener
    {
        private IEventObserverLinker EventObserverLinker;

        [SerializeField] private EnemySpawnController EnemySpawnController;

        private void Awake()
        {
            this.EventObserverLinker = new EventObserverLinker(this);
        }

        private void OnEnable()
        {
            // Stage ID에 대한 해당 Stage Enemy 생성 정보 할당.
            this.EventObserverLinker.RegisterSubscriberListener<EnemySpawnInitialSettingEvent>();

            // TurnID 기반 Enemy 생성.
            this.EventObserverLinker.RegisterSubscriberListener<EnemySpawnEvent_Turn>();

            // TriggerID 기반 Enemy 생성.
            this.EventObserverLinker.RegisterSubscriberListener<EnemySpawnEvent_Trigger>();

            // 생성하던 Enemy 계속 생성.
            this.EventObserverLinker.RegisterSubscriberListener<EnemySpawnEvent_Continue>();

            // 테스트를 위해, (UnitID, SpawnPosition) 에 생성.
            this.EventObserverLinker.RegisterSubscriberListener<EnemySpawnEvent_Test>();

            // Enemy Clear 작업.
            this.EventObserverLinker.RegisterSubscriberListener<EnemyClearEvent>();
        }
        private void OnDisable()
        {
            this.EventObserverLinker.Dispose();
        }

        public void UpdateData(IEventData eventData)
        {
            switch (eventData)
            {
                // Stage ID에 대한 해당 Stage Enemy 생성 정보 할당.
                case EnemySpawnInitialSettingEvent:
                    var data01 = (EnemySpawnInitialSettingEvent)eventData;
                    this.EnemySpawnController.InitialSetting(data01.StageID);
                    break;
                // TurnID 기반 Enemy 생성.
                case EnemySpawnEvent_Turn:
                    var data02 = (EnemySpawnEvent_Turn)eventData;
                    this.EnemySpawnController.AllocateTriggerEnemySpawnData_Turn(data02.TurnID);
                    this.EnemySpawnController.GenerateEnemyUnit();
                    break;
                // Trigger 기반 Enemy 생성.
                case EnemySpawnEvent_Trigger:
                    var data03 = (EnemySpawnEvent_Trigger)eventData;
                    this.EnemySpawnController.AllocateTriggerEnemySpawnData_Trigger(data03.TriggerID);
                    this.EnemySpawnController.GenerateEnemyUnit();
                    break;
                case EnemySpawnEvent_Continue:
                    this.EnemySpawnController.GenerateEnemyUnit();
                    break;
                case EnemySpawnEvent_Test:
                    EnemySpawnEvent_Test enemySpawnEvent_Test = (EnemySpawnEvent_Test)eventData;
                    this.EnemySpawnController.GenerateEnemyUnit(enemySpawnEvent_Test.UnitID, enemySpawnEvent_Test.SpawnPosition);
                    break;
                // Enemy Clear 작업.
                case EnemyClearEvent:
                    this.EnemySpawnController.ClearGameObjectAndDatas();
                    break;
                default:
                    break;
            }
        }
    }
}