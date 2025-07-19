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

            // '새로운 Turn 사건'으로 생성되는 Enemy 사건.
            this.EventObserverLinker.RegisterSubscriberListener<EnemySpawnSettingEvent_Turn>();
            // TurnID 기반 Enemy 생성.
            this.EventObserverLinker.RegisterSubscriberListener<EnemySpawnContinueEvent_Turn>();

            // '특정 사건을 Trigger로 하는 사건'으로 생성되는 Enemy.
            this.EventObserverLinker.RegisterSubscriberListener<EnemySpawnSettingEvent_Trigger>();
            // TriggerID 기반 Enemy 생성.
            this.EventObserverLinker.RegisterSubscriberListener<EnemySpawnContinueEvent_Trigger>();

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
                    this.EnemySpawnController.AllocateStageEnemySpawnData(data01.StageID);
                    break;
                // Turn의 진행에 따른 Enemy 생성.
                case EnemySpawnSettingEvent_Turn:
                    var data02 = (EnemySpawnSettingEvent_Turn)eventData;
                    this.EnemySpawnController.AllocateTriggerEnemySpawnData_Turn(data02.TurnID);
                    break;
                // TurnID 기반 Enemy 생성.
                case EnemySpawnContinueEvent_Turn:
                    this.EnemySpawnController.GenerateEnemy_Turn();
                    break;
                // 유저 행동에 의한 Enemy 생성.
                case EnemySpawnSettingEvent_Trigger:
                    var data03 = (EnemySpawnSettingEvent_Trigger)eventData;
                    this.EnemySpawnController.AllocateTriggerEnemySpawnData_Trigger(data03.TriggerID);
                    break;
                // TriggerID 기반 Enemy 생성.
                case EnemySpawnContinueEvent_Trigger:
                    this.EnemySpawnController.GenerateEnemy_Trigger();
                    break;
                case EnemySpawnEvent_Continue:
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