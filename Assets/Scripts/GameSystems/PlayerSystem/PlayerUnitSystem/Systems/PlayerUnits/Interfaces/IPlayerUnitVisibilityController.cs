using UnityEngine;

namespace GameSystems.PlayerSystem.PlayerUnitSystem
{
    public interface IPlayerUnitVisibilityController
    {
        public void InitialSetting(PlayerUnitManagerData generatedPlayerUnitData);
        public void UpdateVisibleRange(Vector2Int targetPosition);
    }
}