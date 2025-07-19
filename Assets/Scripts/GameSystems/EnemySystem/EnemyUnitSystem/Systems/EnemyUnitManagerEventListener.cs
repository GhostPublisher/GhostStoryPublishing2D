using UnityEngine;
using Foundations.Architecture.EventObserver;

namespace GameSystems.EnemySystem.EnemyUnitSystem
{
    public class EnemyUnitManagerEventListener : MonoBehaviour, IEventObserverListener
    {
        private IEventObserverLinker EventObserverLinker;

        [SerializeField] EnemyUnitManager EnemyUnitManager;

        private void Awake()
        {
            this.EventObserverLinker = new EventObserverLinker(this);
        }

        private void OnEnable()
        {
            this.EventObserverLinker.RegisterSubscriberListener<OperateEnemyAI_Raw>();
            this.EventObserverLinker.RegisterSubscriberListener<OperateNewTurnSetting>();
        }

        private void OnDisable()
        {
            this.EventObserverLinker.Dispose();
        }

        public void UpdateData(IEventData eventData)
        {
            switch (eventData)
            {
                case OperateEnemyAI_Raw:
                    var data01 = (OperateEnemyAI_Raw)eventData;
                    this.EnemyUnitManager.OperateEnemyAI(data01.UniqueID);
                    break;
                case OperateNewTurnSetting:
                    this.EnemyUnitManager.OperateNewTurnSetting();
                    break;
                default:
                    break;
            }
        }
    }
}