using System.Collections;

using UnityEngine;

namespace GameSystems.EnemySystem.EnemyUnitSystem
{

    public class EnemyUnitAnimationController : MonoBehaviour, IEnemyUnitAnimationController
    {
        [SerializeField] private Animator EnemyUnitAnimator;

        public IEnumerator PlayAndWaitAnimation(EnemyUnitAnimationType animationType)
        {
            // 애니메이션 재생
            this.OperateAnimation(animationType);

            // 애니메이션 총 시간 구하기
            if (!this.TryGetAnimationTotalTime(animationType, out float totalTime))
            {
                totalTime = 1.0f; // 기본 fallback
            }

            // 해당 시간만큼 대기
            yield return new WaitForSeconds(totalTime);
        }

        public bool TryGetAnimationTotalTime(EnemyUnitAnimationType enemyUnitAnimationType, out float totalTime)
        {
            var controller = EnemyUnitAnimator.runtimeAnimatorController;

            foreach (var clip in controller.animationClips)
            {
                if (clip.name == enemyUnitAnimationType.ToString())
                {
                    totalTime = clip.length; // 총 재생 시간(초)
                    return true;
                }
            }

            totalTime = 1;
            return false;
        }

        public void OperateAnimation(EnemyUnitAnimationType enemyUnitAnimationType)
        {
            switch (enemyUnitAnimationType)
            {
                case EnemyUnitAnimationType.Idle:
                    this.EnemyUnitAnimator.SetBool("IsWalk", false);
                    this.EnemyUnitAnimator.SetBool("Skill01_Default", false);
//                    this.EnemyUnitAnimator.SetBool("Skill02_Default", false);
//                    this.EnemyUnitAnimator.SetBool("Skill03_Default", false);
                    this.EnemyUnitAnimator.SetBool("IsHitted", false);
                    this.EnemyUnitAnimator.SetBool("IsDie", false);
                    break;
                case EnemyUnitAnimationType.IsWalk:
                    this.EnemyUnitAnimator.SetBool("IsWalk", true);
                    break;
                case EnemyUnitAnimationType.Skill01_Default:
                    this.EnemyUnitAnimator.SetBool("Skill01_Default", true);
                    break;
                case EnemyUnitAnimationType.Skill02_Default:
                    this.EnemyUnitAnimator.SetBool("Skill02_Default", true);
                    break;
                case EnemyUnitAnimationType.Skill03_Default:
                    this.EnemyUnitAnimator.SetBool("Skill03_Default", true);
                    break;
                case EnemyUnitAnimationType.IsHitted:
                    this.EnemyUnitAnimator.SetBool("IsHitted", true);
                    break;
                case EnemyUnitAnimationType.IsDie:
                    this.EnemyUnitAnimator.SetBool("IsDie", true);
                    break;
                default:
                    break;
            }
        }
    }

    public enum EnemyUnitAnimationType
    {
        Spawn,

        Idle,
        IsWalk,

        IsHitted,
        IsDie,

        Skill01_Default,
        Skill02_Default,
        Skill03_Default,
    }
}