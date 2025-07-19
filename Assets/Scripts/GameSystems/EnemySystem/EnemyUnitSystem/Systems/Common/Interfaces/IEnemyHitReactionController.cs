using System.Collections;

namespace GameSystems.EnemySystem.EnemyUnitSystem
{
    public interface IEnemyHitReactionController
    {
        public void InitialSetting(EnemyUnitManagerData enemyUnitManagerData);
        public IEnumerator TakeHitCoroutine(int damage);
        public IEnumerator TakeHitCoroutine_WithEffect(int effectID, int damage);
    }
}