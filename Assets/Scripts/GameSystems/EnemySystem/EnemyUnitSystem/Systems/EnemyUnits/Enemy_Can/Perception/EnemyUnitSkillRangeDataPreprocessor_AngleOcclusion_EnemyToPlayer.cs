using System.Collections.Generic;

using UnityEngine;

using Foundations.Architecture.EventObserver;
using Foundations.Architecture.ReferencesHandler;

using GameSystems.UtilitySystem;

using GameSystems.PlayerSystem;

namespace GameSystems.EnemySystem.EnemyUnitSystem
{
    public interface IEnemyUnitSkillRangeDataPreprocessor
    {
        public int SkillID { get; }
        public SkillSlotType SkillSlotType { get; }
        public void InitialSetting(EnemyUnitManagerData enemyUnitManagerData);
        public void UpdateSkillDataPreprocessor();
    }

    // 공격이 장애물에 영향을 받는 경우 사용.
    public class EnemyUnitSkillRangeDataPreprocessor_AngleOcclusion_EnemyToPlayer : MonoBehaviour, IEnemyUnitSkillRangeDataPreprocessor
    {
        private PlayerUnitManagerDataDBHandler PlayerUnitManagerDataDBHandler;

        private SkillRangeCalculator_AngleOcclusion SkillRangeCalculator_AngleOcclusion;

        [SerializeField] private int SkillID_;
        [SerializeField] private SkillSlotType SkillSlotType_;

        [SerializeField] private List<Vector2Int> SkillTargetingRange;
        [SerializeField] private int SkillTargetingOvercomeWeight;

        [SerializeField] private SkillImpactDirection SkillImpactDirection;
        [SerializeField] private List<Vector2Int> SkillImpactOffset;
        [SerializeField] private int SkillImpactOvercomeWeight;

        private EnemyUnitManagerData myEnemyUnitManagerData;
        // Enemy는 '가공된 범위 데이터'를 '데이터로 보존해야지 다른 곳에서도 사용함'
        private SkillFilteredRange_Default skillFilteredRange;
        private SkillValidTargetRange_Default skillValidTargetRange;

        public int SkillID => this.SkillID_;
        public SkillSlotType SkillSlotType => this.SkillSlotType_;

        private void Awake()
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            this.SkillRangeCalculator_AngleOcclusion = HandlerManager.GetUtilityHandler<SkillRangeCalculator_AngleOcclusion>();
            this.PlayerUnitManagerDataDBHandler = HandlerManager.GetDynamicDataHandler<PlayerUnitManagerDataDBHandler>();
        }

        public void InitialSetting(EnemyUnitManagerData enemyUnitManagerData)
        {
            this.myEnemyUnitManagerData = enemyUnitManagerData;

            this.skillFilteredRange = new();
            this.skillValidTargetRange = new();

            this.myEnemyUnitManagerData.EnemyUnitDynamicData.SetSkillFilteredRangeData(this.SkillSlotType_, this.skillFilteredRange);
            this.myEnemyUnitManagerData.EnemyUnitDynamicData.SetSkillValidTargetRangeData(this.SkillSlotType_, this.skillValidTargetRange);
        }

        public void UpdateSkillDataPreprocessor()
        {
            this.UpdateEnemyUnitRangeData__SkillRange();
            this.UpdateCurrentMemoryData_SkillTargetablePositions();
        }

        // 타겟 범위 가져와서 저장함.
        public void UpdateEnemyUnitRangeData__SkillRange()
        {
            var BaseRange = this.GetAllRotatedDirections(this.SkillTargetingRange);

            var UpdateSkillTargetingRange = this.SkillRangeCalculator_AngleOcclusion.GetFilteredSkillRange_Enemy(
                this.myEnemyUnitManagerData.EnemyUnitGridPosition(), BaseRange, this.SkillTargetingOvercomeWeight);

            this.skillFilteredRange.FilteredRange = UpdateSkillTargetingRange;
        }

        // 타겟 범위 내, 실질적으로 적용가능한 위치 값을 찾아서 저장함.
        public void UpdateCurrentMemoryData_SkillTargetablePositions()
        {
            HashSet<Vector2Int> temp = new();
            foreach (Vector2Int pos in this.skillFilteredRange.FilteredRange)
            {
                // 해당 지역에 Player Unit 이 없으면 넘어감.
                if (!this.PlayerUnitManagerDataDBHandler.TryGetPlayerUnitManagerData(pos, out var _)) continue;

                temp.Add(pos);
            }

            if (temp.Count <= 0)
                this.skillValidTargetRange.ValidTargetPositions = null;
            else
                this.skillValidTargetRange.ValidTargetPositions = temp;
        }

        /// <summary>
        /// 여러 기준 벡터들을 받아 각 벡터를 시계 반대 방향으로 90도씩 회전하여 결과를 반환합니다.
        /// </summary>
        public HashSet<Vector2Int> GetAllRotatedDirections(List<Vector2Int> baseDirections)
        {
            HashSet<Vector2Int> result = new();

            foreach (var dir in baseDirections)
            {
                Vector2Int current = dir;

                for (int i = 0; i < 4; i++)
                {
                    result.Add(this.myEnemyUnitManagerData.EnemyUnitGridPosition() + current);
                    current = new Vector2Int(-current.y, current.x); // 시계 반대 방향 90도 회전
                }
            }

            return result;
        }
    }

    // Skill01의 적용 가능한 범위
    public class SkillFilteredRange_Default : ISkillFilteredRangeData
    {
        public HashSet<Vector2Int> FilteredRange { get; set; }
    }

    public class SkillValidTargetRange_Default : ISkillValidTargetRangeData
    {
        public HashSet<Vector2Int> ValidTargetPositions { get; set; }
    }

    public enum SkillImpactDirection
    {
        All,
        Front,
        Back,
        Left,
        Right
    }

    public enum SkillSlotType
    {
        Skill01,
        Skill02,
        Skill03
    }
}