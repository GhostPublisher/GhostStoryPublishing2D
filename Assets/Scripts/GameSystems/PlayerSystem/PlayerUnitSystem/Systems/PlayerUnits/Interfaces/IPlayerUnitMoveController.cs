using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace GameSystems.PlayerSystem.PlayerUnitSystem
{
    public interface IPlayerUnitMoveController
    {
        public void InitialSetting(PlayerUnitManagerData playerUnitManagerData);

        public IEnumerator OperateMove_Coroutine(Vector2Int targetPosition);

        public void MoveForce(Vector2Int forceDirection);
    }
}
