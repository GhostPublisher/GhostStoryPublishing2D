using System.Collections;

namespace GameSystems.PlayerSystem.PlayerUnitSystem
{
    public interface IPlayerUnitHitReactionController
    {
        public void InitialSetting(PlayerUnitManagerData playerUnitManagerData);
        public IEnumerator TakeHitCoroutine(int damage);
        public IEnumerator TakeHitCoroutine(int effectID, int damage);
    }
}
