using System.Collections.Generic;

using UnityEngine;

using Foundations.Architecture.EventObserver;

namespace GameSystems.TilemapSystem.MovementTilemap
{
    public class InitialSetMovementTilemapEvent_Raw : IEventData
    {
        public int Width;
        public int Height;
    }

    public class ActivateMovementTilemapEvent : IEventData
    {
        public int PlayerUniqueID;
        public Vector2Int CurrentPosition;
        public HashSet<Vector2Int> MoveableRange;
    }

    public class DisActivateMovementTilemapEvent : IEventData { }
}