using UnityEngine;

namespace GameSystems.EnemySystem.EnemyUnitSystem
{

    public class EnemyUnitStatusController : MonoBehaviour, IEnemyUnitStatusController
    {
        private EnemyUnitManagerData myEnemyUnitManagerData;

        private EnemyUnitDynamicData myEnemyUnitDynamicData;

        public void InitialSetting(EnemyUnitManagerData enemyUnitManagerData)
        {
            this.myEnemyUnitManagerData = enemyUnitManagerData;
            this.myEnemyUnitDynamicData = this.myEnemyUnitManagerData.EnemyUnitDynamicData;
        }

        public void ApplyDamage(int rawDamage)
        {
            this.myEnemyUnitDynamicData.CurrentHPCost = Mathf.Max(0, this.myEnemyUnitDynamicData.CurrentHPCost - rawDamage);
        }

        public bool IsDead => this.myEnemyUnitDynamicData.CurrentHPCost <= 0;
    }
}