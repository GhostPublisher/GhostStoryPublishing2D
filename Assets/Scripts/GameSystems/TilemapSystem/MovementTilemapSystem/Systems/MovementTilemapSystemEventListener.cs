using UnityEngine;

using Foundations.Architecture.EventObserver;

namespace GameSystems.TilemapSystem.MovementTilemap
{
    public class MovementTilemapSystemEventListener : MonoBehaviour, IEventObserverListener
    {
        private IEventObserverLinker EventObserverLinker;

        [SerializeField] private MovementTilemapSystem MovementTilemapSystem;

        private void Awake()
        {
            this.EventObserverLinker = new EventObserverLinker(this);
        }

        private void OnEnable()
        {
            this.EventObserverLinker.RegisterSubscriberListener<InitialSetMovementTilemapEvent_Raw>();

            this.EventObserverLinker.RegisterSubscriberListener<ActivateMovementTilemapEvent>();
            this.EventObserverLinker.RegisterSubscriberListener<DisActivateMovementTilemapEvent>();
        }

        private void OnDisable()
        {
            this.EventObserverLinker.Dispose();
        }

        public void UpdateData(IEventData eventData)
        {
            switch (eventData)
            {
                case InitialSetMovementTilemapEvent_Raw:
                    var data01 = (InitialSetMovementTilemapEvent_Raw)eventData;
                    this.MovementTilemapSystem.InitialSetting(data01.Width, data01.Height);
                    break;
                case ActivateMovementTilemapEvent:
                    var data02 = (ActivateMovementTilemapEvent)eventData;
                    this.MovementTilemapSystem.ActivateMovementTilemap(data02.PlayerUniqueID, data02.CurrentPosition, data02.MoveableRange);
                    break;
                case DisActivateMovementTilemapEvent:
                    var data03 = (ActivateMovementTilemapEvent)eventData;
                    this.MovementTilemapSystem.DisActivateMovementTilemap();
                    break;
                default:
                    break;
            }
        }
    }
}