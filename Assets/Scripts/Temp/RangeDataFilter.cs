/*using System;
using System.Collections.Generic;

using UnityEngine;

using Foundations.Architecture.ReferencesHandler;

namespace GameSystems.UnitSystem
{
    public class RangeDataFilterData
    {
        public HashSet<Vector2Int> BaseRange;
        public HashSet<Vector2Int> ObstacleObjectRange;

        public ExtendPatternType ExtendPatternType;
        public int ExtendSize;

        public WeightType WeightType;
        public int OvercomeWeight;

        public Vector2Int CurrentUnitPosition;
    }

    [Serializable]
    public class RangeDataFilter : IUtilityReferenceHandler
    {
        // 'Grid 범위 확장 방식 + 확장 크기'를 통한 '범위' 계산.
        private GridRangeCalculator GridRangeCalculator;

        private HashSet<IObstacleObjectSetter> obstacleObjectSetters;
        private IMainRangeFilter mainRangeFilter;

        public RangeDataFilter()
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;

            this.GridRangeCalculator = HandlerManager.GetUtilityHandler<GridRangeCalculator>();

            this.obstacleObjectSetters = new();
            this.obstacleObjectSetters.Add(HandlerManager.GetUtilityHandler<ObstacleObjectSetter_PlayerUnit>());
            this.obstacleObjectSetters.Add(HandlerManager.GetUtilityHandler<ObstacleObjectSetter_EnemyUnit>());
            this.obstacleObjectSetters.Add(HandlerManager.GetUtilityHandler<ObstacleObjectSetter_Tile>());

            this.mainRangeFilter = HandlerManager.GetUtilityHandler<GridAngleOcclusionFilter>();
        }

        public HashSet<Vector2Int> UpdateData(RangeDataFilterData rangeDataFilterData)
        {
            // 기본 범위 가져옴.
            rangeDataFilterData.BaseRange = this.GridRangeCalculator.GetRange(rangeDataFilterData.CurrentUnitPosition, rangeDataFilterData.ExtendPatternType, rangeDataFilterData.ExtendSize);

            // Obstacle Object Range 가져옴.
            foreach (IObstacleObjectSetter setter in this.obstacleObjectSetters)
            {
                setter.TryGetObstacleObject(rangeDataFilterData.BaseRange, rangeDataFilterData.ObstacleObjectRange, rangeDataFilterData.WeightType, rangeDataFilterData.OvercomeWeight);
            }

            return this.GetAvailableRange(rangeDataFilterData);
        }
        private HashSet<Vector2Int> GetAvailableRange(RangeDataFilterData rangeDataFilterData)
        {
            HashSet<Vector2Int> availableRange = new();

            // 범위 가져오기.
            foreach (Vector2Int basePos in rangeDataFilterData.BaseRange)
            {
                availableRange.Add(basePos);
            }

            // Setters에 의해 가려진 범위 가져오기.
            HashSet<Vector2Int> blockedRanges =
                this.mainRangeFilter.GetBlockedPositionsByObstacle(
                rangeDataFilterData.BaseRange, rangeDataFilterData.ObstacleObjectRange, rangeDataFilterData.CurrentUnitPosition);
            
            // 기본 범위에서 가려진 범위 사겢.
            foreach (Vector2Int blockedPos in blockedRanges)
            {
                availableRange.Remove(blockedPos);
            }

            // 기본 범위에서 
            foreach (Vector2Int obstaclePos in rangeDataFilterData.ObstacleObjectRange)
            {
                availableRange.Add(obstaclePos);
            }

            return availableRange;
        }
    }
}*/