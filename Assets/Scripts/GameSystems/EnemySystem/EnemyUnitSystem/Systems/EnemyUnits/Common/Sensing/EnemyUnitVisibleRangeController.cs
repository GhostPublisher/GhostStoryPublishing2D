using System;
using System.Collections.Generic;

using UnityEngine;

using Foundations.Architecture.ReferencesHandler;

using GameSystems.UtilitySystem;

namespace GameSystems.EnemySystem.EnemyUnitSystem
{
    public class EnemyUnitVisibleRangeController
    {
        private VisibilityRangeCalculator VisibilityRangeCalculator;

        private EnemyUnitManagerData myEnemyUnitManagerData;

        // RangeData
        private EnemyUnitRangeData_Visible myEnemyUnitRangeData_Visible;

        public EnemyUnitVisibleRangeController(EnemyUnitManagerData enemyUnitManagerData)
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            this.VisibilityRangeCalculator = HandlerManager.GetUtilityHandler<VisibilityRangeCalculator>();

            this.myEnemyUnitManagerData = enemyUnitManagerData;
            this.myEnemyUnitRangeData_Visible = new();

            this.myEnemyUnitManagerData.EnemyUnitDynamicData.SetEnemyUnitRangeData<EnemyUnitRangeData_Visible>(this.myEnemyUnitRangeData_Visible);
        }
        
        public void UpdateEnemyUnitVisibleRange()
        {
            if (this.VisibilityRangeCalculator == null) return;

            var updatedVisibleRange = this.VisibilityRangeCalculator.GetFilteredVisibleRange_Enemy(this.myEnemyUnitManagerData.EnemyUnitGridPosition(),
                this.myEnemyUnitManagerData.EnemyUnitStaticData.VisibleSize, this.myEnemyUnitManagerData.EnemyUnitStaticData.VisibleOvercomeWeight);

            this.myEnemyUnitRangeData_Visible.VisibleRanges = updatedVisibleRange;
        }
    }

    public class EnemyUnitRangeData_Visible : IEnemyUnitRangeData
    {
        public HashSet<Vector2Int> VisibleRanges { get; set; }
    }
}