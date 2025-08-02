using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Foundations.Architecture.ReferencesHandler;
using GameSystems.EnemySystem.EnemyUnitSystem;

namespace GameSystems.PlayerSystem.PlayerUnitSystem
{
    /*        [Header("Skill Effect")]
        [Tooltip("'0.0' : Skill Animation 시작 지점. '1.0'은 Skill Animation의 종료 지점입니다.")]
        [SerializeField] private int SkillEffectID;
        [SerializeField, Range(0f, 2f)] private float EffectTrigger_NormalizedTime;
*/

    // Skill Effect 이후 데미지가 적용되어야 하는 경우에는, SkillEffect가 끝나는 시점에 Damage를 적용하도록 하던가,
    // SkillEffectID랑 Effect의 어느시점에서 Dmaage를 적용할지에 대한 HitTriggerTime을 같이 전달한다.
    public class Unit2002_Skill03 : MonoBehaviour, IPlayerSkillController
    {
        // EnemyUnit DB
        private EnemyUnitManagerDataDBHandler EnemyUnitManagerDataDBHandler;

        private IPlayerUnitSpriteRendererController playerUnitSpriteRendererController;
        private IPlayerUnitAnimationController playerUnitAnimationController;

        private PlayerUnitManagerData myPlayerUnitManagerData;

        [SerializeField] private int SkillID_;
        
        [SerializeField] private float HitTriggerTime;
        [SerializeField] private int SkillEffectID;
        [SerializeField] private float EffectTriggerTime;
        [SerializeField] private int DefaultDamage;

        public int SkillID => this.SkillID_;

        public void InitialSetting(PlayerUnitManagerData playerUnitManagerData)
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            this.EnemyUnitManagerDataDBHandler = HandlerManager.GetDynamicDataHandler<EnemyUnitManagerDataDBHandler>();

            this.myPlayerUnitManagerData = playerUnitManagerData;

            this.playerUnitSpriteRendererController = this.myPlayerUnitManagerData.PlayerUnitFeatureInterfaceGroup.PlayerUnitSpriteRendererController;
            this.playerUnitAnimationController = this.myPlayerUnitManagerData.PlayerUnitFeatureInterfaceGroup.PlayerUnitAnimationController;
        }

        public IEnumerator OperateSkill_Coroutine(Vector2Int targetedPosition)
        {
            yield return this.StartCoroutine(this.StartSkillOperation(targetedPosition));

            // Skill Cost 감소.
            this.myPlayerUnitManagerData.PlayerUnitDynamicData.BehaviourCost_Current -= this.myPlayerUnitManagerData.PlayerUnitStaticData.GetSkillActionCost(this.SkillID_);

            // 다 끝나면, Player Unit Action UIUX 갱신 및 상호작용 가능 명시.
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            var PlayerUnitActionUIUXHandler = HandlerManager.GetDynamicDataHandler<UIUXSystem.UIUXSystemHandler>().PlayerUnitActionUIUXHandler;

            // Player Cost 값에 따른 PlayerUnitActionUIUXHandler 업데이트하라는 코드. 
            PlayerUnitActionUIUXHandler.IPlayerUnitActionPanelUIMediator.Update_PlayerUnit_ActionableState();
            // Player Action UIUX 상호작용 가능 명시.
            PlayerUnitActionUIUXHandler.IsInteractived = true;
        }

        private IEnumerator StartSkillOperation(Vector2Int targetedPosition)
        {
            // 타겟 대상 interfaceGroup 가져옴.
            if (!this.EnemyUnitManagerDataDBHandler.TryGetEnemyUnitManagerData(targetedPosition, out var enemyUnitManagerData)) yield break;

            // 스킬 사용 방향으로 FlipX 변경
            this.playerUnitSpriteRendererController.UpdateFlipX(targetedPosition);
            // 해당 Animation 실행.
            this.playerUnitAnimationController.OperateAnimation(PlayerUnitAnimationType.Skill03_Default);

            // Enemy Unit이 Hit을 입는 시점.
            yield return new WaitForSeconds(this.HitTriggerTime);



            // Damage 전달 -> Animation + SpriteHit + Die 판별까지 '피격 유닛쪽에서 코루틴 수행'
            yield return enemyUnitManagerData.EnemyUnitFeatureInterfaceGroup.EnemyHitReactionController.TakeHitCoroutine_WithEffect(this.SkillEffectID, this.DefaultDamage);



            // Animation 종료.
            this.playerUnitAnimationController.OperateAnimation(PlayerUnitAnimationType.Idle);

            yield return null;
        }
    }
}
