using System;

namespace GameSystems.EnemySystem.EnemySpawnSystem
{
    [Serializable]
    public class EnemySpawnData_Trigger
    {
        public int TriggerID;

        public EnemySpawnData[] EnemySpawnDatas;
    }

    [Serializable]
    public class EnemySpawnData_TriggerArrayWrapper
    {
        public EnemySpawnData_Trigger[] EnemySpawnData_Triggers;
    }
}