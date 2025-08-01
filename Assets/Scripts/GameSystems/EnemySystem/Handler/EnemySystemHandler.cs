using System;

using Foundations.Architecture.ReferencesHandler;

using GameSystems.EnemySystem.EnemySpawnSystem;
using GameSystems.EnemySystem.EnemyAISequenceSystem;

namespace GameSystems.EnemySystem
{
    [Serializable]
    public class EnemySystemHandler : IDynamicReferenceHandler
    {
        public IEnemyUnitSpawnController IEnemyUnitSpawnController { get; set; }

        public IEnemyAISequencer IEnemyAISequencer { get; set; }

    }
}