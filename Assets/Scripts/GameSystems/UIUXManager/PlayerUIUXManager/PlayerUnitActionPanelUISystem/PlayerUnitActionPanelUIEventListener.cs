using UnityEngine;

using Foundations.Architecture.EventObserver;

namespace GameSystems.UIUXSystem
{
    public class PlayerUnitActionPanelUIEventListener : MonoBehaviour, IEventObserverListener
    {
        private IEventObserverLinker EventObserverLinker;

        [SerializeField] private PlayerUnitActionPanelUIMediator PlayerUnitActionPanelUIMediator;

        private void Awake()
        {
            this.EventObserverLinker = new EventObserverLinker(this);
        }

        private void OnEnable()
        {
            // PlayerUnitActionPanelUI 최초? 활성화하는 요청.
            this.EventObserverLinker.RegisterSubscriberListener<ActivatePlayerUnitActionPanelUI>();

            // 생성된 Player Units에 해당되는 PlayerUnitIcon 갱신
            this.EventObserverLinker.RegisterSubscriberListener<UpdatePlayerUnitAcitionPanelUI>();
            // 존재하는 Player Icon Group UIUX 삭제.
            this.EventObserverLinker.RegisterSubscriberListener<ClearPlayerUnitAcitionPanelUI>();

            // Player Unit Icon 을 클릭 업 수행 시, 작동하는 이벤트
            this.EventObserverLinker.RegisterSubscriberListener<PlayerUnitIconUIUX_ClickUpEvent>();
            // Player Unit Move Icon 을 클릭 업 수행 시, 작동하는 이벤트
            this.EventObserverLinker.RegisterSubscriberListener<PlayerUnitMoveIconUIUX_ClickUpEvent>();
            // Player Unit Skill Icon 을 클릭 업 수행 시, 작동하는 이벤트
            this.EventObserverLinker.RegisterSubscriberListener<PlayerUnitSkillIconUIUX_ClickUpEvent>();
            // Player Unit Sprite 클릭 업 수행 시, 작동하는 동작.
            this.EventObserverLinker.RegisterSubscriberListener<PlayerUnitSpriteUX_ClickUpEvent>();

            this.EventObserverLinker.RegisterSubscriberListener<PlayerMovementTilemap_CancelOrOperated>();
            this.EventObserverLinker.RegisterSubscriberListener<PlayerSkillRangeTilemap_CancelOrOperated>();


        }

        private void OnDisable()
        {
            this.EventObserverLinker.Dispose();
        }

        public void UpdateData(IEventData eventData)
        {
            switch (eventData)
            {
                // PlayerUnitActionPanelUI 최초? 활성화? 하는 요청.
                case ActivatePlayerUnitActionPanelUI:
                    this.PlayerUnitActionPanelUIMediator.ShowPlayerUnitActionPanel_Half();
                    break;

                // 생성된 Player Units에 해당되는 PlayerUnitIcon 갱신
                case UpdatePlayerUnitAcitionPanelUI:
                    this.PlayerUnitActionPanelUIMediator.UpdateGeneratedPlayerUnitIconUIUX();
                    break;
                // 존재하는 Player Icon Group UIUX 삭제.
                case ClearPlayerUnitAcitionPanelUI:
                    this.PlayerUnitActionPanelUIMediator.ClearGeneratedPlayerUnitIconUIUX();
                    break;

                // Player Unit Icon 을 클릭 업 수행 시, 작동하는 이벤트
                case PlayerUnitIconUIUX_ClickUpEvent:
                    var data02 = (PlayerUnitIconUIUX_ClickUpEvent)eventData;
                    this.PlayerUnitActionPanelUIMediator.ShowPlayerUnitActionPanel_All(data02.UniqueID);
                    break;
                // Player Unit Move Icon 을 클릭 업 수행 시, 작동하는 이벤트
                case PlayerUnitMoveIconUIUX_ClickUpEvent:
                    this.PlayerUnitActionPanelUIMediator.HidePlayerUnitActionPanel_Half();
                    break;
                // Player Unit Skill Icon 을 클릭 업 수행 시, 작동하는 이벤트
                case PlayerUnitSkillIconUIUX_ClickUpEvent:
                    this.PlayerUnitActionPanelUIMediator.HidePlayerUnitActionPanel_Half();
                    break;
                // Player Unit Sprite 클릭 업 수행 시, 작동하는 동작.
                case PlayerUnitSpriteUX_ClickUpEvent:
                    var data03 = (PlayerUnitSpriteUX_ClickUpEvent)eventData;
                    this.PlayerUnitActionPanelUIMediator.ShowPlayerUnitActionPanel_All(data03.UniqueID);
                    break;

                case PlayerMovementTilemap_CancelOrOperated:
                    var data04 = (PlayerMovementTilemap_CancelOrOperated)eventData;
                    this.PlayerUnitActionPanelUIMediator.ShowPlayerUnitActionPanel_All(data04.PlayerUniqueID);
                    this.PlayerUnitActionPanelUIMediator.IsOverTilemapOperation();
                    break;
                case PlayerSkillRangeTilemap_CancelOrOperated:
                    var data05 = (PlayerSkillRangeTilemap_CancelOrOperated)eventData;
                    this.PlayerUnitActionPanelUIMediator.ShowPlayerUnitActionPanel_All(data05.PlayerUniqueID);
                    this.PlayerUnitActionPanelUIMediator.IsOverTilemapOperation();
                    break;
                default:
                    break;
            }
        }
    }
}