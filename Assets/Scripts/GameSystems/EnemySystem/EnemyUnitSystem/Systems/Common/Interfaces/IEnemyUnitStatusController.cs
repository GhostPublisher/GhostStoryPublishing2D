namespace GameSystems.EnemySystem.EnemyUnitSystem
{
    public interface IEnemyUnitStatusController
    {
        public void InitialSetting(EnemyUnitManagerData enemyUnitManagerData);
        public void ApplyDamage(int rawDamage);
        public bool IsDead { get; }
    }
}