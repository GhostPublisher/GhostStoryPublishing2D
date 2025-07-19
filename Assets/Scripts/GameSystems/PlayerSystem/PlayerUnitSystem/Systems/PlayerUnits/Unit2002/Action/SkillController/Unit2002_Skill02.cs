using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Foundations.Architecture.ReferencesHandler;

using GameSystems.UtilitySystem;
using GameSystems.EnemySystem.EnemyUnitSystem;

namespace GameSystems.PlayerSystem.PlayerUnitSystem
{
    public class Unit2002_Skill02 : MonoBehaviour, IPlayerSkillController
    {
        // 다중 피격 구현을 위한 코루틴 동시 종료 인식 유틸 클래스.
        private CoroutineUtility CoroutineUtility;
        // EnemyUnit DB
        private EnemyUnitManagerDataDBHandler EnemyUnitManagerDataDBHandler;

        private IPlayerUnitSpriteRendererController playerUnitSpriteRendererController;
        private IPlayerUnitAnimationController playerUnitAnimationController;

        private PlayerUnitManagerData myPlayerUnitManagerData;

        [SerializeField] private int SkillID_;
        [SerializeField] private int SkillCost;

        [SerializeField] private float HitTriggerTime;
        [SerializeField] private int DefaultDamage;

        public int SkillID => this.SkillID_;

        public void InitialSetting(PlayerUnitManagerData playerUnitManagerData)
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            this.EnemyUnitManagerDataDBHandler = HandlerManager.GetDynamicDataHandler<EnemyUnitManagerDataDBHandler>();
            this.CoroutineUtility = HandlerManager.GetUtilityComponentHandler<CoroutineUtility>();

            this.myPlayerUnitManagerData = playerUnitManagerData;

            this.playerUnitSpriteRendererController = this.myPlayerUnitManagerData.PlayerUnitFeatureInterfaceGroup.PlayerUnitSpriteRendererController;
            this.playerUnitAnimationController = this.myPlayerUnitManagerData.PlayerUnitFeatureInterfaceGroup.PlayerUnitAnimationController;
        }

        public void OperateSkill(Vector2Int targetedPosition)
        {
            this.StopAllCoroutines();

            this.StartCoroutine(this.SkillOperation(targetedPosition));
        }

        private IEnumerator SkillOperation(Vector2Int targetedPosition)
        {
            HashSet<Vector2Int> appliedUnitPositions = this.myPlayerUnitManagerData.PlayerUnitFeatureInterfaceGroup.PlayerUnitSkillRangeCalculators[this.SkillID_].GetSkillAppliedRange(targetedPosition);

            yield return this.StartCoroutine(this.StartSkillOperation(targetedPosition, appliedUnitPositions));

            this.myPlayerUnitManagerData.PlayerUnitDynamicData.CurrentSkillCost -= this.SkillCost;
        }

        // 언젠간 HashSet<Vector2Int> 가 아닌, HashSet<IHitReactionController> 가 넘어올거 같긴함.
        private IEnumerator StartSkillOperation(Vector2Int targetedPosition, HashSet<Vector2Int> appliedUnitPositions)
        {
            // 타겟 대상들의 interfaceGroup 가져옴.
            HashSet<EnemyUnitManagerData> EnemyUnitManagerDatas = new();
            foreach (var pos in appliedUnitPositions)
            {
                // 만약 해당 위치에 적이 없으면 오류.
                if (!this.EnemyUnitManagerDataDBHandler.TryGetEnemyUnitManagerData(pos, out EnemyUnitManagerData enemyUnitManagerData)) yield break;

                EnemyUnitManagerDatas.Add(enemyUnitManagerData);
            }

            // 스킬 사용 방향으로 FlipX 변경
            this.playerUnitSpriteRendererController.UpdateFlipX(targetedPosition);
            // 해당 Animation 실행.
            this.playerUnitAnimationController.OperateAnimation(PlayerUnitAnimationType.Skill02_Default);

            // Enemy Unit이 Hit을 입는 시점.
            yield return new WaitForSeconds(this.HitTriggerTime);



            List<IEnumerator> hitCoroutines = new();
            foreach (var data in EnemyUnitManagerDatas)
            {
                // Damage 전달 -> Animation + SpriteHit + Die 판별까지 '피격 유닛쪽에서 코루틴 수행'
                var HitCoroutine = data.EnemyUnitFeatureInterfaceGroup.EnemyHitReactionController.TakeHitCoroutine(this.DefaultDamage);

                hitCoroutines.Add(HitCoroutine);
            }

            // 동시 타격 애니메이션이 종료될 때까지 대기.
            yield return this.CoroutineUtility.WaitForAll(hitCoroutines);



            // Animation 종료.
            this.playerUnitAnimationController.OperateAnimation(PlayerUnitAnimationType.Idle);

            // 애니메이션 꼬이지 않도록 1프레임 대기.
            yield return null;
        }
    }
}
