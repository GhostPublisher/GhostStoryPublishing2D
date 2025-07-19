namespace GameSystems.EnemySystem.EnemyUnitSystem
{
    public interface IEnemyUnitAIBehaviourController
    {
        public void InitialSetting(EnemyUnitManagerData enemyUnitManagerData);
        public void DecideAIOperation();
    }
}