using System.Collections;

namespace GameSystems.PlayerSystem.PlayerUnitSystem
{
    public interface IPlayerUnitAnimationController
    {
        public IEnumerator PlayAndWaitAnimation(PlayerUnitAnimationType animationType);
        public bool TryGetAnimationTotalTime(PlayerUnitAnimationType playerUnitAnimationType, out float totalTime);
        public void OperateAnimation(PlayerUnitAnimationType playerUnitAnimationType);
    }
}
