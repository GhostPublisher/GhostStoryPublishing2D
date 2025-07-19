namespace GameSystems.EnemySystem.EnemyUnitSystem
{
    public interface IEnemyUnitAIDataPreprocessor
    {
        public void InitialSetting(EnemyUnitManagerData enemyUnitManagerData);
        public void UpdateDataPreprocessor();
        public void UpdateEnemyUnitMemoryData_TurnFlow();
    }
}