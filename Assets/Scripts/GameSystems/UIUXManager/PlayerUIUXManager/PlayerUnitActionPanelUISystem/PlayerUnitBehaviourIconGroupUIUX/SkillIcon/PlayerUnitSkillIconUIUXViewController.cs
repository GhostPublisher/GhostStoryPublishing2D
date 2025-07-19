using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using Foundations.Architecture.EventObserver;

using GameSystems.PlayerSystem.PlayerUnitSystem;

namespace GameSystems.UIUXSystem
{
    public class PlayerUnitSkillIconUIUXViewController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
        private IEventObserverNotifier EventObserverNotifer;

        [SerializeField] private Image UnitIconImageFrame;
        [SerializeField] private Image UnitIconImageSlot;

        [SerializeField] private Color FrameBaseColor;
        [SerializeField] private Color FrameInteractedColor;

        [SerializeField] private Color SlotBaseColor;
        [SerializeField] private Color SlotPointerDownColor;

        private int myUniqueID;
        private int mySkillID;

        public void InitialSetting(int uniqueID, int skillID)
        {
            this.EventObserverNotifer = new EventObserverNotifier();

            this.myUniqueID = uniqueID;
            this.mySkillID = skillID;
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

            PlayerUnitSkillIconUIUX_ClickUpEvent clickUpEvent = new PlayerUnitSkillIconUIUX_ClickUpEvent();
            clickUpEvent.UniqueID = this.myUniqueID;

            UpdateSkillTargetingRange_PlayerUnit updateSkillTargetingRange_PlayerUnit = new();
            updateSkillTargetingRange_PlayerUnit.PlayerUniqueID = this.myUniqueID;
            updateSkillTargetingRange_PlayerUnit.SkillID = this.mySkillID;

            this.EventObserverNotifer.NotifyEvent(clickUpEvent);
            this.EventObserverNotifer.NotifyEvent(updateSkillTargetingRange_PlayerUnit);
        }
    }
}