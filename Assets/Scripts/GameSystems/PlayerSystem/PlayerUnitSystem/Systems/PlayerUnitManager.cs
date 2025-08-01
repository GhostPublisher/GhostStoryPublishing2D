using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Foundations.Architecture.ReferencesHandler;

namespace GameSystems.PlayerSystem.PlayerUnitSystem
{
    public interface IPlayerUnitManager
    {
        public void OperatePlayerUnitInitialSetting();
        public IEnumerator OperatePlayerUnitInitialSetting_Coroutine();

        public void OperateMove(int uniqueID, Vector2Int targetPosition);
        public void OperateSkill(int uniqueID, int skillID, Vector2Int targetedPosition);
        public void OperateDie();

        public void OperateTurnEndSetting();
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
        [SerializeField] private PlayerUnitStaticData myPlayerUnitStaticData;
        [SerializeField] private PlayerUnitDynamicData myPlayerUnitDynamicData;
        private PlayerUnitFeatureInterfaceGroup myPlayerUnitFeatureInterfaceGroup;


        private void OnDestroy()
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            var PlayerUnitManagerDataDBHandler = HandlerManager.GetDynamicDataHandler<PlayerUnitManagerDataDBHandler>();
            var PlayerUnitActionUIUXHandler = HandlerManager.GetDynamicDataHandler<UIUXSystem.UIUXSystemHandler>().PlayerUnitActionUIUXHandler;

            PlayerUnitManagerDataDBHandler.RemovePlayerUnitManagerData(this.myPlayerUnitManagerData);

            PlayerUnitActionUIUXHandler.IPlayerUnitActionPanelUIMediator.Update_PlayerUnitIconUIUX();
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
            this.myPlayerUnitDynamicData = new();

            // PlayerUnitFeatureInterfaceGroup
            this.myPlayerUnitFeatureInterfaceGroup = new();
            this.myPlayerUnitFeatureInterfaceGroup.PlayerUnitManager = this;
            this.myPlayerUnitFeatureInterfaceGroup.PlayerUnitStatusController = this.PlayerUnitStatusController;

            this.myPlayerUnitFeatureInterfaceGroup.PlayerUnitSpriteRendererController = this.PlayerUnitSpriteRendererController;
            this.myPlayerUnitFeatureInterfaceGroup.PlayerUnitAnimationController = this.PlayerUnitAnimationController;

            this.myPlayerUnitFeatureInterfaceGroup.PlayerUnitHitReactionController = this.PlayerUnitHitReactionController;
            this.myPlayerUnitFeatureInterfaceGroup.PlayerUnitEffectController = this.PlayerUnitEffectController;

            this.myPlayerUnitFeatureInterfaceGroup.PlayerUnitVisibilityController = this.PlayerUnitVisibilityController;
            this.myPlayerUnitFeatureInterfaceGroup.IPlayerUnitMoveRangeCalculator = this.PlayerUnitMoveRangeCalculator;
            foreach (var interfaceMember in this.PlayerUnitSkillRangeCalculators)
            {
                this.myPlayerUnitFeatureInterfaceGroup.PlayerUnitSkillRangeCalculators.Add(interfaceMember.Key, interfaceMember.Value);
            }

            this.myPlayerUnitFeatureInterfaceGroup.IPlayerUnitMoveController = this.PlayerUnitMoveController;
            foreach (var interfaceMember in this.PlayerSkillControllers)
            {
                this.myPlayerUnitFeatureInterfaceGroup.IPlayerSkillControllers.Add(interfaceMember.Key, interfaceMember.Value);
            }

            this.myPlayerUnitManagerData = new PlayerUnitManagerData(this.UniqueID, this.myPlayerUnitStaticData, this.myPlayerUnitDynamicData, this.myPlayerUnitFeatureInterfaceGroup, this.transform);
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


        // Player의 Unit의 현재 위치를 기준으로, 전달받은 '목표' 위치'까지 A* 기반 이동 수행.
        // '목표 위치'는 '현재 위치'로 부터 이동 가능한 거리값 안에 존재함.
        public void OperateMove(int uniqueID, Vector2Int targetPosition)
        {
            if (this.UniqueID != uniqueID) return;

            this.StopAllCoroutines();
            this.StartCoroutine(this.PlayerUnitMoveController.OperateMove_Coroutine(targetPosition));


            // 다 끝나면, Player Unit Action UIUX 갱신 및 상호작용 가능 명시.
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            var PlayerUnitActionUIUXHandler = HandlerManager.GetDynamicDataHandler<UIUXSystem.UIUXSystemHandler>().PlayerUnitActionUIUXHandler;

            // Player Cost 값에 따른 PlayerUnitActionUIUXHandler 업데이트하라는 코드. 
            PlayerUnitActionUIUXHandler.IPlayerUnitActionPanelUIMediator.Update_PlayerUnitIconUIUX();
            PlayerUnitActionUIUXHandler.IPlayerUnitActionPanelUIMediator.Update_PlayerUnit_ActionableState();
            // Player Action UIUX 상호작용 가능 명시.
            PlayerUnitActionUIUXHandler.IsInteractived = true;
        }

        // Target 된 지점을 기준으로 Player의 Skill을 동작합니다.
        public void OperateSkill(int uniqueID, int skillID, Vector2Int targetedPosition)
        {
            if (this.UniqueID != uniqueID) return;
            if (!this.PlayerSkillControllers.TryGetValue(skillID, out var playerSkillController)) return;

            this.StopAllCoroutines();
            this.StartCoroutine(playerSkillController.OperateSkill_Coroutine(targetedPosition));


            // 다 끝나면, Player Unit Action UIUX 갱신 및 상호작용 가능 명시.
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            var PlayerUnitActionUIUXHandler = HandlerManager.GetDynamicDataHandler<UIUXSystem.UIUXSystemHandler>().PlayerUnitActionUIUXHandler;

            // Player Cost 값에 따른 PlayerUnitActionUIUXHandler 업데이트하라는 코드. 
            PlayerUnitActionUIUXHandler.IPlayerUnitActionPanelUIMediator.Update_PlayerUnitIconUIUX();
            PlayerUnitActionUIUXHandler.IPlayerUnitActionPanelUIMediator.Update_PlayerUnit_ActionableState();
            // Player Action UIUX 상호작용 가능 명시.
            PlayerUnitActionUIUXHandler.IsInteractived = true;
        }
        // Player 죽는 작업?
        public void OperateDie()
        {
            Destroy(this.gameObject);
        }

        public void OperateTurnEndSetting()
        {
            // 이곳에서 이동 코스트 및 스킬 코스트를 초기화.
            this.myPlayerUnitDynamicData.BehaviourCost_Current = this.myPlayerUnitStaticData.BehaviourCost_Default;
        }
    }
}
