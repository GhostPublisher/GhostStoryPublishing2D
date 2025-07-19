/*using System;
using System.Collections.Generic;

using UnityEngine;

using Foundations.Architecture.ReferencesHandler;
using GameSystems.EnemySystem.EnemyUnitSystem;

using GameSystems.UtilitySystem;

namespace GameSystems.UnitSystem
{
    [Serializable]
    public class InteractionRangeProcessorPlugIn_Command : MonoBehaviour, EnemyIInteractionRangeProcessorPlugIn
    {
        // Range 형태
        [SerializeField] private ExtendPatternType ExtendPatternType;
        // 확장값
        [SerializeField] private int ExtendSize;
        // 극복 가중치.
        [SerializeField] private int OvercomeWeight;

        // 가중치 형식
        private WeightType WeightType = WeightType.Command;

        private InteractionRangeData_Command InteractionRangeData_Command;

        private GridRangeCalculator GridRangeCalculator;
        private HashSet<IObstacleObjectSetter> obstacleObjectSetters;
        private GridAngleOcclusionFilter GridAngleOcclusionFilter;

        public void InitialSetting(IInteractionRangeDataGroup interactionRangeDataGroup)
        {
            // 해당 PlugIn이 담당하는 값 정의.
            this.InteractionRangeData_Command = new();

            this.WeightType = WeightType.Command;
            // PlugIn Data를 Unit Data에 연결.
            interactionRangeDataGroup.InteractionRangeDatas.Add(this.GetType(), this.InteractionRangeData_Command);

            this.InitialSetting();
        }

        private void InitialSetting()
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;

            // 기본 모양.
            this.GridRangeCalculator = HandlerManager.GetUtilityHandler<GridRangeCalculator>();

            // 장애물 확인.
            this.obstacleObjectSetters = new();
            this.obstacleObjectSetters.Add(HandlerManager.GetUtilityHandler<ObstacleObjectSetter_PlayerUnit>());
            this.obstacleObjectSetters.Add(HandlerManager.GetUtilityHandler<ObstacleObjectSetter_Tile>());

            // 걸러진 정보.
            this.GridAngleOcclusionFilter = HandlerManager.GetUtilityHandler<GridAngleOcclusionFilter>();
        }

        public void UpdateInteractionRange(Vector2Int currentPosition)
        {
            HashSet<Vector2Int> baseRange = new();
            HashSet<Vector2Int> obstacleRange = new();
            HashSet<Vector2Int> availableRange = new();

            // 기본 범위 가져옴.
            baseRange = this.GridRangeCalculator.GetRange(currentPosition, this.ExtendPatternType, this.ExtendSize);

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

            // 실질적으로 '범위'를 가리는 Range와 가리는 범위 값을 가져옴.
            var appliedOcclusionMap = this.GridAngleOcclusionFilter.GetObstacleAngleRanges(currentPosition, obstacleRange);
            // 가려지지 않은 Range 들을 가져옴.
            availableRange = this.GridAngleOcclusionFilter.GetUseableRange(currentPosition, baseRange, appliedOcclusionMap);

            // 범위 값 변경.
            this.InteractionRangeData_Command.AvailableRange.Clear();
            foreach (Vector2Int pos in availableRange)
            {
                this.InteractionRangeData_Command.AvailableRange.Add(pos);
            }
        }

        public void InitialSetting(EnemyUnitManagerData generatedEnemyUnitData)
        {
            throw new NotImplementedException();
        }
    }

    [Serializable]
    public class InteractionRangeData_Command : IInteractionRangeData
    {
        public HashSet<Vector2Int> AvailableRange { get; }

        public InteractionRangeData_Command()
        {
            this.AvailableRange = new();
        }
    }
}*/