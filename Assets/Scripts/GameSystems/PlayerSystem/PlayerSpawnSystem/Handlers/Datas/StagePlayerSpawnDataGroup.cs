using System;
using System.Collections.Generic;

using UnityEngine;

namespace GameSystems.PlayerSystem.PlayerSpawnSystem
{
    [Serializable]
    [CreateAssetMenu(menuName = "ScriptableObject/PlayerSpawn/StagePlayerSpawnDataGroup", fileName = "StagePlayerSpawnDataGroup")]
    public class StagePlayerSpawnDataGroup : ScriptableObject
    {
        [SerializeField] private List<StagePlayerSpawnData> StagePlayerSpawnDatas;

        public bool TryGetStagePlayerSpawnDatas(int stageID, out HashSet<PlayerSpawnData> playerSpawnDatas)
        {
            playerSpawnDatas = null;

            if (this.StagePlayerSpawnDatas == null) return false;

            foreach (var data in this.StagePlayerSpawnDatas)
            {
                if(data.StageID == stageID)
                {
                    playerSpawnDatas = new HashSet<PlayerSpawnData>(data.PlayerSpawnDatas);
                    return true;
                }
            }

            return false;
        }
    }

    [Serializable]
    public class StagePlayerSpawnData
    {
        [SerializeField] public int StageID;
        [SerializeField] public List<PlayerSpawnData> PlayerSpawnDatas;
    }
}