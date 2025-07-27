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
            // Stage ID�� ���� �ش� Stage Enemy ���� ���� �Ҵ�.
            this.EventObserverLinker.RegisterSubscriberListener<EnemyUnitSpawnEvent_StageSetting>();
            // TurnID ��� Enemy ����.
            this.EventObserverLinker.RegisterSubscriberListener<EnemyUnitSpawnEvent_TurnSetting>();
            // TriggerID ��� Enemy ����.
            this.EventObserverLinker.RegisterSubscriberListener<EnemyUnitSpawnEvent_Trigger>();

            // �����ϴ� Enemy ��� ����.
            this.EventObserverLinker.RegisterSubscriberListener<EnemySpawnEvent_Continue>();

            // �׽�Ʈ�� ����, (UnitID, SpawnPosition) �� ����.
            this.EventObserverLinker.RegisterSubscriberListener<EnemyUnitSpawnEvent_Test>();

            // Enemy Clear �۾�.
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
                // Stage ID�� ���� �ش� Stage Enemy ���� ���� �Ҵ�.
                case EnemyUnitSpawnEvent_StageSetting:
                    var enemyUnitSpawnEvent_StageSetting = (EnemyUnitSpawnEvent_StageSetting)eventData;
                    this.EnemySpawnController.InitialSetting(enemyUnitSpawnEvent_StageSetting.StageID);
                    break;
                // TurnID ��� Enemy ����.
                case EnemyUnitSpawnEvent_TurnSetting:
                    var enemyUnitSpawnEvent_TurnSetting = (EnemyUnitSpawnEvent_TurnSetting)eventData;
                    this.EnemySpawnController.AllocateEnemyUnitSpawnData_Turn(enemyUnitSpawnEvent_TurnSetting.TurnID);
                    this.EnemySpawnController.GenerateEnemyUnit();
                    break;
                // Trigger ��� Enemy ����.
                case EnemyUnitSpawnEvent_Trigger:
                    var enemyUnitSpawnEvent_Trigger = (EnemyUnitSpawnEvent_Trigger)eventData;
                    this.EnemySpawnController.AllocateEnemyUnitSpawnData_Trigger(enemyUnitSpawnEvent_Trigger.TriggerID);
                    this.EnemySpawnController.GenerateEnemyUnit();
                    break;
                case EnemySpawnEvent_Continue:
                    this.EnemySpawnController.GenerateEnemyUnit();
                    break;
                // Enemy Clear �۾�.
                case EnemyUnitClearEvent:
                    this.EnemySpawnController.ClearGameObjectAndDatas();
                    break;
                // Test Enemy ���� �۾�
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