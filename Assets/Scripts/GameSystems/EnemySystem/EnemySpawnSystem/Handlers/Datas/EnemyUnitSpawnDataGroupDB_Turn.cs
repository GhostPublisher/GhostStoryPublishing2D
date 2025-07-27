using System;
using System.Collections.Generic;

using UnityEngine;

namespace GameSystems.EnemySystem.EnemySpawnSystem
{
    [Serializable]
    [CreateAssetMenu(menuName = "ScriptableObject/EnemySpawn/EnemyUnitSpawnDataGroupDB_Turn", fileName = "EnemyUnitSpawnDataGroupDB_Turn")]
    public class EnemyUnitSpawnDataGroupDB_Turn : ScriptableObject
    {
        [SerializeField] private List<EnemyUnitSpawnDataGroup_Turn> EnemyUnitSpawnDataGroup_Turns;

        public bool TryGetEnemyUnitSpawnDataGroup_Turn(int stageID, out EnemyUnitSpawnDataGroup_Turn enemyUnitSpawnDataGroup_Turn)
        {
            if(this.EnemyUnitSpawnDataGroup_Turns == null)
            {
                enemyUnitSpawnDataGroup_Turn = null;
                return false;
            }

            foreach (var data in this.EnemyUnitSpawnDataGroup_Turns)
            {
                if (data.StageID == stageID)
                {
                    enemyUnitSpawnDataGroup_Turn = data;
                    return true;
                }
            }

            enemyUnitSpawnDataGroup_Turn = null;
            return false;
        }
    }

    [Serializable]
    public class EnemyUnitSpawnDataGroup_Turn
    {
        public int StageID;

        public TextAsset EnemyUnitSpawnData_TurnJsonFile;
    }

    [Serializable]
    public class EnemyUnitSpawnData_Turn
    {
        public int TurnID;

        public UnitSpawnData[] UnitSpawnDatas;
    }

    [Serializable]
    public class EnemyUnitSpawnData_TurnArrayWrapper
    {
        public EnemyUnitSpawnData_Turn[] EnemyUnitSpawnData_Turns;
    }
}