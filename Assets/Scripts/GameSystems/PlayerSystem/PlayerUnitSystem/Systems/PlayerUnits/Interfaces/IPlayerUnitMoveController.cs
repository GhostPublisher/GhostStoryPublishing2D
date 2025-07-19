
using UnityEngine;

namespace GameSystems.PlayerSystem.PlayerUnitSystem
{
    public interface IPlayerUnitMoveController
    {
        public void InitialSetting(PlayerUnitManagerData playerUnitManagerData);

        public void OperatePlayerUnitMove(Vector2Int targetPosition);

        public void MoveForce(Vector2Int forceDirection);
    }
}
