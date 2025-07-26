using System;
using System.Collections.Generic;

using UnityEngine;

namespace GameSystems.EnemySystem.EnemySpawnSystem
{
    [Serializable]
    [CreateAssetMenu(menuName = "ScriptableObject/EnemySpawn/StageEnemySpawnData_TurnGroup", fileName = "StageEnemySpawnData_TurnGroup")]
    public class StageEnemySpawnData_TurnGroup : ScriptableObject
    {
        [SerializeField] private List<StageEnemySpawnData_Turn> StageEnemySpawnData_Turns;

        public bool TryGetStageEnemySpawnData_Turn(int stageID, out StageEnemySpawnData_Turn stageEnemySpawnData_Turn)
        {
            foreach (var data in this.StageEnemySpawnData_Turns)
            {
                if (data.StageID == stageID)
                {
                    stageEnemySpawnData_Turn = data;
                    return true;
                }
            }

            stageEnemySpawnData_Turn = null;
            return false;
        }
    }

    [Serializable]
    public class StageEnemySpawnData_Turn
    {
        public int StageID;

        public TextAsset EnemySpawnData_TurnJsonFile;
    }

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