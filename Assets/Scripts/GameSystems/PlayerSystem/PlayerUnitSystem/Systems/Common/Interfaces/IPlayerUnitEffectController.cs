using System.Collections;

namespace GameSystems.PlayerSystem.PlayerUnitSystem
{
    public interface IPlayerUnitEffectController
    {
        public void InitialSetting(PlayerUnitManagerData playerUnitManagerData);

        public IEnumerator OperateEffect(int effectID);
    }
}
