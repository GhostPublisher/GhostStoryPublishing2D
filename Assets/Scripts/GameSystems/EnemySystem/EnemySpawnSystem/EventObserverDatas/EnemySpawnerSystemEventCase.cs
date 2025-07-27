using System;

using UnityEngine;

using Foundations.Architecture.EventObserver;

namespace GameSystems.EnemySystem.EnemySpawnSystem
{
    [Serializable]
    public class EnemyUnitSpawnEvent_StageSetting : IEventData
    {
        public int StageID;
    }

    [Serializable]
    public class EnemyUnitSpawnEvent_Trigger : IEventData
    {
        public int TriggerID;
    }

    [Serializable]
    public class EnemyUnitSpawnEvent_TurnSetting : IEventData
    {
        public int TurnID;
    }

    [Serializable]
    public class EnemySpawnEvent_Continue : IEventData { }

    [Serializable]
    public class EnemyUnitClearEvent : IEventData { }

    [Serializable]
    public class EnemyUnitSpawnEvent_Test : IEventData
    {
        public int UnitID;
        public Vector2Int SpawnPosition;
    }
}