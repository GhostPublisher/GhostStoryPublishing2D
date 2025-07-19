/*using System;
using System.Collections.Generic;

using UnityEngine;

using Foundations.Architecture.ReferencesHandler;

using GameSystems.EnemySystem.EnemyUnitSystem;
using GameSystems.UtilitySystem;

namespace GameSystems.UnitSystem
{
    [Serializable]
    public class EnemyInteractionRangeProcessorPlugIn_Visible : MonoBehaviour, EnemyIInteractionRangeProcessorPlugIn
    {
        // Range 형태
        [SerializeField] public ExtendPatternType ExtendPatternType;
        // 확장값
        [SerializeField] public int ExtendSize;
        // 극복치
        [SerializeField] public int OvercomeWeight;
        // 가중치 형식
        private WeightType WeightType = WeightType.Visible;

        private EnemyUnitManagerData myGeneratedEnemyUnitData;
        private InteractionRangeData_Visible myInteractionRangeData_Visible;

        private GridRangeCalculator GridRangeCalculator;
        private HashSet<IObstacleObjectSetter> obstacleObjectSetters;
        private GridAngleOcclusionFilter GridAngleOcclusionFilter;

        public void InitialSetting(EnemyUnitManagerData generatedEnemyUnitData)
        {
            // 해당 PlugIn이 담당하는 값 정의.
            this.myInteractionRangeData_Visible = new();
            this.WeightType = WeightType.Visible;
            // PlugIn Data를 Unit Data에 연결.
            this.myGeneratedEnemyUnitData = generatedEnemyUnitData;

            this.myGeneratedEnemyUnitData.InteractionRangeDatas.Add(this.myInteractionRangeData_Visible.GetType(), this.myInteractionRangeData_Visible);

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
            this.myInteractionRangeData_Visible.AvailableRange.Clear();
            foreach (Vector2Int pos in availableRange)
            {
                this.myInteractionRangeData_Visible.AvailableRange.Add(pos);
            }
        }
    }

    [Serializable]
    public class InteractionRangeData_Visible : IInteractionRangeData
    {
        public HashSet<Vector2Int> AvailableRange { get; }

        public InteractionRangeData_Visible()
        {
            this.AvailableRange = new();
        }
    }
}*/