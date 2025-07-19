using System;
using System.Collections.Generic;

using UnityEngine;

using Foundations.Architecture.EventObserver;

namespace GameSystems.EnemySystem.EnemyUnitSystem
{
    [Serializable]
    public class OperateEnemyAI_Raw : IEventData
    {
        public int UniqueID;
    }

    [Serializable]
    public class OperateNewTurnSetting : IEventData { }
}