using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using Foundations.Architecture.ReferencesHandler;

namespace GameSystems.UIUXSystem
{
    public interface IPlayerUnitSkillIconUIUXViewController
    {
        public void Update_SkillActionableState();
    }

    public class PlayerUnitSkillIconUIUXViewController : MonoBehaviour, IPlayerUnitSkillIconUIUXViewController,
        IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private Image UnitIconImageFrame;
        [SerializeField] private Image UnitIconImageSlot;

        [SerializeField] private Color FrameBaseColor;
        [SerializeField] private Color FrameInteractedColor;

        [SerializeField] private Color SlotBaseColor;
        [SerializeField] private Color SlotPointerDownColor;

        private PlayerUnitActionUIUXHandler myPlayerUnitActionUIUXHandler;
        private PlayerSystem.PlayerUnitManagerData myPlayerUnitManagerData;

        private int myUniqueID;
        private int mySkillID;

        private bool isBlocked = false;

        public void InitialSetting(PlayerUnitActionUIUXHandler playerUnitActionUIUXHandler, int uniqueID, int skillID)
        {
            this.myPlayerUnitActionUIUXHandler = playerUnitActionUIUXHandler;
            this.myUniqueID = uniqueID;
            this.mySkillID = skillID;

            if(this.myPlayerUnitActionUIUXHandler.TryGetPlayerUnitIconGroupUIUXData(this.myUniqueID, out var playerUnitIconGroupUIUXData))
            {
                playerUnitIconGroupUIUXData.IPlayerUnitSkillIconUIUXViewControllers.Add(this.mySkillID, this);
            }

            var HandlerManager = LazyReferenceHandlerManager.Instance;
            var PlayerUnitManagerDataDBHandler = HandlerManager.GetDynamicDataHandler<PlayerSystem.PlayerUnitManagerDataDBHandler>();

            if (PlayerUnitManagerDataDBHandler.TryGetPlayerUnitManagerData(this.myUniqueID, out var data))
            {
                this.myPlayerUnitManagerData = data;
            }
        }

        public void Update_SkillActionableState()
        {
            this.isBlocked = this.myPlayerUnitManagerData.PlayerUnitDynamicData.BehaviourCost_Current < this.myPlayerUnitManagerData.PlayerUnitStaticData.GetSkillCost(this.mySkillID);
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

            if (this.isBlocked) return;

            // 이동 TilemapUIUX, 이동 작업, 스킬 Tilemap UIUX, 스킬 작업이 수행 중인 경우, 해당 기능 Block.
            if (!this.myPlayerUnitActionUIUXHandler.IsInteractived) return;

            this.myPlayerUnitActionUIUXHandler.IsInteractived = false;
            // UniqueID에 해당되는 UI 애니메이션 재생.
            this.myPlayerUnitActionUIUXHandler.IPlayerUnitIconGroupUIUXViewController.Hide_BehaviourUIUX();
            // Mouse 인식 기능 비활성화.
            this.myPlayerUnitActionUIUXHandler.IPlayerUnitActionPanelMouseInteractor.DIsActivateMouseInteractor();

            // Player Unit에게 해당 SkillID의 SkillRagne를 계산하여 출력하라는 요청 전달.
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            var PlayerUnitManagerDataDBHandler = HandlerManager.GetDynamicDataHandler<PlayerSystem.PlayerUnitManagerDataDBHandler>();

            if (PlayerUnitManagerDataDBHandler.TryGetPlayerUnitManagerData(this.myUniqueID, out var playerUnitManagerData))
            {
                if(playerUnitManagerData.PlayerUnitFeatureInterfaceGroup.PlayerUnitSkillRangeCalculators.TryGetValue(this.mySkillID, out var playerUnitSkillRangeCalculator))
                {
                    playerUnitSkillRangeCalculator.UpdateSkillTargetingRange();
                }
            }
        }
    }
}