using System;
using System.Collections.Generic;

using UnityEngine;

namespace GameSystems.EnemySystem.EnemySpawnSystem
{
    [Serializable]
    [CreateAssetMenu(menuName = "ScriptableObject/EnemySpawn/EnemyUnitSpawnDataGroupDB_Stage", fileName = "EnemyUnitSpawnDataGroupDB_Stage")]
    public class EnemyUnitSpawnDataGroupDB_Stage : ScriptableObject
    {
        [SerializeField] private List<EnemyUnitSpawnDataGroup_Stage> EnemyUnitSpawnDataGroup_Stages;

        public bool TryGetEnemyUnitSpawnDataGroup_Stage(int stageID, out EnemyUnitSpawnDataGroup_Stage enemyUnitSpawnDataGroup_Stage)
        {
            if (this.EnemyUnitSpawnDataGroup_Stages == null)
            {
                enemyUnitSpawnDataGroup_Stage = null;
                return false;
            }

            foreach (var data in this.EnemyUnitSpawnDataGroup_Stages)
            {
                if (data.StageID == stageID)
                {
                    enemyUnitSpawnDataGroup_Stage = data;
                    return true;
                }
            }

            enemyUnitSpawnDataGroup_Stage = null;
            return false;
        }
    }

    [Serializable]
    public class EnemyUnitSpawnDataGroup_Stage
    {
        public int StageID;

        public TextAsset EnemyUnitSpawnData_StageJsonFile;
    }

    [Serializable]
    public class EnemyUnitSpawnData_Stage
    {
        public UnitSpawnData[] UnitSpawnDatas;
    }
}