using System;

using UnityEngine;

using Foundations.Architecture.EventObserver;

namespace GameSystems.TerrainSystem
{
    [Serializable]
    public class GenerateGroundTilemapEvent_Test : IEventData
    {
        public GameObject GroundTilemapGameObject;
    }

    [Serializable]
    public class ClearGroundTilemapEvent : IEventData { }
}