using UnityEngine;

using Foundations.Architecture.EventObserver;

namespace GameSystems.BattleSceneSystem
{
    public class BattleSceneFlowControllerEventListener : MonoBehaviour, IEventObserverListener
    {
        private IEventObserverLinker EventObserverLinker;

        [SerializeField] private BattleSceneFlowController BattleSceneFlowController;

        private void Awake()
        {
            this.EventObserverLinker = new EventObserverLinker(this);
        }

        private void OnEnable()
        {
            this.EventObserverLinker.RegisterSubscriberListener<InitialSetBattleSceneFlowControllerEvent>();
        }

        private void OnDisable()
        {
            this.EventObserverLinker.Dispose();
        }

        public void UpdateData(IEventData eventData)
        {
            switch (eventData)
            {
                case InitialSetBattleSceneFlowControllerEvent:
                    var data01 = (InitialSetBattleSceneFlowControllerEvent)eventData;
                    this.BattleSceneFlowController.InitialSetting(data01.StageID);
                    break;
                default:
                    break;
            }
        }
    }
}