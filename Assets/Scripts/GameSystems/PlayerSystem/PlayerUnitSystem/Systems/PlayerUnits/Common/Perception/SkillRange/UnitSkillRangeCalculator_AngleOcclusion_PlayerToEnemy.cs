using System.Collections.Generic;

using UnityEngine;

using Foundations.Architecture.EventObserver;
using Foundations.Architecture.ReferencesHandler;

using GameSystems.UtilitySystem;
using GameSystems.EnemySystem.EnemyUnitSystem;

using GameSystems.TilemapSystem.SkillRangeTilemap;

namespace GameSystems.PlayerSystem.PlayerUnitSystem
{

    // 이 부분에서 Target으로 지정하고 싶은, 유형을 Enum으로 SerializedField를 통해 받아서 switchcase로 target을 다양하게 명시할 수 있도록 하자.

    // 공격이 장애물에 영향을 받는 경우 사용.
    public class UnitSkillRangeCalculator_AngleOcclusion_PlayerToEnemy : MonoBehaviour, IPlayerUnitSkillRangeCalculator
    {
        private IEventObserverNotifier EventObserverNotifier;

        private EnemyUnitManagerDataDBHandler EnemyUnitManagerDataDBHandler;

        private SkillRangeCalculator_AngleOcclusion SkillRangeCalculator_AngleOcclusion;

        [SerializeField] private int skillID;

        [SerializeField] private List<Vector2Int> SkillTargetingRange;
        [SerializeField] private int SkillTargetingOvercomeWeight;

        [SerializeField] private SkillImpactDirection SkillImpactDirection;
        [SerializeField] private List<Vector2Int> SkillImpactOffset;
        [SerializeField] private int SkillImpactOvercomeWeight;

        private PlayerUnitManagerData myPlayerUnitManagerData;

        int IPlayerUnitSkillRangeCalculator.SkillID => this.skillID;

        private void Awake()
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            this.SkillRangeCalculator_AngleOcclusion = HandlerManager.GetUtilityHandler<SkillRangeCalculator_AngleOcclusion>();

            this.EventObserverNotifier = new EventObserverNotifier();

            this.EnemyUnitManagerDataDBHandler = HandlerManager.GetDynamicDataHandler<EnemyUnitManagerDataDBHandler>();
        }

        public void InitialSetting(PlayerUnitManagerData playerUnitManagerData)
        {
            this.myPlayerUnitManagerData = playerUnitManagerData;
        }

        public void UpdateSkillTargetingRange()
        {
            var BaseRange = this.GetAllRotatedDirections(this.SkillTargetingRange);

            var UpdateSkillTargetingRange = this.SkillRangeCalculator_AngleOcclusion.GetFilteredSkillRange_Player(
                this.myPlayerUnitManagerData.PlayerUnitGridPosition(), BaseRange, this.SkillTargetingOvercomeWeight);

            // Enemy 위치만 가져옴.
            var targetPositions = this.GetEnemyPositions(UpdateSkillTargetingRange);

            // Player Unit의 공격범위를 명시적으로 보여주기 위한 UIUX에 관련 정보를 전달하는 부분.
            // 여기서 Notify.
            ActivateFilteredSkillRangeTilemapEvent activateFilteredSkillRangeTilemapEvent = new();
            activateFilteredSkillRangeTilemapEvent.PlayerUniqueID = this.myPlayerUnitManagerData.UniqueID;
            activateFilteredSkillRangeTilemapEvent.SkillID = this.skillID;

            activateFilteredSkillRangeTilemapEvent.CurrentPosition = this.myPlayerUnitManagerData.PlayerUnitGridPosition();
            activateFilteredSkillRangeTilemapEvent.FilteredSkillRange = UpdateSkillTargetingRange;
            activateFilteredSkillRangeTilemapEvent.SkillTargetPositions = targetPositions;

            this.EventObserverNotifier.NotifyEvent(activateFilteredSkillRangeTilemapEvent);
        }

