namespace GameSystems.EnemySystem.EnemyUnitSystem
{
    public interface IEnemyUnitSkillController
    {
        public int SkillID { get; }
        public SkillSlotType SkillSlotType { get; }

        public void InitialSetting(EnemyUnitManagerData enemyUnitManagerData);
    }
}