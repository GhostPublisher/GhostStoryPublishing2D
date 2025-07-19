using System.Collections;

using UnityEngine;

using Foundations.Architecture.ReferencesHandler;

using GameSystems.EnemySystem.EnemyUnitSystem;

namespace GameSystems.PlayerSystem.PlayerUnitSystem
{
    public class Unit2002_Skill01 : MonoBehaviour, IPlayerSkillController
    {
        // EnemyUnit DB
        private EnemyUnitManagerDataDBHandler EnemyUnitManagerDataDBHandler;

        private IPlayerUnitSpriteRendererController PlayerUnitSpriteRendererController;
        private IPlayerUnitAnimationController PlayerUnitAnimationController;

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

            this.myPlayerUnitManagerData = playerUnitManagerData;

            this.PlayerUnitSpriteRendererController = this.myPlayerUnitManagerData.PlayerUnitFeatureInterfaceGroup.PlayerUnitSpriteRendererController;
            this.PlayerUnitAnimationController = this.myPlayerUnitManagerData.PlayerUnitFeatureInterfaceGroup.PlayerUnitAnimationController;
        }

        public void OperateSkill(Vector2Int targetedPosition)
        {
            this.StopAllCoroutines();

            this.StartCoroutine(this.SkillOperation(targetedPosition));
        }

        private IEnumerator SkillOperation(Vector2Int targetedPosition)
        {
            yield return this.StartCoroutine(this.StartSkillOperation(targetedPosition));

            Debug.Log($"CurrentSkillCost : {this.myPlayerUnitManagerData.PlayerUnitDynamicData.CurrentSkillCost} -> {this.myPlayerUnitManagerData.PlayerUnitDynamicData.CurrentSkillCost - this.SkillCost}");

            this.myPlayerUnitManagerData.PlayerUnitDynamicData.CurrentSkillCost -= this.SkillCost;
        }


        private IEnumerator StartSkillOperation(Vector2Int targetedPosition)
        {
            // 타겟 대상 interfaceGroup 가져옴.
            if (!this.EnemyUnitManagerDataDBHandler.TryGetEnemyUnitManagerData(targetedPosition, out var enemyUnitManagerData)) yield break;

            // 스킬 사용 방향으로 FlipX 변경
            this.PlayerUnitSpriteRendererController.UpdateFlipX(targetedPosition);
            // 해당 Animation 실행.
            this.PlayerUnitAnimationController.OperateAnimation(PlayerUnitAnimationType.Skill01_Default);

            // Enemy Unit이 Hit을 입는 시점.
            yield return new WaitForSeconds(this.HitTriggerTime);



            // Damage 전달 -> Animation + SpriteHit + Die 판별까지 '피격 유닛쪽에서 코루틴 수행'
            yield return enemyUnitManagerData.EnemyUnitFeatureInterfaceGroup.EnemyHitReactionController.TakeHitCoroutine(this.DefaultDamage);



            // Animation 종료.
            this.PlayerUnitAnimationController.OperateAnimation(PlayerUnitAnimationType.Idle);

            // 애니메이션 꼬이지 않도록 1프레임 대기.
            yield return null;
        }
    }
}
