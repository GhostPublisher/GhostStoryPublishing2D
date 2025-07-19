using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using Foundations.Architecture.EventObserver;
using GameSystems.PlayerSystem.PlayerUnitSystem;

namespace GameSystems.UIUXSystem
{
    public class PlayerUnitMoveIconUIUXViewController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
        private IEventObserverNotifier EventObserverNotifer;

        [SerializeField] private Image UnitIconImageFrame;
        [SerializeField] private Image UnitIconImageSlot;

        [SerializeField] private Color FrameBaseColor;
        [SerializeField] private Color FrameInteractedColor;

        [SerializeField] private Color SlotBaseColor;
        [SerializeField] private Color SlotPointerDownColor;

        private int myUniqueID;

        public void InitialSetting(int uniqueID)
        {
            this.EventObserverNotifer = new EventObserverNotifier();

            this.myUniqueID = uniqueID;
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
            this.UnitIconImageSlot.color = this.SlotBaseColor;

            PlayerUnitMoveIconUIUX_ClickUpEvent clickUpEvent = new PlayerUnitMoveIconUIUX_ClickUpEvent();
            clickUpEvent.UniqueID = this.myUniqueID;

            UpdateMoveableRange_PlayerUnit UpdateMoveableRange_PlayerUnit = new UpdateMoveableRange_PlayerUnit();
            UpdateMoveableRange_PlayerUnit.PlayerUniqueID = this.myUniqueID;

            this.EventObserverNotifer.NotifyEvent(clickUpEvent);
            this.EventObserverNotifer.NotifyEvent(UpdateMoveableRange_PlayerUnit);
        }
    }
}