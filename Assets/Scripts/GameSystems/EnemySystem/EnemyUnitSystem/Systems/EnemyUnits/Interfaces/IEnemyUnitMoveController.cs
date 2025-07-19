namespace GameSystems.EnemySystem.EnemyUnitSystem
{
    public interface IEnemyUnitMoveController
    {
        public void InitialSetting(EnemyUnitManagerData enemyUnitManagerData);
        public bool TryOperateMoveToTarget_NearestTarget();
    }
}