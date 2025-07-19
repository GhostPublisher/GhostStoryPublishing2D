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

        public StageEnemySpawnData_Turn GetStageEnemySpawnData_Turn(int stageID)
        {
            return this.StageEnemySpawnData_Turns.Find(entry => entry.StageID == stageID);
        }
    }

    [Serializable]
    public class StageEnemySpawnData_Turn
    {
        public int StageID;

        public TextAsset EnemySpawnData_TurnJsonFile;
    }
}