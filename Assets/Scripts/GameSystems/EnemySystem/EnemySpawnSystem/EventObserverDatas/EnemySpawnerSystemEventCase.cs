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
    public class EnemySpawnSettingEvent_Trigger : IEventData
    {
        public int TriggerID;
    }

    [Serializable]
    public class EnemySpawnContinueEvent_Trigger : IEventData
    {

    }

    [Serializable]
    public class EnemySpawnSettingEvent_Turn : IEventData
    {
        public int TurnID;
    }

    [Serializable]
    public class EnemySpawnContinueEvent_Turn : IEventData
    {

    }

    [Serializable]
    public class EnemySpawnEvent_Test : IEventData
    {
        public int UnitID;
        public Vector2Int SpawnPosition;
    }

    [Serializable]
    public class EnemySpawnEvent_Continue : IEventData
    {

    }

    [Serializable]
    public class EnemyClearEvent : IEventData
    {

    }
}