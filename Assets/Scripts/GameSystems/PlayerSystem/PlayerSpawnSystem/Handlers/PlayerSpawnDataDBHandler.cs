using System.Collections.Generic;

using UnityEngine;

using Foundations.Architecture.ReferencesHandler;

namespace GameSystems.PlayerSystem.PlayerSpawnSystem
{
    public class PlayerSpawnDataDBHandler : IStaticReferenceHandler
    {
        private StagePlayerSpawnDataGroup StagePlayerSpawnDataGroup;
        private TriggerPlayerSpawnDataGroup TriggerPlayerSpawnDataGroup;

        public PlayerSpawnDataDBHandler()
        {
            this.LoadScriptableObject();
        }

        private void LoadScriptableObject()
        {
            this.StagePlayerSpawnDataGroup = Resources.Load<StagePlayerSpawnDataGroup>("ScriptableObject/PlayerSpawn/StagePlayerSpawnDataGroup");
            this.TriggerPlayerSpawnDataGroup = Resources.Load<TriggerPlayerSpawnDataGroup>("ScriptableObject/PlayerSpawn/TriggerPlayerSpawnDataGroup");
        }

        public bool TryGetStagePlayerSpawnData(int stageID, out HashSet<PlayerSpawnData> playerSpawnDatas)
        {
            playerSpawnDatas = null;

            if (this.StagePlayerSpawnDataGroup == null) return false;

            return this.StagePlayerSpawnDataGroup.TryGetStagePlayerSpawnDatas(stageID, out playerSpawnDatas);
        }

        public bool TryGetTriggerPlayerSpawnData(int triggerID, out HashSet<PlayerSpawnData> playerSpawnDatas)
        {
            playerSpawnDatas = null;

            if (this.TriggerPlayerSpawnDataGroup == null) return false;

            return this.TriggerPlayerSpawnDataGroup.TryGetTriggerPlayerSpawnDatas(triggerID, out playerSpawnDatas);
        }
    }
}
