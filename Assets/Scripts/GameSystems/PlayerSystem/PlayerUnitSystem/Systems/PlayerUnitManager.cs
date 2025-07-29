using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Foundations.Architecture.ReferencesHandler;

namespace GameSystems.PlayerSystem.PlayerUnitSystem
{
    public interface IPlayerUnitManager
    {
        public void OperateDie();

        public void OperatePlayerUnitInitialSetting();
        public IEnumerator OperatePlayerUnitInitialSetting_Coroutine();

        public void StopAllCoroutines();
    }

    public class PlayerUnitManager : MonoBehaviour, IPlayerUnitManager
    {
        // 공통적으로 사용.
        [SerializeField] private PlayerUnitStatusController PlayerUnitStatusController;

        [SerializeField] private PlayerUnitSpriteRendererController PlayerUnitSpriteRendererController;
        [SerializeField] private PlayerUnitAnimationController PlayerUnitAnimationController;

        [SerializeField] private PlayerUnitHitReactionController PlayerUnitHitReactionController;
        [SerializeField] private PlayerUnitEffectController PlayerUnitEffectController;

        [SerializeField] private PlayerUnitVisibilityController PlayerUnitVisibilityController;
        [SerializeField] private PlayerUnitMouseInteractor PlayerUnitMouseInteractor;

        // 개별적으로 다르거나, 나중에 일반화 할 예정.
        [SerializeField] private GameObject PlayerUnitInterfaceComponentContainer;
        // 가공
        private IPlayerUnitMoveRangeCalculator PlayerUnitMoveRangeCalculator;
        // 사용 ( 판단은 유저가 )
        private IPlayerUnitMoveController PlayerUnitMoveController;

        [SerializeField] private GameObject PlayerUnitSkillInterfaceComponentContainer;
        // 가공
        private Dictionary<int, IPlayerUnitSkillRangeCalculator> PlayerUnitSkillRangeCalculators;
        // 사용 ( 판단은 유저가 )
        private Dictionary<int, IPlayerSkillController> PlayerSkillControllers;

        private int UniqueID;
        // Player Unit 데이터 + 인터페이스 + Transform;
        private PlayerUnitManagerData myPlayerUnitManagerData;
        [SerializeField] private PlayerUnitStaticData PlayerUnitStaticData;
        private PlayerUnitDynamicData PlayerUnitDynamicData;
        private PlayerUnitFeatureInterfaceGroup PlayerUnitFeatureInterfaceGroup;

        private void OnDestroy()
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            var PlayerUnitManagerDataDBHandler = HandlerManager.GetDynamicDataHandler<PlayerUnitManagerDataDBHandler>();

            PlayerUnitManagerDataDBHandler.RemovePlayerUnitManagerData(this.myPlayerUnitManagerData);
        }


        public void OperatePlayerUnitInitialSetting()
        {
            this.InitialSetting();

            this.PlayerUnitVisibilityController.UpdateVisibleRange(this.myPlayerUnitManagerData.PlayerUnitGridPosition());
        }

        public IEnumerator OperatePlayerUnitInitialSetting_Coroutine()
        {
            this.InitialSetting();

            // 해당 위치의 시야 작업 수행.
            this.PlayerUnitVisibilityController.UpdateVisibleRange(this.myPlayerUnitManagerData.PlayerUnitGridPosition());

            // 카메라 포커싱.
            // 생성 애니메이션 재생.
            yield return StartCoroutine(this.PlayerUnitAnimationController.PlayAndWaitAnimation(PlayerUnitAnimationType.Spawn));

        }

        public void InitialSetting()
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            var PlayerUnitManagerDataDBHandler = HandlerManager.GetDynamicDataHandler<PlayerUnitManagerDataDBHandler>();

            this.UniqueID = this.gameObject.GetInstanceID();

            // PlugIn 조립.
            this.PlayerUnitMoveRangeCalculator = this.PlayerUnitInterfaceComponentContainer.GetComponent<IPlayerUnitMoveRangeCalculator>();
            this.PlayerUnitMoveController = this.PlayerUnitInterfaceComponentContainer.GetComponent<IPlayerUnitMoveController>();

            this.PlayerUnitSkillRangeCalculators = new();
            this.PlayerSkillControllers = new();
            foreach (var component in this.PlayerUnitSkillInterfaceComponentContainer.GetComponents<MonoBehaviour>())
            {
                if (component is IPlayerUnitSkillRangeCalculator playerUnitSkillRangeCalculator)
                {
                    this.PlayerUnitSkillRangeCalculators.Add(playerUnitSkillRangeCalculator.SkillID, playerUnitSkillRangeCalculator);
                }

                if (component is IPlayerSkillController playerSkillController)
                {
                    this.PlayerSkillControllers.Add(playerSkillController.SkillID, playerSkillController);
                }
            }

            // PlayerUnitDynamicData
            this.PlayerUnitDynamicData = new();

            // PlayerUnitFeatureInterfaceGroup
            this.PlayerUnitFeatureInterfaceGroup = new();
            this.PlayerUnitFeatureInterfaceGroup.PlayerUnitManager = this;
            this.PlayerUnitFeatureInterfaceGroup.PlayerUnitStatusController = this.PlayerUnitStatusController;

