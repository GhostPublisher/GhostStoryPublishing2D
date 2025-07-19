
using UnityEngine;

using Foundations.Architecture.EventObserver;

namespace GameSystems.TerrainSystem
{
    public class GroundTilemapControllerEventListener : MonoBehaviour, IEventObserverListener
    {
        private IEventObserverLinker EventObserverLinker;

        [SerializeField] private GroundTilemapController GroundTilemapController;

        private void Awake()
        {
            this.EventObserverLinker = new EventObserverLinker(this);
        }

        private void OnEnable()
        {
            this.EventObserverLinker.RegisterSubscriberListener<GenerateGroundTilemapEvent_Test>();
            this.EventObserverLinker.RegisterSubscriberListener<ClearGroundTilemapEvent>();
        }

        private void OnDisable()
        {
            this.EventObserverLinker.Dispose();
        }

        public void UpdateData(IEventData eventData)
        {
            switch (eventData)
            {
                case GenerateGroundTilemapEvent_Test:
                    var data01 = (GenerateGroundTilemapEvent_Test)eventData;
                    this.GroundTilemapController.GenerateGroundTilemap(data01.GroundTilemapGameObject);
                    break;
                case ClearGroundTilemapEvent:
                    this.GroundTilemapController.ClearGroundTilemap();
                    break;
                default:
                    break;
            }
        }
    }
}