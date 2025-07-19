using UnityEngine;

namespace GameSystems.EnemySystem.EnemyUnitSystem
{
    public class EnemyUnitAIDataPreprocessor_Can : MonoBehaviour, IEnemyUnitAIDataPreprocessor
    {
        private EnemyUnitManagerData myEnemyUnitManagerData;

        // Range
        private EnemyUnitVisibleRangeController EnemyUnitVisibleRnageController;

        // MemoryData
        private EnemyUnitMemoryDataController_PlayerUnit EnemyUnitMemoryDataController_PlayerUnit;

        public void InitialSetting(EnemyUnitManagerData enemyUnitManagerData)
        {
            this.myEnemyUnitManagerData = enemyUnitManagerData;

            this.EnemyUnitVisibleRnageController = new EnemyUnitVisibleRangeController(this.myEnemyUnitManagerData);

            this.EnemyUnitMemoryDataController_PlayerUnit = new EnemyUnitMemoryDataController_PlayerUnit(this.myEnemyUnitManagerData);
        }

        public void UpdateDataPreprocessor()
        {
            // 범위값 갱신.
            this.UpdateEnemyUnitRangeData();

            // 인지 데이터 갱신.
            this.UpdateEnemyUnitCurrentMemoryData();

            // 기억 데이터 갱신
            this.UpdateEnemyUnitMemoryData_WithCurrentDetectedData();
        }

        // 인지 범위 값 갱신.
        public void UpdateEnemyUnitRangeData()
        {
            this.EnemyUnitVisibleRnageController.UpdateEnemyUnitVisibleRange();
        }
        // 현재 인식 데이터 갱신.
        public void UpdateEnemyUnitCurrentMemoryData()
        {
            this.EnemyUnitMemoryDataController_PlayerUnit.UpdateCurrentMemoryData_PlayerUnit();
        }
        // 현재 인식 데이터를 기억 데이터에 넣기.
        public void UpdateEnemyUnitMemoryData_WithCurrentDetectedData()
        {
            this.EnemyUnitMemoryDataController_PlayerUnit.UpdateMemoryData_PlayerUnit_WithCurrentDetectedData();
        }
        // 시간의 흐름에 따라 Memory 데이터 갱신.
        public void UpdateEnemyUnitMemoryData_TurnFlow()
        {
            this.EnemyUnitMemoryDataController_PlayerUnit.UpdateMemoryData_PlayerUnit_TurnFlow();
        }
    }
}