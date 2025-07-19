using Foundations.Architecture.EventObserver;

namespace GameSystems.UIUXSystem
{
    public class ActivatePlayerUnitActionPanelUI : IEventData { }

    // 생성된 Player Units에 해당되는 PlayerUnitIcon 갱신
    public class UpdatePlayerUnitAcitionPanelUI : IEventData { }
    // 존재하는 Player Icon Group UIUX 삭제.
    public class ClearPlayerUnitAcitionPanelUI : IEventData { }


    // Player Unit Icon 을 클릭 업 수행 시, 작동하는 이벤트
    public class PlayerUnitIconUIUX_ClickUpEvent : IEventData
    {
        public int UniqueID;
    }
    // Player Unit Move Icon 을 클릭 업 수행 시, 작동하는 이벤트
    public class PlayerUnitMoveIconUIUX_ClickUpEvent : IEventData
    {
        public int UniqueID;
    }
    // Player Unit Skill Icon 을 클릭 업 수행 시, 작동하는 이벤트
    public class PlayerUnitSkillIconUIUX_ClickUpEvent : IEventData
    {
        public int UniqueID;
    }
    // Player Unit Sprite 클릭 업 수행 시, 작동하는 동작.
    public class PlayerUnitSpriteUX_ClickUpEvent : IEventData
    {
        public int UniqueID;
    }

    // Cancel 이벤트들.
    // 이동 작업 취소.
    public class PlayerMovementTilemap_CancelOrOperated : IEventData
    {
        public int PlayerUniqueID;
    }
    // 스킬 범위 출력 작업 취소.
    public class PlayerSkillRangeTilemap_CancelOrOperated : IEventData
    {
        public int PlayerUniqueID;
    }
}