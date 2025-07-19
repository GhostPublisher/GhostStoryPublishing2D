using System.Collections;

using UnityEngine;

namespace GameSystems.PlayerSystem.PlayerUnitSystem
{
    public class PlayerUnitAnimationController : MonoBehaviour, IPlayerUnitAnimationController
    {
        [SerializeField] private Animator PlayerUnitAnimator;

        public IEnumerator PlayAndWaitAnimation(PlayerUnitAnimationType animationType)
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

        public bool TryGetAnimationTotalTime(PlayerUnitAnimationType playerUnitAnimationType, out float totalTime)
        {
            var controller = PlayerUnitAnimator.runtimeAnimatorController;

            foreach (var clip in controller.animationClips)
            {
                if (clip.name == playerUnitAnimationType.ToString())
                {
                    totalTime = clip.length / this.PlayerUnitAnimator.speed; // 총 재생 시간(초)
                    return true;
                }
            }

            totalTime = 1;
            return false;
        }

        public void OperateAnimation(PlayerUnitAnimationType playerUnitAnimationType)
        {
            switch (playerUnitAnimationType)
            {
                case PlayerUnitAnimationType.Idle:
                    this.PlayerUnitAnimator.SetBool("IsWalk", false);
                    this.PlayerUnitAnimator.SetBool("Skill01_Default", false);
                    this.PlayerUnitAnimator.SetBool("Skill02_Default", false);
                    this.PlayerUnitAnimator.SetBool("Skill03_Default", false);
                    this.PlayerUnitAnimator.SetBool("IsHitted", false);
                    break;
                case PlayerUnitAnimationType.IsWalk:
                    this.PlayerUnitAnimator.SetBool("IsWalk", true);
                    break;
                case PlayerUnitAnimationType.Skill01_Default:
                    this.PlayerUnitAnimator.SetBool("Skill01_Default", true);
                    break;
                case PlayerUnitAnimationType.Skill02_Default:
                    this.PlayerUnitAnimator.SetBool("Skill02_Default", true);
                    break;
                case PlayerUnitAnimationType.Skill03_Default:
                    this.PlayerUnitAnimator.SetBool("Skill03_Default", true);
                    break;
                case PlayerUnitAnimationType.IsHitted:
                    this.PlayerUnitAnimator.SetBool("IsHitted", true);
                    break;
                case PlayerUnitAnimationType.IsDie:
                    this.PlayerUnitAnimator.SetBool("IsDie", true);
                    break;
                default:
                    break;
            }
        }
    }

    public enum PlayerUnitAnimationType
    {
        Idle,
        IsWalk,

        Skill01_Default,
        Skill02_Default,
        Skill03_Default,

        IsHitted,
        IsDie,

    }
}
