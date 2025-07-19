using UnityEngine;

using Foundations.Architecture.EventObserver;

namespace GameSystems.TilemapSystem.FogTilemap
{
    public class FogTilemapSystemEventListener : MonoBehaviour, IEventObserverListener
    {
        private IEventObserverLinker EventObserverLinker;

        [SerializeField] private FogTilemapController FogTilemapController;

        private void Awake()
        {
            this.EventObserverLinker = new EventObserverLinker(this);
        }

        private void OnEnable()
        {
            // 초기 설정.
            this.EventObserverLinker.RegisterSubscriberListener<FogTilemapInitialSettingEvent>();

            // Player Unit 시야 값에 의한 Fog Visible 갱신
            this.EventObserverLinker.RegisterSubscriberListener<UpdatePlayerVisibleData_ForFogVisibility>();
            this.EventObserverLinker.RegisterSubscriberListener<RemovePlayerVisibleData_ForFogVisibility>();

            // Scanner 시야 값에 의한 Fog Visible 갱신
            this.EventObserverLinker.RegisterSubscriberListener<UpdateScanerVisibleData_ForFogVisibility>();
            this.EventObserverLinker.RegisterSubscriberListener<RemoveScanerVisibleData_ForFogVisibility>();

            // Test를 위한 Fog Visible 갱신
            this.EventObserverLinker.RegisterSubscriberListener<InitialSetFogTilemapEvent_Raw>();
            this.EventObserverLinker.RegisterSubscriberListener<UpdateScanerRawVisibleData_ForFogVisibility>();
            this.EventObserverLinker.RegisterSubscriberListener<ShowAllFogTilemap>();
            this.EventObserverLinker.RegisterSubscriberListener<HideAllFogTilemap>();
            this.EventObserverLinker.RegisterSubscriberListener<ClearFogTilemapEvent>();
        }

        private void OnDisable()
        {
            this.EventObserverLinker.Dispose();
        }

        public void InitialSetting(int stageID)
        {
            this.EventObserverLinker = new EventObserverLinker(this);

            this.FogTilemapController.InitialSetting(stageID);
        }

        public void UpdateData(IEventData eventData)
        {
            switch (eventData)
            {
                // 초기 설정.
                case FogTilemapInitialSettingEvent:
                    var data01 = (FogTilemapInitialSettingEvent)eventData;
                    this.FogTilemapController.InitialSetting(data01.StageID);
                    break;

                // Player Unit 시야 값에 의한 Fog Visible 갱신
                case UpdatePlayerVisibleData_ForFogVisibility:
                    var data02 = (UpdatePlayerVisibleData_ForFogVisibility)eventData;
                    this.FogTilemapController.UpdatePlayerVisibleData_ForFogVisibility(data02.PlayerUniqueID, data02.VisibleRange);
                    this.FogTilemapController.UpdateFogVisibility();
                    break;
                case RemovePlayerVisibleData_ForFogVisibility:
                    var data03 = (RemovePlayerVisibleData_ForFogVisibility)eventData;
                    this.FogTilemapController.RemovePlayerVisibleData_ForFogVisibility(data03.PlayerUniqueID);
                    this.FogTilemapController.UpdateFogVisibility();
                    break;

                // Scanner 시야 값에 의한 Fog Visible 갱신
                case UpdateScanerVisibleData_ForFogVisibility:
                    var data04 = (UpdateScanerVisibleData_ForFogVisibility)eventData;
                    this.FogTilemapController.UpdateScannerVisibleData_ForFogVisibility(data04.ScannerUniqueID, data04.VisibleRange);
                    this.FogTilemapController.UpdateFogVisibility();
                    break;
                case RemoveScanerVisibleData_ForFogVisibility:
                    var data05 = (RemoveScanerVisibleData_ForFogVisibility)eventData;
                    this.FogTilemapController.RemoveScannerVisibleData_ForFogVisibility(data05.ScannerUniqueID);
                    this.FogTilemapController.UpdateFogVisibility();
                    break;

                // Test를 위한 Fog Visible 갱신
                case InitialSetFogTilemapEvent_Raw:
                    var data06 = (InitialSetFogTilemapEvent_Raw)eventData;
                    this.FogTilemapController.InitialSetting(data06.Width, data06.Height);
                    break;
                case UpdateScanerRawVisibleData_ForFogVisibility:
                    var data07 = (UpdateScanerRawVisibleData_ForFogVisibility)eventData;
                    this.FogTilemapController.UpdateScannerRawVisibleData_ForFogVisibility(data07.ScannerUniqueID, data07.TargetPosition, data07.VisibleSize, data07.VisibleOvercomeWeight);
                    this.FogTilemapController.UpdateFogVisibility();
                    break;
                case ShowAllFogTilemap:
                    this.FogTilemapController.ShowAllFog();
                    break;
                case HideAllFogTilemap:
                    this.FogTilemapController.HideAllFog();
                    break;
                case ClearFogTilemapEvent:
                    this.FogTilemapController.ClearFogData();
                    break;
                default:
                    break;
            }
        }
    }
}
