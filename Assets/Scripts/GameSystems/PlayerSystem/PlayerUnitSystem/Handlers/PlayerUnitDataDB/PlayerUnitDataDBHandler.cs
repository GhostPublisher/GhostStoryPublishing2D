using UnityEngine;

using Foundations.Architecture.ReferencesHandler;

namespace GameSystems.PlayerSystem.PlayerUnitSystem
{
    public class PlayerUnitDataDBHandler : IStaticReferenceHandler
    {
        private string PlayerUnitResourceDataGroup_SOPath = "ScriptableObject/Player/PlayerUnitResourceDataGroup";

        // EnemyPrefabResource SO 로드 시도.
        private bool TryLoadScriptableObject(out PlayerUnitResourceDataGroup playerUnitResourceDataGroup)
        {
            playerUnitResourceDataGroup = Resources.Load<PlayerUnitResourceDataGroup>(this.PlayerUnitResourceDataGroup_SOPath);

            // 파일 못찾으면 false 리턴.
            if (playerUnitResourceDataGroup == null) return false;
            return true;
        }

        public bool TryGetPlayerUnitResourceData(int unitID, out PlayerUnitResourceData playerUnitResourceData)
        {
            // SO를 못찾을 경우 false 리턴.
            if (!this.TryLoadScriptableObject(out var playerUnitResourceDataGroup))
            {
                Debug.LogError($"[EnemyUnitDataDBHandler] SO 위치 못 찾음.");
                playerUnitResourceData = null;
                return false;
            }

            return playerUnitResourceDataGroup.TryGetPlayerUnitResourceData(unitID, out playerUnitResourceData);
        }
    }
}