/*using System;
using System.Collections.Generic;

using UnityEngine;

using Foundations.Architecture.ReferencesHandler;

namespace GameSystems.UnitSystem
{
    public class BFSRangeDataFilterData
    {
        public HashSet<Vector2Int> BaseRange;
        public HashSet<Vector2Int> ObstacleObjectRange;

        public MovementPatternType MovementPatternType;
        public int RemainCost;

        public WeightType WeightType;
        public int OvercomeWeight;

        public Vector2Int CurrentUnitPosition;
    }

    [Serializable]
    public class BFSRangeDataFilter : IUtilityReferenceHandler
    {
        private GridRangeCalculator_MovePattern GridRangeCalculator_MovePattern;

        private HashSet<IObstacleObjectSetter> IObstacleObjectSetters;

        public BFSRangeDataFilter()
        {
            this.GridRangeCalculator_MovePattern = new GridRangeCalculator_MovePattern();

            this.IObstacleObjectSetters = new();
            this.IObstacleObjectSetters.Add(new ObstacleObjectSetter_PlayerUnit());
            this.IObstacleObjectSetters.Add(new ObstacleObjectSetter_EnemyUnit());
            this.IObstacleObjectSetters.Add(new ObstacleObjectSetter_Tile());
        }

        public HashSet<Vector2Int> UpdateData(BFSRangeDataFilterData BFSRangeDataFilterData_)
        {
            BFSRangeDataFilterData_.BaseRange = this.GridRangeCalculator_MovePattern
                .GetBaseRange(BFSRangeDataFilterData_.CurrentUnitPosition, BFSRangeDataFilterData_.MovementPatternType, BFSRangeDataFilterData_.RemainCost);

            foreach (IObstacleObjectSetter setter in this.IObstacleObjectSetters)
            {
                setter.TryGetObstacleObject(BFSRangeDataFilterData_.BaseRange, BFSRangeDataFilterData_.ObstacleObjectRange, BFSRangeDataFilterData_.WeightType, BFSRangeDataFilterData_.OvercomeWeight);
            }

            return this.GridRangeCalculator_MovePattern
                .GetAvailableRanges(BFSRangeDataFilterData_.BaseRange, BFSRangeDataFilterData_.ObstacleObjectRange, 
                                    BFSRangeDataFilterData_.CurrentUnitPosition, BFSRangeDataFilterData_.MovementPatternType, BFSRangeDataFilterData_.RemainCost);
        }
    }
}*/