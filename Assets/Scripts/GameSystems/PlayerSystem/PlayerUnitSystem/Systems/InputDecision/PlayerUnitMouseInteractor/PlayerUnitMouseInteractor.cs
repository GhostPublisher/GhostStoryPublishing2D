using UnityEngine;

using Foundations.Architecture.ReferencesHandler;
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

            var HandlerManager = LazyReferenceHandlerManager.Instance;
            var PlayerUnitActionUIUXHandler = HandlerManager.GetDynamicDataHandler<GameSystems.UIUXSystem.UIUXSystemHandler>().PlayerUnitActionUIUXHandler;

            PlayerUnitActionUIUXHandler.IPlayerUnitActionPanelUIMediator.Show_PlayerUnitBehaviourPanel(this.myUniqueID);
        }
    }
}