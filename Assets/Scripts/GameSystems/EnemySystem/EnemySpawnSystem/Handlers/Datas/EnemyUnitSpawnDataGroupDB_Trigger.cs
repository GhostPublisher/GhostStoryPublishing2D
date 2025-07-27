using System;
using System.Collections.Generic;

using UnityEngine;

namespace GameSystems.EnemySystem.EnemySpawnSystem
{
    [Serializable]
    [CreateAssetMenu(menuName = "ScriptableObject/EnemySpawn/EnemyUnitSpawnDataGroupDB_Trigger", fileName = "EnemyUnitSpawnDataGroupDB_Trigger")]
    public class EnemyUnitSpawnDataGroupDB_Trigger : ScriptableObject
    {
        [SerializeField] private List<EnemyUnitSpawnDataGroup_Trigger> EnemyUnitSpawnDataGroup_Triggers;

        public bool TryGetEnemyUnitSpawnDataGroup_Trigger(int stageID, out EnemyUnitSpawnDataGroup_Trigger enemyUnitSpawnDataGroup_Trigger)
        {
            foreach (var data in this.EnemyUnitSpawnDataGroup_Triggers)
            {
                if (data.StageID == stageID)
                {
                    enemyUnitSpawnDataGroup_Trigger = data;
                    return true;
                }
            }

            enemyUnitSpawnDataGroup_Trigger = null;
            return false;
        }
    }

    [Serializable]
    public class EnemyUnitSpawnDataGroup_Trigger
    {
        public int StageID;

        public TextAsset EnemyUnitSpawnData_TriggerJsonFile;
    }

    [Serializable]
    public class EnemyUnitSpawnData_Trigger
    {
        public int TriggerID;

        public UnitSpawnData[] UnitSpawnDatas;
    }

    [Serializable]
    public class EnemyUnitSpawnData_TriggerArrayWrapper
    {
        public EnemyUnitSpawnData_Trigger[] EnemyUnitSpawnData_Triggers;
    }
}