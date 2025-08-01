using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Foundations.Architecture.ReferencesHandler;
using GameSystems.PlayerSystem;

namespace GameSystems.EnemySystem.EnemyUnitSystem
{
    public class Enemy_Can_Skill01 : MonoBehaviour, IEnemyUnitSkillController
    {
        // PlayerUnit DB
        private PlayerUnitManagerDataDBHandler PlayerUnitManagerDataDBHandler;

        private IEnemyUnitSpriteRendererController EnemyUnitSpriteRendererController;
        private IEnemyUnitAnimationController EnemyUnitAnimationController;

        private EnemyUnitManagerData myEnemyUnitManagerData;

        [SerializeField] private int SkillID_;
        [SerializeField] private SkillSlotType SkillSlotType_;
        [SerializeField] private int SkillCost;
        
        [Header("Hit 지점")]
        [SerializeField, Range(0f, 2f)] private float HitTrigger_NormalizedTime;
        [SerializeField] private int DefaultDamage;

        public int SkillID => this.SkillID_;
        public SkillSlotType SkillSlotType => this.SkillSlotType_;

        public void InitialSetting(EnemyUnitManagerData enemyUnitManagerData)
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            this.PlayerUnitManagerDataDBHandler = HandlerManager.GetDynamicDataHandler<PlayerUnitManagerDataDBHandler>();

            this.myEnemyUnitManagerData = enemyUnitManagerData;

            this.EnemyUnitSpriteRendererController = this.myEnemyUnitManagerData.EnemyUnitFeatureInterfaceGroup.EnemyUnitSpriteRendererController;
            this.EnemyUnitAnimationController = this.myEnemyUnitManagerData.EnemyUnitFeatureInterfaceGroup.EnemyUnitAnimationController;
        }

        // 스킬 수행.
        public IEnumerator OperateSkill_Coroutine()
        {
            // 정상적으로 가까운 객체 위치값들 가져온 경우.
            if (this.TryGetNearestTarget(out var vector2Ints))
            {
                // 가장 먼저 탐색된 객체 공격.
                yield return StartCoroutine(this.OperateSkill(vector2Ints.First()));
            }
        }

        // 스킬 01의 인지한 적을 공격.
        // 가장 가까운 순으로 공격. 
        public bool TryGetNearestTarget(out HashSet<Vector2Int> nearestTargets)
        {
            // 스킬 01 공격 범위 내, 유효한 좌표 값을 가져오는 작업.
            // 해당 데이터 정보를 가져올 수 없거나, 데이터가 유효하지 않을 경우 '공격' 작업 실패 리턴.
            if (!this.myEnemyUnitManagerData.EnemyUnitDynamicData.TryGetSkillValidTargetRangeData(SkillSlotType.Skill01, out var ValidTargetRangeData)
                || ValidTargetRangeData == null || ValidTargetRangeData.ValidTargetPositions == null || ValidTargetRangeData.ValidTargetPositions.Count <= 0)
            {
                nearestTargets = null;
                return false;
            }

            var tempNearestTargets = this.GetNearestTarget(this.myEnemyUnitManagerData.EnemyUnitGridPosition(), ValidTargetRangeData.ValidTargetPositions);
            // 이럴일은 없겠지만 존나 오류임. 정상작동 실패.
            if (tempNearestTargets == null || tempNearestTargets.Count <= 0)
            {
                nearestTargets = null;
                return false;
            }
            // 가장 가까운 대상이 1개일 때, 바로 공격.
            else if (tempNearestTargets.Count == 1)
            {
                nearestTargets = tempNearestTargets;
                return true;
            }
            // 같은 거리에 있는 애들이 존재시, 추가 비교 수행. ( 일단 그냥 함 )
            else
            {
                nearestTargets = tempNearestTargets;
                return true;
            }
        }


        // currentPosition 위치 기준으로, validTargetPositions 들 중 가장 가까운 거리에 있는 좌표 반환.
        // 동일한 거리가 있을 경우 모두 리턴.
        private HashSet<Vector2Int> GetNearestTarget(Vector2Int currentPosition, HashSet<Vector2Int> validTargetPositions)
        {
            HashSet<Vector2Int> nearestPositions = new HashSet<Vector2Int>();
            int minDistance = int.MaxValue;

            foreach (var target in validTargetPositions)
            {
                int distance = Heuristic(currentPosition, target);

                if (distance < minDistance)
                {
                    // 더 가까운 거리 발견: 초기화
                    minDistance = distance;
                    nearestPositions.Clear();
                    nearestPositions.Add(target);
                }
                else if (distance == minDistance)
                {
                    // 동일 거리: 추가
                    nearestPositions.Add(target);
                }
            }

            return nearestPositions;
        }
        // 두 지점 간의 맨해튼 거리(예상 이동 비용)를 반환하는 휴리스틱 함수
        private int Heuristic(Vector2Int a, Vector2Int b)
        {
            return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
        }


        // 이 부분은 모든 스킬이 다 다를 듯. 어쩔수 없다.
        private IEnumerator OperateSkill(Vector2Int targetPosition)
        {
            yield return StartCoroutine(this.StartSkillOperation(targetPosition));

            this.myEnemyUnitManagerData.EnemyUnitDynamicData.CurrentSkillCost -= this.SkillCost;
        }

        private IEnumerator StartSkillOperation(Vector2Int targetedPosition)
        {
            // 타겟 대상 interfaceGroup 가져옴.
            if (!this.PlayerUnitManagerDataDBHandler.TryGetPlayerUnitManagerData(targetedPosition, out var playerUnitManagerData)) yield break;

            this.EnemyUnitSpriteRendererController.UpdateFlipX(targetedPosition);
            this.EnemyUnitAnimationController.OperateAnimation(EnemyUnitAnimationType.Skill01_Default);

            // '스킬 애니메이션'에 대응되는 Hit Trigger Time을 가져오는 방식.
            this.EnemyUnitAnimationController.TryGetAnimationTotalTime(EnemyUnitAnimationType.Skill01_Default, out float skillTotalTime);
            yield return new WaitForSeconds(skillTotalTime * this.HitTrigger_NormalizedTime);



            // Damage 전달 -> Animation + SpriteHit + Die 판별까지 '피격 유닛쪽에서 코루틴 수행'
            yield return playerUnitManagerData.PlayerUnitFeatureInterfaceGroup.PlayerUnitHitReactionController.TakeHitCoroutine(this.DefaultDamage);



            // Animation 종료.
            this.EnemyUnitAnimationController.OperateAnimation(EnemyUnitAnimationType.Idle);

            // Hit 이후의 애니메이션 시간.
            yield return null;
        }
    }
}