
using UnityEngine;

using Foundations.Architecture.EventObserver;

using GameSystems.TilemapSystem.FogTilemap;

namespace Example00
{
    public class FogTilemapTest : MonoBehaviour
    {
        private IEventObserverNotifier EventObserverNotifier;

        [SerializeField] private InitialSetFogTilemapEvent_Raw FogTilemapRawInitialSettingEvent;
        [SerializeField] private UpdateScanerRawVisibleData_ForFogVisibility UpdateScanerRawVisibleData_ForFogVisibility;
        [SerializeField] private ClearFogTilemapEvent ClearFogTilemapEvent;

        private void Awake()
        {
            this.EventObserverNotifier = new EventObserverNotifier();
        }

        // Notify 요청.
        public void Notify_FogTilemapRawInitialSettingEvent()
        {
            this.EventObserverNotifier.NotifyEvent(FogTilemapRawInitialSettingEvent);
        }


        // Notify 요청.
        public void Notify_UpdateScanerRawVisibleData_ForFogVisibility()
        {
            this.EventObserverNotifier.NotifyEvent(UpdateScanerRawVisibleData_ForFogVisibility);
        }

        // Notify 요청.
        public void Notify_ClearFogTilemapEvent()
        {
            this.EventObserverNotifier.NotifyEvent(ClearFogTilemapEvent);
        }
    }
}