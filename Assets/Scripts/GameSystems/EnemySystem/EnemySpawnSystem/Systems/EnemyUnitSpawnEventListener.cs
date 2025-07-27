using UnityEngine;

using Foundations.Architecture.EventObserver;

namespace GameSystems.EnemySystem.EnemySpawnSystem
{
    public class EnemyUnitSpawnEventListener : MonoBehaviour, IEventObserverListener
    {
        private IEventObserverLinker EventObserverLinker;

        [SerializeField] private EnemyUnitSpawnController EnemySpawnController;

        private void Awake()
        {
            this.EventObserverLinker = new EventObserverLinker(this);
        }

        private void OnEnable()
        {
            // Stage ID에 대한 해당 Stage Enemy 생성 정보 할당.
            this.EventObserverLinker.RegisterSubscriberListener<EnemyUnitSpawnEvent_StageSetting>();
            // TurnID 기반 Enemy 생성.
            this.EventObserverLinker.RegisterSubscriberListener<EnemyUnitSpawnEvent_TurnSetting>();
            // TriggerID 기반 Enemy 생성.
            this.EventObserverLinker.RegisterSubscriberListener<EnemyUnitSpawnEvent_Trigger>();

            // 생성하던 Enemy 계속 생성.
            this.EventObserverLinker.RegisterSubscriberListener<EnemySpawnEvent_Continue>();

            // 테스트를 위해, (UnitID, SpawnPosition) 에 생성.
            this.EventObserverLinker.RegisterSubscriberListener<EnemyUnitSpawnEvent_Test>();

            // Enemy Clear 작업.
            this.EventObserverLinker.RegisterSubscriberListener<EnemyUnitClearEvent>();
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
                case EnemyUnitSpawnEvent_StageSetting:
                    var enemyUnitSpawnEvent_StageSetting = (EnemyUnitSpawnEvent_StageSetting)eventData;
                    this.EnemySpawnController.InitialSetting(enemyUnitSpawnEvent_StageSetting.StageID);
                    break;
                // TurnID 기반 Enemy 생성.
                case EnemyUnitSpawnEvent_TurnSetting:
                    var enemyUnitSpawnEvent_TurnSetting = (EnemyUnitSpawnEvent_TurnSetting)eventData;
                    this.EnemySpawnController.AllocateEnemyUnitSpawnData_Turn(enemyUnitSpawnEvent_TurnSetting.TurnID);
                    this.EnemySpawnController.GenerateEnemyUnit();
                    break;
                // Trigger 기반 Enemy 생성.
                case EnemyUnitSpawnEvent_Trigger:
                    var enemyUnitSpawnEvent_Trigger = (EnemyUnitSpawnEvent_Trigger)eventData;
                    this.EnemySpawnController.AllocateEnemyUnitSpawnData_Trigger(enemyUnitSpawnEvent_Trigger.TriggerID);
                    this.EnemySpawnController.GenerateEnemyUnit();
                    break;
                case EnemySpawnEvent_Continue:
                    this.EnemySpawnController.GenerateEnemyUnit();
                    break;
                // Enemy Clear 작업.
                case EnemyUnitClearEvent:
                    this.EnemySpawnController.ClearGameObjectAndDatas();
                    break;
                // Test Enemy 생성 작업
                case EnemyUnitSpawnEvent_Test:
                    EnemyUnitSpawnEvent_Test testData = (EnemyUnitSpawnEvent_Test)eventData;
                    this.EnemySpawnController.GenerateEnemyUnit(testData.UnitID, testData.SpawnPosition);
                    break;
                default:
                    break;
            }
        }
    }
}