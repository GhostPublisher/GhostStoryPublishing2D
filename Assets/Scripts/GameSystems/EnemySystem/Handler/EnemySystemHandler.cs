using System;

using Foundations.Architecture.ReferencesHandler;

using GameSystems.EnemySystem.EnemySpawnSystem;

namespace GameSystems.EnemySystem
{
    [Serializable]
    public class EnemySystemHandler : IDynamicReferenceHandler
    {
        public IEnemyUnitSpawnController IEnemyUnitSpawnController { get; set; }


    }
}