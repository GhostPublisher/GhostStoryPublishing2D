using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace GameSystems.UIUXSystem
{
    public interface IPlayerUnitIconUIUXViewController
    {
        public void Update_UnitActionableState();
        public void ApplyActiveColor();
        public void ApplyInactiveColor();
    }

    public class PlayerUnitIconUIUXViewController : MonoBehaviour, IPlayerUnitIconUIUXViewController, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private Image UnitIconImageFrame;
        [SerializeField] private Image UnitIconImageSlot;

        [SerializeField] private Color FrameBaseColor;
        [SerializeField] private Color FrameInteractedColor;

        [SerializeField] private Color SlotInactivatedColor;
        [SerializeField] private Color SlotActivatedColor;

        [SerializeField] private Color SlotPointerDownColor;

        private PlayerUnitActionUIUXHandler myPlayerUnitActionUIUXHandler;

        private int myUniqueID;

        public void InitialSetting(PlayerUnitActionUIUXHandler playerUnitActionUIUXHandler, int uniqueID)
        {
            this.myPlayerUnitActionUIUXHandler = playerUnitActionUIUXHandler;
            this.myUniqueID = uniqueID;

            if (!this.myPlayerUnitActionUIUXHandler.PlayerUnitIconGroupUIUXDatas.TryGetValue(this.myUniqueID, out var playerUnitIconGroupUIUXData)) return;

            playerUnitIconGroupUIUXData.IPlayerUnitIconUIUXViewController = this;
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

            // 이동 TilemapUIUX, 이동 작업, 스킬 Tilemap UIUX, 스킬 작업이 수행 중인 경우, 해당 기능 Block.
            if (!this.myPlayerUnitActionUIUXHandler.IsInteractived) return;

            // UniqueID에 해당되는 UI 전환.
            this.myPlayerUnitActionUIUXHandler.IPlayerUnitIconGroupUIUXViewController.TogglePlayerBehaviourUIUX(this.myUniqueID);
            // UniqueID에 해당되는 UI 애니메이션 재생.
            this.myPlayerUnitActionUIUXHandler.IPlayerUnitIconGroupUIUXViewController.Show_BehaviourUIUX();
            // Mouse 인식 기능 활성화.
            this.myPlayerUnitActionUIUXHandler.IPlayerUnitActionPanelMouseInteractor.ActivateMouseInteractor();
        }

        public void ApplyActiveColor()
        {
            this.UnitIconImageSlot.color = this.SlotActivatedColor;
        }
        public void ApplyInactiveColor()
        {
            this.UnitIconImageSlot.color = this.SlotInactivatedColor;
        }

        public void Update_UnitActionableState()
        {
            // 이 곳에서는 Unit Icon Image 부분에 행동 코스트를 다 사용하였다는 느낌을 주는 무언가를 표시한다.
        }

    }
}