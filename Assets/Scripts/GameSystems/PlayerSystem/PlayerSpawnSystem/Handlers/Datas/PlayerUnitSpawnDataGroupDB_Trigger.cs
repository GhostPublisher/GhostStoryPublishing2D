using System;
using System.Collections.Generic;

using UnityEngine;

namespace GameSystems.PlayerSystem.PlayerSpawnSystem
{
    [Serializable]
    [CreateAssetMenu(menuName = "ScriptableObject/PlayerSpawn/PlayerUnitSpawnDataGroupDB_Trigger", fileName = "PlayerUnitSpawnDataGroupDB_Trigger")]
    public class PlayerUnitSpawnDataGroupDB_Trigger : ScriptableObject
    {
        [SerializeField] private List<PlayerUnitSpawnDataGroup_Trigger> PlayerUnitSpawnDataGroup_Triggers;

        public bool TryGetPlayerUnitSpawnDataGroup_Trigger(int stageID, out PlayerUnitSpawnDataGroup_Trigger playerUnitSpawnDataGroup_Trigger)
        {
            if (this.PlayerUnitSpawnDataGroup_Triggers == null)
            {
                playerUnitSpawnDataGroup_Trigger = null;
                return false;
            }

            foreach (var data in this.PlayerUnitSpawnDataGroup_Triggers)
            {
                if (data.StageID == stageID)
                {
                    playerUnitSpawnDataGroup_Trigger = data;
                    return true;
                }
            }

            playerUnitSpawnDataGroup_Trigger = null;
            return false;
        }
    }

    [Serializable]
    public class PlayerUnitSpawnDataGroup_Trigger
    {
        public int StageID;

        public TextAsset PlayerUnitSpawnData_TriggerJsonFile;
    }

    [Serializable]
    public class PlayerUnitSpawnData_Trigger
    {
        public int TriggerID;

        public UnitSpawnData[] UnitSpawnDatas;
    }

    [Serializable]
    public class PlayerUnitSpawnData_TriggerArrayWrapper
    {
        public PlayerUnitSpawnData_Trigger[] PlayerUnitSpawnData_Triggers;
    }
}