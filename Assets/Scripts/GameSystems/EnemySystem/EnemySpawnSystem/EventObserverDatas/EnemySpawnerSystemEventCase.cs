using System;

using UnityEngine;

using Foundations.Architecture.EventObserver;

namespace GameSystems.EnemySystem.EnemySpawnSystem
{
    [Serializable]
    public class EnemySpawnInitialSettingEvent : IEventData
    {
        public int StageID;
    }

    [Serializable]
    public class EnemySpawnEvent_Trigger : IEventData
    {
        public int TriggerID;
    }

    [Serializable]
    public class EnemySpawnEvent_Turn : IEventData
    {
        public int TurnID;
    }

    [Serializable]
    public class EnemySpawnEvent_Continue : IEventData
    {

    }

    [Serializable]
    public class EnemyClearEvent : IEventData
    {

    }

    [Serializable]
    public class EnemySpawnEvent_Test : IEventData
    {
        public int UnitID;
        public Vector2Int SpawnPosition;
    }
}