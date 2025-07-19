using UnityEngine;

using Foundations.Architecture.EventObserver;

namespace GameSystems.EnemySystem.EnemyVisibilitySystem
{
    // Enemy의 visible, InVisible의 갱신은 언제 들어오는가?
    // 1. Player의 이동.
    // 1-1. Player의 시야 범위 갱신 시.
    // 1-2. Player의 OvercomeWeight 변경 시.

    // 2. Enemy의 이동.
    // 2-1. Enemy의 BlockWeight 변경 시.

    // 3. Scaner에 의한 특정 지점 활성화.

    public class EnemyVisibilityEventListener : MonoBehaviour, IEventObserverListener
    {
        private IEventObserverLinker EventObserverLinker;

        [SerializeField] private EnemyVisibilityController EnemyVisibilityController;

        private void Awake()
        {
            this.EventObserverLinker = new EventObserverLinker(this);
        }

        private void OnEnable()
        {
            // 초기 설정
            this.EventObserverLinker.RegisterSubscriberListener<EnemyVisibilitySystemInitialSetting>();

            // Player Unit 시야 값에 의한 Enemy Visibility 갱신
            this.EventObserverLinker.RegisterSubscriberListener<UpdatePlayerVisibleData_ForEnemyVisibility>();
            this.EventObserverLinker.RegisterSubscriberListener<RemovePlayerVisibleData_ForEnemyVisibility>();

            // Scanner 시야 값에 의한 Enemy Visibility 갱신
            this.EventObserverLinker.RegisterSubscriberListener<UpdateScanerVisibleData_ForEnemyVisibility>();
            this.EventObserverLinker.RegisterSubscriberListener<RemoveScanerVisibleData_ForEnemyVisibility>();

            // Test를 위한 Enemy Visibility 갱신
            this.EventObserverLinker.RegisterSubscriberListener<ShowAllEnemyVisibility>();
            this.EventObserverLinker.RegisterSubscriberListener<HideAllEnemyVisibility>();
            this.EventObserverLinker.RegisterSubscriberListener<ClearEnemyVisibilityData>();
        }

        private void OnDisable()
        {
            this.EventObserverLinker.Dispose();
        }

        public void UpdateData(IEventData eventData)
        {
            switch (eventData)
            {
                // 초기 설정
                case EnemyVisibilitySystemInitialSetting:
                    this.EnemyVisibilityController.InitialSetting();
                    break;

                // Player Unit 시야 값에 의한 Enemy Visibility 갱신
                case UpdatePlayerVisibleData_ForEnemyVisibility:
                    var data01 = (UpdatePlayerVisibleData_ForEnemyVisibility)eventData;
                    this.EnemyVisibilityController.UpdatePlayerVisibleData_ForEnemyVisibility(
                        data01.PlayerUniqueID, data01.VisibleRange, data01.PhysicalVisionOvercomeWeight, data01.SpiritualVisionOvercomeWeight);
                    this.EnemyVisibilityController.UpdateEnemyVisibility();
                    break;
                case RemovePlayerVisibleData_ForEnemyVisibility:
                    var data02 = (RemovePlayerVisibleData_ForEnemyVisibility)eventData;
                    this.EnemyVisibilityController.RemovePlayerVisibleData_ForEnemyVisibility(data02.PlayerUniqueID);
                    this.EnemyVisibilityController.UpdateEnemyVisibility();
                    break;

                // Scanner 시야 값에 의한 Enemy Visibility 갱신
                case UpdateScanerVisibleData_ForEnemyVisibility:
                    var data03 = (UpdateScanerVisibleData_ForEnemyVisibility)eventData;
                    this.EnemyVisibilityController.UpdateScanerVisibleData_ForEnemyVisibility(
                        data03.ScannerUniqueID, data03.VisibleRange, data03.PhysicalVisionOvercomeWeight, data03.SpiritualVisionOvercomeWeight);
                    this.EnemyVisibilityController.UpdateEnemyVisibility();
                    break;
                case RemoveScanerVisibleData_ForEnemyVisibility:
                    var data04 = (RemoveScanerVisibleData_ForEnemyVisibility)eventData;
                    this.EnemyVisibilityController.RemoveScanerVisibleData_ForEnemyVisibility(data04.ScannerUniqueID);
                    this.EnemyVisibilityController.UpdateEnemyVisibility();
                    break;

                // Test를 위한 Enemy Visibility 갱신
                case ShowAllEnemyVisibility:
                    this.EnemyVisibilityController.ShowEnemyVisibilityAll();
                    break;
                case HideAllEnemyVisibility:
                    this.EnemyVisibilityController.HideEnemyVisibilityAll();
                    break;
                case ClearEnemyVisibilityData:
                    this.EnemyVisibilityController.ClearEnemyVisibilityData();
                    break;
                default:
                    break;
            }
        }
    }
}