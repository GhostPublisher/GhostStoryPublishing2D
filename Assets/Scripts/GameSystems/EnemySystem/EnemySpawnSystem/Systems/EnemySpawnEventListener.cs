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

            // TurnID ��� Enemy ����.
            this.EventObserverLinker.RegisterSubscriberListener<EnemySpawnEvent_Turn>();

            // TriggerID ��� Enemy ����.
            this.EventObserverLinker.RegisterSubscriberListener<EnemySpawnEvent_Trigger>();

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
                    this.EnemySpawnController.InitialSetting(data01.StageID);
                    break;
                // TurnID ��� Enemy ����.
                case EnemySpawnEvent_Turn:
                    var data02 = (EnemySpawnEvent_Turn)eventData;
                    this.EnemySpawnController.AllocateTriggerEnemySpawnData_Turn(data02.TurnID);
                    this.EnemySpawnController.GenerateEnemyUnit();
                    break;
                // Trigger ��� Enemy ����.
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