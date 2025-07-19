using System;

using UnityEngine;

using Foundations.Architecture.EventObserver;

namespace GameSystems.PlayerSystem.PlayerUnitSystem
{
    [Serializable]
    public class UpdatePlayerUnitVisibleRange : IEventData { }

    [Serializable]
    public class MoveToTargetPosition_PlayerUnit : IEventData
    {
        public int PlayerUniqueID;
        public Vector2Int TargetPosition;
    }

    // 이동 범위 계산을 위한 EventObserver.
    [Serializable]
    public class UpdateMoveableRange_PlayerUnit : IEventData
    {
        public int PlayerUniqueID;
    }

    // 스킬 타겟팅 범위 계산을 위한 EventObserver.
    [Serializable]
    public class UpdateSkillTargetingRange_PlayerUnit : IEventData
    {
        public int PlayerUniqueID;
        public int SkillID;
    }

    // 스킬 적용 범위 계산을 위한 EventObserver.
    [Serializable]
    public class UpdateSkillImpactRange_PlayerUnit : IEventData
    {
        public int PlayerUniqueID;
        public int SkillID;
        public Vector2Int TargetedPosition;
    }

    [Serializable]
    public class OperateSkill_PlayerUnit : IEventData
    {
        public int PlayerUniqueID;
        public int SkillID;
        public Vector2Int TargetedPosition;
    }
}
