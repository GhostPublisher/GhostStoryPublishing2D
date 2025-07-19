using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using Foundations.Architecture.EventObserver;

namespace GameSystems.UIUXSystem
{
    public interface IPlayerUnitIconUIUXViewController
    {
        public void ApplyActiveColor();
        public void ApplyInactiveColor();
    }

    public class PlayerUnitIconUIUXViewController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPlayerUnitIconUIUXViewController
    {
        private IEventObserverNotifier EventObserverNotifer;

        [SerializeField] private Image UnitIconImageFrame;
        [SerializeField] private Image UnitIconImageSlot;

        [SerializeField] private Color FrameBaseColor;
        [SerializeField] private Color FrameInteractedColor;

        [SerializeField] private Color SlotInactivatedColor;
        [SerializeField] private Color SlotActivatedColor;

        [SerializeField] private Color SlotPointerDownColor;

        private PlayerUnitIconGroupUIUXData myPlayerUnitIconGroupUIUXData;

        public void InitialSetting(PlayerUnitIconGroupUIUXData playerUnitIconGroupUIUXData)
        {
            this.EventObserverNotifer = new EventObserverNotifier();

            this.myPlayerUnitIconGroupUIUXData = playerUnitIconGroupUIUXData;
            this.myPlayerUnitIconGroupUIUXData.IPlayerUnitIconUIUXViewController = this;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            this.UnitIconImageFrame.color = this.FrameInteractedColor;
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            this.UnitIconImageFrame.color = this.FrameBaseColor;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            this.UnitIconImageSlot.color = this.SlotPointerDownColor;
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            this.UnitIconImageSlot.color = this.SlotActivatedColor;

            PlayerUnitIconUIUX_ClickUpEvent clickUpEvent = new PlayerUnitIconUIUX_ClickUpEvent();
            clickUpEvent.UniqueID = this.myPlayerUnitIconGroupUIUXData.UniqueID;

            this.EventObserverNotifer.NotifyEvent(clickUpEvent);
        }

        public void ApplyActiveColor()
        {
            this.UnitIconImageSlot.color = this.SlotActivatedColor;
        }
        public void ApplyInactiveColor()
        {
            this.UnitIconImageSlot.color = this.SlotInactivatedColor;
        }
    }
}