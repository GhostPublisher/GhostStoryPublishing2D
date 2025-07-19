using UnityEngine;

using Foundations.Architecture.EventObserver;

namespace GameSystems.PlayerSystem.PlayerUnitSystem
{
    public class PlayerUnitMouseInteractor : MonoBehaviour
    {
        private IEventObserverNotifier EventObserverNotifer;

        private IPlayerUnitSpriteRendererController PlayerUnitSpriteRendererController;

        private int myUniqueID;

        public void InitialSetting(PlayerUnitManagerData playerUnitManagerData)
        {
            this.EventObserverNotifer = new EventObserverNotifier();

            this.PlayerUnitSpriteRendererController = playerUnitManagerData.PlayerUnitFeatureInterfaceGroup.PlayerUnitSpriteRendererController;
            this.myUniqueID = playerUnitManagerData.UniqueID;
        }

        private void OnMouseEnter()
        {
            this.PlayerUnitSpriteRendererController.OnPointerEnter();
        }
        private void OnMouseExit()
        {
            this.PlayerUnitSpriteRendererController.OnPointerExit();
        }

        private void OnMouseDown()
        {
            this.PlayerUnitSpriteRendererController.OnPointerDown();
        }
        private void OnMouseUp()
        {
            this.PlayerUnitSpriteRendererController.OnPointerUp();

            UIUXSystem.PlayerUnitSpriteUX_ClickUpEvent eventData01 = new();
            eventData01.UniqueID = this.myUniqueID;

            this.EventObserverNotifer.NotifyEvent(eventData01);
            // 유닛을 클릭하였을 때는 모든 작업 중지 -> Icon 그룹 Show all + 해당 UnitID를 출력.
        }
    }
}