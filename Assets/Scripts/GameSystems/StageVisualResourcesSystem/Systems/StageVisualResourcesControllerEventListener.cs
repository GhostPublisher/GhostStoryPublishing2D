
using UnityEngine;

using Foundations.Architecture.EventObserver;

namespace GameSystems.TerrainSystem
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
                case InitialSetStageVisualResourcesData_EventTest:
                    var data01 = (InitialSetStageVisualResourcesData_EventTest)eventData;
                    this.StageVisualResourcesController.GenerateFloorTilemap(data01.GroundTilemapGameObject);
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