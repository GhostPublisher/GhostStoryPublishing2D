/*using System;
using System.Collections.Generic;

using UnityEngine;

using Foundations.Architecture.ReferencesHandler;

using GameSystems.UtilitySystem;

namespace GameSystems.UnitSystem
{
    // 근접공격
    [Serializable]
    public class MeleeAttack_Single : MonoBehaviour, ISkillRangeDataProcessor
    {
        [SerializeField] private PlayerUnitOrEnemyUnit PlayerUnitOrEnemyUnit;
        // Range 형태
        [SerializeField] public ExtendPatternType ExtendPatternType;
        // 확장값
        [SerializeField] public int ExtendSize;
        // 극복치
        [SerializeField] private int OvercomeWeight;


        private CalculatedTargetRangeData calculatedTargetRangeData = new();

        // 계산을 위한 참조.
        private GridRangeCalculator GridRangeCalculator;

        private HashSet<IObstacleObjectSetter> obstacleObjectSetters;

        private GridAngleOcclusionFilter GridAngleOcclusionFilter;
        private HashSet<IObjectPresenceFilter> objectPresenceFilters;

        public void InitialSetting(GeneratedSkillData generatedSkillData)
        {
            generatedSkillData.CalculatedTargetRangeData = this.calculatedTargetRangeData;

            this.InitialProcessorSetting();
        }
        private void InitialProcessorSetting()
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;

            this.GridRangeCalculator = HandlerManager.GetUtilityHandler<GridRangeCalculator>();

            // obstacleObjectSetters 등록.
            this.obstacleObjectSetters = new();
            this.obstacleObjectSetters.Add(HandlerManager.GetUtilityHandler<ObstacleObjectSetter_Tile>());
            if(this.PlayerUnitOrEnemyUnit == PlayerUnitOrEnemyUnit.EnemyUnit)
                this.obstacleObjectSetters.Add(HandlerManager.GetUtilityHandler<ObstacleObjectSetter_PlayerUnit>());
            else if(this.PlayerUnitOrEnemyUnit == PlayerUnitOrEnemyUnit.PlayerUnit)
                this.obstacleObjectSetters.Add(HandlerManager.GetUtilityHandler<ObstacleObjectSetter_EnemyUnit>());


            // mainRangeFilter 등록.
            this.GridAngleOcclusionFilter = HandlerManager.GetUtilityHandler<GridAngleOcclusionFilter>();

            // additionalRangeFilter 등록.
            this.objectPresenceFilters = new();
            if (this.PlayerUnitOrEnemyUnit == PlayerUnitOrEnemyUnit.EnemyUnit)
                this.objectPresenceFilters.Add(HandlerManager.GetUtilityHandler<ObjectPresenceFilter_Player>());
            else if (this.PlayerUnitOrEnemyUnit == PlayerUnitOrEnemyUnit.PlayerUnit)
                this.objectPresenceFilters.Add(HandlerManager.GetUtilityHandler<ObjectPresenceFilter_Enemy>());
        }

        public void UpdateSkillRange(Vector2Int currentPosition)
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
                if (setter.TryGetObstacleObject(baseRange, WeightType.Skill, this.OvercomeWeight, out var returnObstacleRagne))
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
            var nonBlockedRange = this.GridAngleOcclusionFilter.GetUseableRange(currentPosition, baseRange, appliedOcclusionMap);

            // 스킬 특징에 맞지 않은 Range 제외.
            foreach (IObjectPresenceFilter filter in this.objectPresenceFilters)
            {
                // nonBlockedRange에서 IObjectPresenceFilter가 담당하는 object가 포함된 좌표값만 리턴.
                if (filter.TryGetPositionsContainingObject(nonBlockedRange, out var range))
                {
                    availableRange = range;
                }
            }

            // 해당 스킬 가공 결과 저장.
            this.calculatedTargetRangeData.BaseRange = baseRange;
            this.calculatedTargetRangeData.ObstacleObjectRange = new(appliedOcclusionMap.Keys);
            this.calculatedTargetRangeData.AvailableRange = availableRange;
        }
    }
}*/