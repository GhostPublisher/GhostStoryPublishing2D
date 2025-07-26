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
                case InitialSetMovementTilemapEvent:
                    var data01 = (InitialSetMovementTilemapEvent)eventData;
                    this.MovementTilemapSystem.InitialSetting(data01.StageID);
                    break;
                case InitialSetMovementTilemapEvent_Raw:
                    var data02 = (InitialSetMovementTilemapEvent_Raw)eventData;
                    this.MovementTilemapSystem.InitialSetting(data02.Width, data02.Height);
                    break;
                case ActivateMovementTilemapEvent:
                    var data03 = (ActivateMovementTilemapEvent)eventData;
                    this.MovementTilemapSystem.ActivateMovementTilemap(data03.PlayerUniqueID, data03.CurrentPosition, data03.MoveableRange);
                    break;
                case DisActivateMovementTilemapEvent:
                    var data04 = (ActivateMovementTilemapEvent)eventData;
                    this.MovementTilemapSystem.DisActivateMovementTilemap();
                    break;
                default:
                    break;
            }
        }
    }
}