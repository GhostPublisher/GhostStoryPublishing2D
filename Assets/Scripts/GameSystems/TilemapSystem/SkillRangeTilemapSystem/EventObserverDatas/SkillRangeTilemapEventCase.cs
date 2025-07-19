using System;
using System.Collections.Generic;

using UnityEngine;

using Foundations.Architecture.EventObserver;

namespace GameSystems.TilemapSystem.SkillRangeTilemap
{
    [Serializable]
    public class InitialSetSkillRangeTilemapEvent_Raw : IEventData
    {
        public int Width;
        public int Height;
    }

    [Serializable]
    public class ActivateFilteredSkillRangeTilemapEvent : IEventData
    {
        public int PlayerUniqueID;
        public int SkillID;

        public Vector2Int CurrentPosition;
        public HashSet<Vector2Int> FilteredSkillRange;
        public HashSet<Vector2Int> SkillTargetPositions;
    }

    [Serializable]
    public class ActiavteSkillImpactRangeTilemap : IEventData
    {
        public Vector2Int MainTargetPosition;
        public HashSet<Vector2Int> FilteredSkillImpactRange;
        public HashSet<Vector2Int> AdditionalTargetPositions;
    }

    [Serializable]
    public class DisActivateSkillRangeTilemap : IEventData
    {

    }
}