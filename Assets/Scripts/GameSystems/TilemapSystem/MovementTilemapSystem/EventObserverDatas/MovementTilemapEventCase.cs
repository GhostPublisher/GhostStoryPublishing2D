using System;
using System.Collections.Generic;

using UnityEngine;

using Foundations.Architecture.EventObserver;

namespace GameSystems.TilemapSystem.MovementTilemap
{
    [Serializable]
    public class InitialSetMovementTilemapEvent : IEventData
    {
        public int StageID;
    }

    [Serializable]
    public class InitialSetMovementTilemapEvent_Raw : IEventData
    {
        public int Width;
        public int Height;
    }

    [Serializable]
    public class ActivateMovementTilemapEvent : IEventData
    {
        public int PlayerUniqueID;
        public Vector2Int CurrentPosition;
        public HashSet<Vector2Int> MoveableRange;
    }

    [Serializable]
    public class DisActivateMovementTilemapEvent : IEventData { }
}