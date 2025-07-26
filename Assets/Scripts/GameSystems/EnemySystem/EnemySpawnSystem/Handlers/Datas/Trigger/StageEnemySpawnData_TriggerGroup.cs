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

        public bool TryGetStageEnemySpawnData_Turn(int stageID, out StageEnemySpawnData_Trigger stageEnemySpawnData_Trigger)
        {
            foreach (var data in this.StageEnemySpawnData_Triggers)
            {
                if (data.StageID == stageID)
                {
                    stageEnemySpawnData_Trigger = data;
                    return true;
                }
            }

            stageEnemySpawnData_Trigger = null;
            return false;
        }
    }

    [Serializable]
    public class StageEnemySpawnData_Trigger
    {
        public int StageID;

        public TextAsset EnemySpawnData_TriggerJsonFile;
    }

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