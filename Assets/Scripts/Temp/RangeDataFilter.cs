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
        // 'Grid ���� Ȯ�� ��� + Ȯ�� ũ��'�� ���� '����' ���.
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
            // �⺻ ���� ������.
            rangeDataFilterData.BaseRange = this.GridRangeCalculator.GetRange(rangeDataFilterData.CurrentUnitPosition, rangeDataFilterData.ExtendPatternType, rangeDataFilterData.ExtendSize);

            // Obstacle Object Range ������.
            foreach (IObstacleObjectSetter setter in this.obstacleObjectSetters)
            {
                setter.TryGetObstacleObject(rangeDataFilterData.BaseRange, rangeDataFilterData.ObstacleObjectRange, rangeDataFilterData.WeightType, rangeDataFilterData.OvercomeWeight);
            }

            return this.GetAvailableRange(rangeDataFilterData);
        }
        private HashSet<Vector2Int> GetAvailableRange(RangeDataFilterData rangeDataFilterData)
        {
            HashSet<Vector2Int> availableRange = new();

            // ���� ��������.
            foreach (Vector2Int basePos in rangeDataFilterData.BaseRange)
            {
                availableRange.Add(basePos);
            }

            // Setters�� ���� ������ ���� ��������.
            HashSet<Vector2Int> blockedRanges =
                this.mainRangeFilter.GetBlockedPositionsByObstacle(
                rangeDataFilterData.BaseRange, rangeDataFilterData.ObstacleObjectRange, rangeDataFilterData.CurrentUnitPosition);
            
            // �⺻ �������� ������ ���� �灴.
            foreach (Vector2Int blockedPos in blockedRanges)
            {
                availableRange.Remove(blockedPos);
            }

            // �⺻ �������� 
            foreach (Vector2Int obstaclePos in rangeDataFilterData.ObstacleObjectRange)
            {
                availableRange.Add(obstaclePos);
            }

            return availableRange;
        }
    }
}*/