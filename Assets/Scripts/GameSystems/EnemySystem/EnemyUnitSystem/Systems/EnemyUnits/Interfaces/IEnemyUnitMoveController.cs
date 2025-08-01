using System.Collections;

using UnityEngine;

namespace GameSystems.EnemySystem.EnemyUnitSystem
{
    public interface IEnemyUnitMoveController
    {
        public void InitialSetting(EnemyUnitManagerData enemyUnitManagerData);
        public bool TryGetNextPosition_NearestTarget(out Vector2Int nextPosition);
        public IEnumerator OperateMoveBehaviour(Vector2Int nextPosition);
    }
}