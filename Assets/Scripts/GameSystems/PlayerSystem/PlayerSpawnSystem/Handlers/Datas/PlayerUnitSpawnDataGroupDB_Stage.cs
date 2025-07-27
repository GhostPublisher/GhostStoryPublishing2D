using System;
using System.Collections.Generic;

using UnityEngine;

namespace GameSystems.PlayerSystem.PlayerSpawnSystem
{
    [Serializable]
    [CreateAssetMenu(menuName = "ScriptableObject/PlayerSpawn/PlayerUnitSpawnDataGroupDB_Stage", fileName = "PlayerUnitSpawnDataGroupDB_Stage")]
    public class PlayerUnitSpawnDataGroupDB_Stage : ScriptableObject
    {
        [SerializeField] private List<PlayerUnitSpawnDataGroup_Stage> PlayerUnitSpawnDataGroup_Stages;

        public bool TryGetPlayerUnitSpawnDataGroup_Stage(int stageID, out PlayerUnitSpawnDataGroup_Stage playerUnitSpawnDataGroup_Stage)
        {
            if (this.PlayerUnitSpawnDataGroup_Stages == null)
            {
                playerUnitSpawnDataGroup_Stage = null;
                return false;
            }

            foreach (var data in this.PlayerUnitSpawnDataGroup_Stages)
            {
                if(data.StageID == stageID)
                {
                    playerUnitSpawnDataGroup_Stage = data;
                    return true;
                }
            }

            playerUnitSpawnDataGroup_Stage = null;
            return false;
        }
    }

    [Serializable]
    public class PlayerUnitSpawnDataGroup_Stage
    {
        public int StageID;

        public TextAsset PlayerUnitSpawnData_StageJsonFile;
    }

    [Serializable]
    public class PlayerUnitSpawnData_Stage
    {
        public UnitSpawnData[] UnitSpawnDatas;
    }
}