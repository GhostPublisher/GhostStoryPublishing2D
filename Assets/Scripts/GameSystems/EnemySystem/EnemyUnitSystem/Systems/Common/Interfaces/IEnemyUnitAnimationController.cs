using System.Collections;

namespace GameSystems.EnemySystem.EnemyUnitSystem
{
    public interface IEnemyUnitAnimationController
    {
        public IEnumerator PlayAndWaitAnimation(EnemyUnitAnimationType animationType);
        public bool TryGetAnimationTotalTime(EnemyUnitAnimationType enemyUnitAnimationType, out float totalTime);
        public void OperateAnimation(EnemyUnitAnimationType enemyUnitAnimationType);
    }
}