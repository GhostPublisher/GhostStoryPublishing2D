namespace GameSystems.PlayerSystem.PlayerUnitSystem
{
    public interface IPlayerUnitMoveRangeCalculator
    {
        public void InitialSetting(PlayerUnitManagerData playerUnitManagerData);
        public void UpdateMoveableRange();
    }
}
