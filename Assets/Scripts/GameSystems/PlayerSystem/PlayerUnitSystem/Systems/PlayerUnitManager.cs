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
        // ���������� ���.
        [SerializeField] private PlayerUnitStatusController PlayerUnitStatusController;

        [SerializeField] private PlayerUnitSpriteRendererController PlayerUnitSpriteRendererController;
        [SerializeField] private PlayerUnitAnimationController PlayerUnitAnimationController;

        [SerializeField] private PlayerUnitHitReactionController PlayerUnitHitReactionController;
        [SerializeField] private PlayerUnitEffectController PlayerUnitEffectController;

        [SerializeField] private PlayerUnitVisibilityController PlayerUnitVisibilityController;
        [SerializeField] private PlayerUnitMouseInteractor PlayerUnitMouseInteractor;

        // ���������� �ٸ��ų�, ���߿� �Ϲ�ȭ �� ����.
        [SerializeField] private GameObject PlayerUnitInterfaceComponentContainer;
        // ����
        private IPlayerUnitMoveRangeCalculator PlayerUnitMoveRangeCalculator;
        // ��� ( �Ǵ��� ������ )
        private IPlayerUnitMoveController PlayerUnitMoveController;

        [SerializeField] private GameObject PlayerUnitSkillInterfaceComponentContainer;
        // ����
        private Dictionary<int, IPlayerUnitSkillRangeCalculator> PlayerUnitSkillRangeCalculators;
        // ��� ( �Ǵ��� ������ )
        private Dictionary<int, IPlayerSkillController> PlayerSkillControllers;

        private int UniqueID;
        // Player Unit ������ + �������̽� + Transform;
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

            // PlugIn ����.
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

            // �ش� ��ġ�� �þ� �۾� ����.
            this.PlayerUnitVisibilityController.UpdateVisibleRange(this.myPlayerUnitManagerData.PlayerUnitGridPosition());

            // ī�޶� ��Ŀ��.
            // ���� �ִϸ��̼� ���.
            yield return StartCoroutine(this.PlayerUnitAnimationController.PlayAndWaitAnimation(PlayerUnitAnimationType.Spawn));

        }


        // Player�� Unit�� ���� ��ġ�� ��������, ���޹��� '��ǥ' ��ġ'���� A* ��� �̵� ����.
        // '��ǥ ��ġ'�� '���� ��ġ'�� ���� �̵� ������ �Ÿ��� �ȿ� ������.
        public void OperateMove(int uniqueID, Vector2Int targetPosition)
        {
            if (this.UniqueID != uniqueID) return;

            this.StopAllCoroutines();
            this.StartCoroutine(this.PlayerUnitMoveController.OperateMove_Coroutine(targetPosition));


            // �� ������, Player Unit Action UIUX ���� �� ��ȣ�ۿ� ���� ���.
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            var PlayerUnitActionUIUXHandler = HandlerManager.GetDynamicDataHandler<UIUXSystem.UIUXSystemHandler>().PlayerUnitActionUIUXHandler;

            // Player Cost ���� ���� PlayerUnitActionUIUXHandler ������Ʈ�϶�� �ڵ�. 
            PlayerUnitActionUIUXHandler.IPlayerUnitActionPanelUIMediator.Update_PlayerUnitIconUIUX();
            PlayerUnitActionUIUXHandler.IPlayerUnitActionPanelUIMediator.Update_PlayerUnit_ActionableState();
            // Player Action UIUX ��ȣ�ۿ� ���� ���.
            PlayerUnitActionUIUXHandler.IsInteractived = true;
        }

        // Target �� ������ �������� Player�� Skill�� �����մϴ�.
        public void OperateSkill(int uniqueID, int skillID, Vector2Int targetedPosition)
        {
            if (this.UniqueID != uniqueID) return;
            if (!this.PlayerSkillControllers.TryGetValue(skillID, out var playerSkillController)) return;

            this.StopAllCoroutines();
            this.StartCoroutine(playerSkillController.OperateSkill_Coroutine(targetedPosition));


            // �� ������, Player Unit Action UIUX ���� �� ��ȣ�ۿ� ���� ���.
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            var PlayerUnitActionUIUXHandler = HandlerManager.GetDynamicDataHandler<UIUXSystem.UIUXSystemHandler>().PlayerUnitActionUIUXHandler;

            // Player Cost ���� ���� PlayerUnitActionUIUXHandler ������Ʈ�϶�� �ڵ�. 
            PlayerUnitActionUIUXHandler.IPlayerUnitActionPanelUIMediator.Update_PlayerUnitIconUIUX();
            PlayerUnitActionUIUXHandler.IPlayerUnitActionPanelUIMediator.Update_PlayerUnit_ActionableState();
            // Player Action UIUX ��ȣ�ۿ� ���� ���.
            PlayerUnitActionUIUXHandler.IsInteractived = true;
        }
        // Player �״� �۾�?
        public void OperateDie()
        {
            Destroy(this.gameObject);
        }

        public void OperateTurnEndSetting()
        {
            // �̰����� �̵� �ڽ�Ʈ �� ��ų �ڽ�Ʈ�� �ʱ�ȭ.
            this.myPlayerUnitDynamicData.BehaviourCost_Current = this.myPlayerUnitStaticData.BehaviourCost_Default;
        }
    }
}
