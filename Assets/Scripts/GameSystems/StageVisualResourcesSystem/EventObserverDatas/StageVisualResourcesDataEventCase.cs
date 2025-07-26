using System;

using UnityEngine;

using Foundations.Architecture.EventObserver;

namespace GameSystems.StageVisualSystem
{
    [Serializable]
    public class InitialSetStageVisualResourcesData : IEventData
    {
        public int StageID;
    }

    [Serializable]
    public class InitialSetStageVisualResourcesData_EventTest : IEventData
    {
        public StageVisualResourcesData StageVisualResourcesData;
    }

    [Serializable]
    public class ClearStageVisualResourcesDataEvent : IEventData { }
}