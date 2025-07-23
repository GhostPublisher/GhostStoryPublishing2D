using System;

using UnityEngine;

using Foundations.Architecture.EventObserver;

namespace GameSystems.TerrainSystem
{
    [Serializable]
    public class InitialSetStageVisualResourcesData_EventTest : IEventData
    {
        public GameObject GroundTilemapGameObject;
    }

    [Serializable]
    public class ClearStageVisualResourcesDataEvent : IEventData { }
}