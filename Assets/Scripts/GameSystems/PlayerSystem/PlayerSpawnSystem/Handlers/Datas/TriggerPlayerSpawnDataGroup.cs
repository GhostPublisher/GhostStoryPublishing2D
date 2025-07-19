using System;
using System.Collections.Generic;

using UnityEngine;

namespace GameSystems.PlayerSystem.PlayerSpawnSystem
{
    [Serializable]
    [CreateAssetMenu(menuName = "ScriptableObject/PlayerSpawn/TriggerPlayerSpawnDataGroup", fileName = "TriggerPlayerSpawnDataGroup")]
    public class TriggerPlayerSpawnDataGroup : ScriptableObject
    {
        [SerializeField] private List<TriggerPlayerSpawnData> TriggerPlayerSpawnDatas;

        public bool TryGetTriggerPlayerSpawnDatas(int triggerID, out HashSet<PlayerSpawnData> playerSpawnDatas)
        {
            playerSpawnDatas = null;

            if (this.TriggerPlayerSpawnDatas == null) return false;

            foreach (var data in this.TriggerPlayerSpawnDatas)
            {
                if (data.StageID == triggerID)
                {
                    playerSpawnDatas = new HashSet<PlayerSpawnData>(data.PlayerSpawnDatas);
                    return true;
                }
            }

            return false;
        }
    }

    [Serializable]
    public class TriggerPlayerSpawnData
    {
        [SerializeField] public int StageID;
        [SerializeField] public List<PlayerSpawnData> PlayerSpawnDatas;
    }
}