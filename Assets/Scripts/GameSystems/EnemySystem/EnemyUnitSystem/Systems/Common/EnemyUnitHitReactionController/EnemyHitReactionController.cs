using System.Collections;

using UnityEngine;

namespace GameSystems.EnemySystem.EnemyUnitSystem
{

    public class EnemyHitReactionController : MonoBehaviour, IEnemyHitReactionController
    {
        private EnemyUnitManagerData myEnemyUnitManagerData;

        private IEnemyUnitAnimationController myEnemyUnitAnimationController;
        private IEnemyUnitStatusController myEnemyUnitStatusController;
        private IEnemyUnitEffectController myEnemyUnitEffectController;

        public void InitialSetting(EnemyUnitManagerData enemyUnitManagerData)
        {
            this.myEnemyUnitManagerData = enemyUnitManagerData;

            this.myEnemyUnitAnimationController = this.myEnemyUnitManagerData.EnemyUnitFeatureInterfaceGroup.EnemyUnitAnimationController;
            this.myEnemyUnitStatusController = this.myEnemyUnitManagerData.EnemyUnitFeatureInterfaceGroup.EnemyUnitStatusController;
            this.myEnemyUnitEffectController = this.myEnemyUnitManagerData.EnemyUnitFeatureInterfaceGroup.EnemyUnitEffectController;
        }

        public IEnumerator TakeHitCoroutine(int damage)
        {
            // 피격 애니메이션
            yield return this.myEnemyUnitAnimationController.PlayAndWaitAnimation(EnemyUnitAnimationType.IsHitted);
            // Hitted에서 Idle로 변경 대기. ( IsHitted에 의해 변경된 멤버 값을 복구. )
            this.myEnemyUnitAnimationController.OperateAnimation(EnemyUnitAnimationType.Idle);
            yield return null;

            // 데미지 Hit 연산 및 죽었을 경우 Die 애니메이션
            this.myEnemyUnitStatusController.ApplyDamage(damage);
            if (this.myEnemyUnitStatusController.IsDead)
            {
                // Die Animation
                yield return this.myEnemyUnitAnimationController.PlayAndWaitAnimation(EnemyUnitAnimationType.IsDie);
                // Die 작업.
                this.myEnemyUnitManagerData.EnemyUnitFeatureInterfaceGroup.EnemyUnitManager.OperateDie();
                yield return null;
            }
        }

        public IEnumerator TakeHitCoroutine_WithEffect(int effectID, int damage)
        {
            yield return this.myEnemyUnitEffectController.OperateEffect(effectID);

            yield return this.TakeHitCoroutine(damage);
        }
    }
}