            this.PlayerUnitFeatureInterfaceGroup.PlayerUnitSpriteRendererController = this.PlayerUnitSpriteRendererController;
            this.PlayerUnitFeatureInterfaceGroup.PlayerUnitAnimationController = this.PlayerUnitAnimationController;

            this.PlayerUnitFeatureInterfaceGroup.PlayerUnitHitReactionController = this.PlayerUnitHitReactionController;
            this.PlayerUnitFeatureInterfaceGroup.PlayerUnitEffectController = this.PlayerUnitEffectController;

            this.PlayerUnitFeatureInterfaceGroup.PlayerUnitVisibilityController = this.PlayerUnitVisibilityController;

            foreach(var interfaceMember in this.PlayerUnitSkillRangeCalculators)
            {
                this.PlayerUnitFeatureInterfaceGroup.PlayerUnitSkillRangeCalculators.Add(interfaceMember.Key, interfaceMember.Value);
            }

            this.myPlayerUnitManagerData = new PlayerUnitManagerData(this.UniqueID, this.PlayerUnitStaticData, this.PlayerUnitDynamicData, this.PlayerUnitFeatureInterfaceGroup, this.transform);
            PlayerUnitManagerDataDBHandler.AddPlayerUnitManagerData(this.myPlayerUnitManagerData);

            this.SubObjectInitialSetting();
        }
        private void SubObjectInitialSetting()
        {
            this.PlayerUnitStatusController.InitialSetting(this.myPlayerUnitManagerData);

            this.PlayerUnitSpriteRendererController.InitialSetting(this.myPlayerUnitManagerData);

            this.PlayerUnitHitReactionController.InitialSetting(this.myPlayerUnitManagerData);
            this.PlayerUnitEffectController.InitialSetting(this.myPlayerUnitManagerData);

            this.PlayerUnitVisibilityController.InitialSetting(this.myPlayerUnitManagerData);

            this.PlayerUnitMoveRangeCalculator.InitialSetting(this.myPlayerUnitManagerData);
            this.PlayerUnitMoveController.InitialSetting(this.myPlayerUnitManagerData);

            this.PlayerUnitMouseInteractor.InitialSetting(this.myPlayerUnitManagerData);

            foreach (var comp in this.PlayerUnitSkillRangeCalculators.Values)
            {
                comp.InitialSetting(this.myPlayerUnitManagerData);
            }

            foreach (var comp in this.PlayerSkillControllers.Values)
            {
                comp.InitialSetting(this.myPlayerUnitManagerData);
            }
        }

        // Player Unit의 현재 위치를 기준으로, 주변 시야 범위를 갱신합니다.
        public void UpdateVisibleRange()
        {
            this.PlayerUnitVisibilityController.UpdateVisibleRange(this.myPlayerUnitManagerData.PlayerUnitGridPosition());
        }

        // Player Unit의 현재 위치를 기준으로, 현재 이동 가능 범위를 갱신합니다.
        public void UpdateMoveableRange(int playerUniqueID)
        {
            if (this.UniqueID != playerUniqueID) return;

            this.PlayerUnitMoveRangeCalculator.UpdateMoveableRange();
        }
        // Player의 Unit의 현재 위치를 기준으로, 전달받은 '목표' 위치'까지 A* 기반 이동 수행.
        // '목표 위치'는 '현재 위치'로 부터 이동 가능한 거리값 안에 존재함.
        public void OperatePlayerUnitMove(int playerUniqueID, Vector2Int targetPosition)
        {
            if (this.UniqueID != playerUniqueID) return;

            this.PlayerUnitMoveController.OperatePlayerUnitMove(targetPosition);
        }

        // Player의 현재 위치에 기반하여, Skill의 Targeting 가능 범위를 구한 뒤, Notify 합니다.
        public void UpdateSkillTargetingRange(int playerUniqueID, int skillID)
        {
            if(this.UniqueID != playerUniqueID) return;
            if (!this.PlayerUnitSkillRangeCalculators.TryGetValue(skillID, out var playerUnitSkillRangeCalculator)) return;

            playerUnitSkillRangeCalculator.UpdateSkillTargetingRange();
        }
        // Target 된 지점을 기준으로, Skill의 Impact 범위를 구한 뒤, Notify합니다.
        public void UpdateSkillImpactRange(int playerUniqueID, int skillID, Vector2Int targetedPosition)
        {
            if (this.UniqueID != playerUniqueID) return;
            if (!this.PlayerUnitSkillRangeCalculators.TryGetValue(skillID, out var playerUnitSkillRangeCalculator)) return;

            playerUnitSkillRangeCalculator.UpdateSkillImpactRange(targetedPosition);
        }
        // Target 된 지점을 기준으로 Player의 Skill을 동작합니다.
        public void OperateSkill(int playerUniqueID, int skillID, Vector2Int targetedPosition)
        {
//            Debug.Log($"OperateSkill - playerUniqueID : {playerUniqueID}. skillID : {skillID}. targetedPosition : {targetedPosition}");
            if (this.UniqueID != playerUniqueID) return;
            if (!this.PlayerSkillControllers.TryGetValue(skillID, out var playerSkillControllers)) return;

            playerSkillControllers.OperateSkill(targetedPosition);
        }

        public void OperateDie()
        {
            Destroy(this.gameObject);
        }
    }
}
