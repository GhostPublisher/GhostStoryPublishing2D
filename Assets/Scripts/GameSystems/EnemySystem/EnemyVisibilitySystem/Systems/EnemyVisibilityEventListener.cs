using UnityEngine;

using Foundations.Architecture.EventObserver;

namespace GameSystems.EnemySystem.EnemyVisibilitySystem
{
    // Enemy�� visible, InVisible�� ������ ���� �����°�?
    // 1. Player�� �̵�.
    // 1-1. Player�� �þ� ���� ���� ��.
    // 1-2. Player�� OvercomeWeight ���� ��.

    // 2. Enemy�� �̵�.
    // 2-1. Enemy�� BlockWeight ���� ��.

    // 3. Scaner�� ���� Ư�� ���� Ȱ��ȭ.

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
            // �ʱ� ����
            this.EventObserverLinker.RegisterSubscriberListener<EnemyVisibilitySystemInitialSetting>();

            // Player Unit �þ� ���� ���� Enemy Visibility ����
            this.EventObserverLinker.RegisterSubscriberListener<UpdatePlayerVisibleData_ForEnemyVisibility>();
            this.EventObserverLinker.RegisterSubscriberListener<RemovePlayerVisibleData_ForEnemyVisibility>();

            // Scanner �þ� ���� ���� Enemy Visibility ����
            this.EventObserverLinker.RegisterSubscriberListener<UpdateScanerVisibleData_ForEnemyVisibility>();
            this.EventObserverLinker.RegisterSubscriberListener<RemoveScanerVisibleData_ForEnemyVisibility>();

            // Test�� ���� Enemy Visibility ����
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
                // �ʱ� ����
                case EnemyVisibilitySystemInitialSetting:
                    this.EnemyVisibilityController.InitialSetting();
                    break;

                // Player Unit �þ� ���� ���� Enemy Visibility ����
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

                // Scanner �þ� ���� ���� Enemy Visibility ����
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

                // Test�� ���� Enemy Visibility ����
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