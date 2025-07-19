/*using System;
using System.Collections.Generic;

using UnityEngine;

using GameSystems.UtilitySystem;
using Foundations.Architecture.ReferencesHandler;

namespace GameSystems.UnitSystem
{
    public interface IMoveRangeProcessor
    {
        public void UpdateMoveRangeData(Vector2Int currentPosition, int remainCost);
    }

    [Serializable]
    public class MoveRangeProcessor : MonoBehaviour, IMoveRangeProcessor
    {
        // 이동 방향 형태
        [SerializeField] private MovementPatternType MovementPatternType;
        // 가중치 형식
        [SerializeField] private WeightType WeightType;
        // 극복 가중치.
        [SerializeField] private int OvercomeWeight;

        private MoveRangeData moveRangeData;

        private HashSet<IObstacleObjectSetter> obstacleObjectSetters;

        private GridRangeCalculator_MovePattern GridRangeCalculator_MovePattern;

        public void InitialSetting(MoveRangeData moveRangeData)
        {
            // 해당 PlugIn이 담당하는 값 정의.
            this.moveRangeData = moveRangeData;

            var HandlerManager = LazyReferenceHandlerManager.Instance;

            // 기본 모양 + 이동 BFS
            this.GridRangeCalculator_MovePattern = HandlerManager.GetUtilityHandler<GridRangeCalculator_MovePattern>();

            // 장애물 확인.
            this.obstacleObjectSetters = new();
            this.obstacleObjectSetters.Add(HandlerManager.GetUtilityHandler<ObstacleObjectSetter_PlayerUnit>());
            this.obstacleObjectSetters.Add(HandlerManager.GetUtilityHandler<ObstacleObjectSetter_EnemyUnit>());
            this.obstacleObjectSetters.Add(HandlerManager.GetUtilityHandler<ObstacleObjectSetter_Tile>());

        }

        public void UpdateMoveRangeData(Vector2Int currentPosition, int remainCost)
        {
            HashSet<Vector2Int> baseRange = new();
            HashSet<Vector2Int> obstacleRange = new();
            HashSet<Vector2Int> availableRange = new();

            baseRange = this.GridRangeCalculator_MovePattern.GetBaseRange(currentPosition, this.MovementPatternType, remainCost);

            // Obstacle Object Range 가져옴.
            foreach (IObstacleObjectSetter setter in this.obstacleObjectSetters)
            {
                // 범위에 '장애물 객체'가 존재한다면,
                if (setter.TryGetObstacleObject(baseRange, this.WeightType, this.OvercomeWeight, out var returnObstacleRagne))
                {
                    // 장애물 추가.
                    foreach (Vector2Int pos in returnObstacleRagne)
                    {
                        obstacleRange.Add(pos);
                    }
                }
            }

            availableRange = this.GridRangeCalculator_MovePattern.GetAvailableRanges(
                this.moveRangeData.BaseRange, this.moveRangeData.ObstacleObjectRange, currentPosition, this.MovementPatternType, remainCost);

            // 범위 값 변경.
            this.moveRangeData.BaseRange.Clear();
            foreach (Vector2Int pos in baseRange)
            {
                this.moveRangeData.BaseRange.Add(pos);
            }

            // 범위 값 변경.
            this.moveRangeData.ObstacleObjectRange.Clear();
            foreach (Vector2Int pos in obstacleRange)
            {
                this.moveRangeData.ObstacleObjectRange.Add(pos);
            }

            // 범위 값 변경.
            this.moveRangeData.AvailableRange.Clear();
            foreach (Vector2Int pos in availableRange)
            {
                this.moveRangeData.AvailableRange.Add(pos);
            }
        }
    }

    [Serializable]
    public class MoveRangeData
    {
        public MoveRangeData()
        {
            this.BaseRange = new();
            this.ObstacleObjectRange = new();
            this.AvailableRange = new();
        }

        // 기본 범위
        public HashSet<Vector2Int> BaseRange { get; }
        // 이동 방해 장애물 범위
        public HashSet<Vector2Int> ObstacleObjectRange { get; }
        // 사용 가능한 범위
        public HashSet<Vector2Int> AvailableRange { get; }
    }
}*/