using UnityEngine;

using Foundations.Architecture.ReferencesHandler;

namespace GameSystems.PlayerSystem.PlayerUnitSystem
{
    public class PlayerUnitDataDBHandler : IStaticReferenceHandler
    {
        private PlayerUnitResourceDataGroup PlayerUnitResourceDataGroup;

        public PlayerUnitDataDBHandler()
        {
            this.LoadScriptableObject();
        }
        private void LoadScriptableObject()
        {
            this.PlayerUnitResourceDataGroup = Resources.Load<PlayerUnitResourceDataGroup>("ScriptableObject/Player/PlayerUnitResourceDataGroup");
        }

        public bool TryGetPlayerUnitResourceData(int unitID, out PlayerUnitResourceData playerUnitResourceDataGroup)
        {
            playerUnitResourceDataGroup = null;

            if (this.PlayerUnitResourceDataGroup == null) return false;

            return this.PlayerUnitResourceDataGroup.TryGetPlayerUnitResourceData(unitID, out playerUnitResourceDataGroup);
        }
    }
}