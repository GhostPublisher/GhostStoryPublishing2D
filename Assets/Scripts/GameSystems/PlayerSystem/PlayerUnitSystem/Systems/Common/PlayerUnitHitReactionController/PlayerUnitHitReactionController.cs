using System.Collections;

using UnityEngine;

namespace GameSystems.PlayerSystem.PlayerUnitSystem
{
    public class PlayerUnitHitReactionController : MonoBehaviour, IPlayerUnitHitReactionController
    {
        private PlayerUnitManagerData myPlayerUnitManagerData;

        private IPlayerUnitAnimationController myPlayerUnitAnimationController;
        private IPlayerUnitStatusController myPlayerUnitStatusController;
        private IPlayerUnitEffectController myPlayerUnitEffectController;

        public void InitialSetting(PlayerUnitManagerData playerUnitManagerData)
        {
            this.myPlayerUnitManagerData = playerUnitManagerData;

            this.myPlayerUnitAnimationController = this.myPlayerUnitManagerData.PlayerUnitFeatureInterfaceGroup.PlayerUnitAnimationController;
            this.myPlayerUnitStatusController = this.myPlayerUnitManagerData.PlayerUnitFeatureInterfaceGroup.PlayerUnitStatusController;
            this.myPlayerUnitEffectController = this.myPlayerUnitManagerData.PlayerUnitFeatureInterfaceGroup.PlayerUnitEffectController;
        }

        public IEnumerator TakeHitCoroutine(int damage)
        {
            // 피격 애니메이션
            yield return this.myPlayerUnitAnimationController.PlayAndWaitAnimation(PlayerUnitAnimationType.IsHitted);
            // Hitted에서 Idle로 변경 대기. ( IsHitted에 의해 변경된 멤버 값을 복구. )
            this.myPlayerUnitAnimationController.OperateAnimation(PlayerUnitAnimationType.Idle);
            yield return null;

            // 데미지 Hit 연산 및 죽었을 경우 Die 애니메이션
            this.myPlayerUnitStatusController.ApplyDamage(damage);
            if (this.myPlayerUnitStatusController.IsDead)
            {
                // Die Animation
                yield return this.myPlayerUnitAnimationController.PlayAndWaitAnimation(PlayerUnitAnimationType.IsDie);
                // Die 작업.
                this.myPlayerUnitManagerData.PlayerUnitFeatureInterfaceGroup.PlayerUnitManager.OperateDie();
                yield return null;
            }
        }

        public IEnumerator TakeHitCoroutine(int effectID, int damage)
        {
            yield return this.myPlayerUnitEffectController.OperateEffect(effectID);

            yield return this.TakeHitCoroutine(damage);
        }
    }
}
