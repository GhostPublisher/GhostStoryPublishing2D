using System;
using System.Collections.Generic;

using UnityEngine;

namespace GameSystems.EnemySystem.EnemySpawnSystem
{
    [Serializable]
    [CreateAssetMenu(menuName = "ScriptableObject/EnemySpawn/StageEnemySpawnData_TriggerGroup", fileName = "StageEnemySpawnData_TriggerGroup")]
    public class StageEnemySpawnData_TriggerGroup : ScriptableObject
    {
        [SerializeField] private List<StageEnemySpawnData_Trigger> StageEnemySpawnData_Triggers;

        public StageEnemySpawnData_Trigger GetStageEnemySpawnData_Trigger(int stageID)
        {
            return this.StageEnemySpawnData_Triggers.Find(entry => entry.StageID == stageID);
        }
    }

    [Serializable]
    public class StageEnemySpawnData_Trigger
    {
        public int StageID;

        public TextAsset EnemySpawnData_TriggerJsonFile;
    }
}