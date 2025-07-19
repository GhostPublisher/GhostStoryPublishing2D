using System.Collections;

namespace GameSystems.EnemySystem.EnemyUnitSystem
{
    public interface IEnemyUnitEffectController
    {
        public void InitialSetting(EnemyUnitManagerData enemyUnitManagerData);

        public IEnumerator OperateEffect(int effectID);
    }
}