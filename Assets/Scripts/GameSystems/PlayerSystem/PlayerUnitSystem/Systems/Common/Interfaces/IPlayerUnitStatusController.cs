using System.Collections;

namespace GameSystems.PlayerSystem.PlayerUnitSystem
{
    public interface IPlayerUnitStatusController
    {
        public void InitialSetting(PlayerUnitManagerData playerUnitManagerData);
        public void ApplyDamage(int rawDamage);
        public bool IsDead { get; }
    }
}
