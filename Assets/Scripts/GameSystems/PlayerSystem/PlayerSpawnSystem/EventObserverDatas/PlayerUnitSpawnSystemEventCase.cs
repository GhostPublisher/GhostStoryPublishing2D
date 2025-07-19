using System;

using UnityEngine;

using Foundations.Architecture.EventObserver;

namespace GameSystems.PlayerSystem.PlayerSpawnSystem
{
    [Serializable]
    public class PlayerUnitSpawnEvent_Continue : IEventData { }

    [Serializable]
    public class PlayerUnitSpawnEvent_StageSetting : IEventData
    {
        public int StageID;
    }
    [Serializable]
    public class PlayerUnitSpawnEvent_Trigger : IEventData
    {
        public int TriggerID;
    }
    [Serializable]
    public class PlayerUnitSpawnEvent_Clear : IEventData { }

    [Serializable]
    public class PlayerUnitSpawnEvent_Test : IEventData
    {
        public int UnitID;
        public Vector2Int SpawnPosition;
    }
}