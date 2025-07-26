
using UnityEngine;

using Foundations.Architecture.EventObserver;

namespace GameSystems.StageVisualSystem
{
    public class StageVisualResourcesControllerEventListener : MonoBehaviour, IEventObserverListener
    {
        private IEventObserverLinker EventObserverLinker;

        [SerializeField] private StageVisualResourcesController StageVisualResourcesController;

        private void Awake()
        {
            this.EventObserverLinker = new EventObserverLinker(this);
        }

        private void OnEnable()
        {
            this.EventObserverLinker.RegisterSubscriberListener<InitialSetStageVisualResourcesData>();
            this.EventObserverLinker.RegisterSubscriberListener<InitialSetStageVisualResourcesData_EventTest>();
            this.EventObserverLinker.RegisterSubscriberListener<ClearStageVisualResourcesDataEvent>();
        }

        private void OnDisable()
        {
            this.EventObserverLinker.Dispose();
        }

        public void UpdateData(IEventData eventData)
        {
            switch (eventData)
            {
                case InitialSetStageVisualResourcesData:
                    var data01 = (InitialSetStageVisualResourcesData)eventData;
                    this.StageVisualResourcesController.InitialSetStageVisualResources(data01.StageID);
                    break;
                case InitialSetStageVisualResourcesData_EventTest:
                    var data02 = (InitialSetStageVisualResourcesData_EventTest)eventData;
                    this.StageVisualResourcesController.GenerateVisualResourcesTilemap(data02.StageVisualResourcesData);
                    break;
                case ClearStageVisualResourcesDataEvent:
                    this.StageVisualResourcesController.ClearFloorTilemap();
                    break;
                default:
                    break;
            }
        }
    }
}