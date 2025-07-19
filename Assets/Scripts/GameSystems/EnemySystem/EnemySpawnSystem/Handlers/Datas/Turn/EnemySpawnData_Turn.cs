using System;

namespace GameSystems.EnemySystem.EnemySpawnSystem
{
    [Serializable]
    public class EnemySpawnData_Turn
    {
        public int TurnID;

        public EnemySpawnData[] EnemySpawnDatas;
    }

    [Serializable]
    public class EnemySpawnData_TurnArrayWrapper
    {
        public EnemySpawnData_Turn[] EnemySpawnData_Turns;
    }
}