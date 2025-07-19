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
            // Stage ID�� ���� �ش� Stage Enemy ���� ���� �Ҵ�.
            this.EventObserverLinker.RegisterSubscriberListener<EnemySpawnInitialSettingEvent>();

            // '���ο� Turn ���'���� �����Ǵ� Enemy ���.
            this.EventObserverLinker.RegisterSubscriberListener<EnemySpawnSettingEvent_Turn>();
            // TurnID ��� Enemy ����.
            this.EventObserverLinker.RegisterSubscriberListener<EnemySpawnContinueEvent_Turn>();

            // 'Ư�� ����� Trigger�� �ϴ� ���'���� �����Ǵ� Enemy.
            this.EventObserverLinker.RegisterSubscriberListener<EnemySpawnSettingEvent_Trigger>();
            // TriggerID ��� Enemy ����.
            this.EventObserverLinker.RegisterSubscriberListener<EnemySpawnContinueEvent_Trigger>();

            // �����ϴ� Enemy ��� ����.
            this.EventObserverLinker.RegisterSubscriberListener<EnemySpawnEvent_Continue>();

            // �׽�Ʈ�� ����, (UnitID, SpawnPosition) �� ����.
            this.EventObserverLinker.RegisterSubscriberListener<EnemySpawnEvent_Test>();

            // Enemy Clear �۾�.
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
                // Stage ID�� ���� �ش� Stage Enemy ���� ���� �Ҵ�.
                case EnemySpawnInitialSettingEvent:
                    var data01 = (EnemySpawnInitialSettingEvent)eventData;
                    this.EnemySpawnController.AllocateStageEnemySpawnData(data01.StageID);
                    break;
                // Turn�� ���࿡ ���� Enemy ����.
                case EnemySpawnSettingEvent_Turn:
                    var data02 = (EnemySpawnSettingEvent_Turn)eventData;
                    this.EnemySpawnController.AllocateTriggerEnemySpawnData_Turn(data02.TurnID);
                    break;
                // TurnID ��� Enemy ����.
                case EnemySpawnContinueEvent_Turn:
                    this.EnemySpawnController.GenerateEnemy_Turn();
                    break;
                // ���� �ൿ�� ���� Enemy ����.
                case EnemySpawnSettingEvent_Trigger:
                    var data03 = (EnemySpawnSettingEvent_Trigger)eventData;
                    this.EnemySpawnController.AllocateTriggerEnemySpawnData_Trigger(data03.TriggerID);
                    break;
                // TriggerID ��� Enemy ����.
                case EnemySpawnContinueEvent_Trigger:
                    this.EnemySpawnController.GenerateEnemy_Trigger();
                    break;
                case EnemySpawnEvent_Continue:
                    break;
                case EnemySpawnEvent_Test:
                    EnemySpawnEvent_Test enemySpawnEvent_Test = (EnemySpawnEvent_Test)eventData;
                    this.EnemySpawnController.GenerateEnemyUnit(enemySpawnEvent_Test.UnitID, enemySpawnEvent_Test.SpawnPosition);
                    break;
                // Enemy Clear �۾�.
                case EnemyClearEvent:
                    this.EnemySpawnController.ClearGameObjectAndDatas();
                    break;
                default:
                    break;
            }
        }
    }
}