        public void UpdateSkillImpactRange(Vector2Int targetedPosition)
        {
            var BaseRange = this.GetBaseImpactRange(targetedPosition);

            var UpdateSkillTargetingRange = this.SkillRangeCalculator_AngleOcclusion.GetFilteredSkillRange_Player(
                targetedPosition, BaseRange, this.SkillTargetingOvercomeWeight);

            // 가공이 한번 더 필요할거 같긴 함. ( 벽과 타일이 존재하지 않는 부분 제외 코드 )
            var additionalTargetPositions = this.GetEnemyPositions(UpdateSkillTargetingRange);

            // 여기서 Notify. 
            ActiavteSkillImpactRangeTilemap actiavteSkillImpactRangeTilemap = new();
            actiavteSkillImpactRangeTilemap.MainTargetPosition = targetedPosition;
            actiavteSkillImpactRangeTilemap.FilteredSkillImpactRange = UpdateSkillTargetingRange;
            actiavteSkillImpactRangeTilemap.AdditionalTargetPositions = additionalTargetPositions;

            this.EventObserverNotifier.NotifyEvent(actiavteSkillImpactRangeTilemap);
        }

        public HashSet<Vector2Int> GetSkillAppliedRange(Vector2Int targetedPosition)
        {
            var BaseRange = this.GetBaseImpactRange(targetedPosition);

            var UpdateSkillTargetingRange = this.SkillRangeCalculator_AngleOcclusion.GetFilteredSkillRange_Player(
                targetedPosition, BaseRange, this.SkillTargetingOvercomeWeight);

            // 가공이 한번 더 필요할거 같긴 함. ( 벽과 타일이 존재하지 않는 부분 제외 코드 )
            var additionalTargetPositions = this.GetEnemyPositions(UpdateSkillTargetingRange);

            additionalTargetPositions.Add(targetedPosition);

            return additionalTargetPositions;
        }

        // Target - Current 좌표 기준으로 x > 0 이면 front, y > 0 이면 left 방향이다.
        private HashSet<Vector2Int> GetBaseImpactRange(Vector2Int targetedPosition)
        {
            Vector2Int direction = targetedPosition - this.myPlayerUnitManagerData.PlayerUnitGridPosition();

            // 방향 기준 계산
            bool isFront = direction.x > 0;
            bool isBack = direction.x < 0;
            bool isLeft = direction.y > 0;
            bool isRight = direction.y < 0;

            // 기본: front 기준 좌표 사용
            HashSet<Vector2Int> result = new();

            foreach (var offset in SkillImpactOffset)
            {
                Vector2Int rotated;

                if (isFront) // 그대로
                    rotated = offset;
                else if (isBack) // 뒤쪽: x 반전, y 반전
                    rotated = new Vector2Int(-offset.x, -offset.y);
                else if (isLeft) // 왼쪽: 좌회전 90도
                    rotated = new Vector2Int(-offset.y, offset.x);
                else if (isRight) // 오른쪽: 우회전 90도
                    rotated = new Vector2Int(offset.y, -offset.x);
                else // 제자리나 예외
                    rotated = offset;

                result.Add(targetedPosition + rotated); // 공격 대상 b 기준으로 결과 생성
            }

            return result;
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
                    result.Add(this.myPlayerUnitManagerData.PlayerUnitGridPosition() +  current);
                    current = new Vector2Int(-current.y, current.x); // 시계 반대 방향 90도 회전
                }
            }

            return result;
        }

        // 매개변수로 받은 범위에서 Enemy 위치만 특정시켜서 리턴함.
        private HashSet<Vector2Int> GetEnemyPositions(HashSet<Vector2Int> filteredRange)
        {
            HashSet<Vector2Int> result = new();

            foreach (Vector2Int pos in filteredRange)
            {
                if (!this.EnemyUnitManagerDataDBHandler.TryGetEnemyUnitManagerData(pos, out var _)) continue;

                result.Add(pos);
            }

            return result;
        }
    }

    public enum SkillImpactDirection
    {
        All,
        Front,
        Back,
        Left,
        Right
    }
}
