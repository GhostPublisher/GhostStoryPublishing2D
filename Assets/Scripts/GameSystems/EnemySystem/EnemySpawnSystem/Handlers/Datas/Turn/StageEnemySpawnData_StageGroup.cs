using System;
using System.Collections.Generic;

using UnityEngine;

namespace GameSystems.EnemySystem.EnemySpawnSystem
{
    [Serializable]
    [CreateAssetMenu(menuName = "ScriptableObject/EnemySpawn/StageEnemySpawnData_StageGroup", fileName = "StageEnemySpawnData_StageGroup")]
    public class StageEnemySpawnData_StageGroup : ScriptableObject
    {
        [SerializeField] private List<StageEnemySpawnData_Stage> StageEnemySpawnData_Stages;

        public bool TryGetStageEnemySpawnData_Stage(int stageID, out StageEnemySpawnData_Stage stageEnemySpawnData_Stage)
        {
            foreach (var data in this.StageEnemySpawnData_Stages)
            {
                if (data.StageID == stageID)
                {
                    stageEnemySpawnData_Stage = data;
                    return true;
                }
            }

            stageEnemySpawnData_Stage = null;
            return false;
        }
    }

    [Serializable]
    public class StageEnemySpawnData_Stage
    {
        public int StageID;

        public TextAsset EnemySpawnData_StageJsonFile;
    }

    [Serializable]
    public class EnemySpawnData_Stage
    {
        public EnemySpawnData[] EnemySpawnDatas;
    }
}