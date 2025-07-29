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

            // �ش� ��ġ�� �þ� �۾� ����.
            this.PlayerUnitVisibilityController.UpdateVisibleRange(this.myPlayerUnitManagerData.PlayerUnitGridPosition());

            // ī�޶� ��Ŀ��.
            // ���� �ִϸ��̼� ���.
            yield return StartCoroutine(this.PlayerUnitAnimationController.PlayAndWaitAnimation(PlayerUnitAnimationType.Spawn));

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

        // Player Unit�� ���� ��ġ�� ��������, �ֺ� �þ� ������ �����մϴ�.
        public void UpdateVisibleRange()
        {
            this.PlayerUnitVisibilityController.UpdateVisibleRange(this.myPlayerUnitManagerData.PlayerUnitGridPosition());
        }

        // Player Unit�� ���� ��ġ�� ��������, ���� �̵� ���� ������ �����մϴ�.
        public void UpdateMoveableRange(int playerUniqueID)
        {
            if (this.UniqueID != playerUniqueID) return;

            this.PlayerUnitMoveRangeCalculator.UpdateMoveableRange();
        }
        // Player�� Unit�� ���� ��ġ�� ��������, ���޹��� '��ǥ' ��ġ'���� A* ��� �̵� ����.
        // '��ǥ ��ġ'�� '���� ��ġ'�� ���� �̵� ������ �Ÿ��� �ȿ� ������.
        public void OperatePlayerUnitMove(int playerUniqueID, Vector2Int targetPosition)
        {
            if (this.UniqueID != playerUniqueID) return;

            this.PlayerUnitMoveController.OperatePlayerUnitMove(targetPosition);
        }

        // Player�� ���� ��ġ�� ����Ͽ�, Skill�� Targeting ���� ������ ���� ��, Notify �մϴ�.
        public void UpdateSkillTargetingRange(int playerUniqueID, int skillID)
        {
            if(this.UniqueID != playerUniqueID) return;
            if (!this.PlayerUnitSkillRangeCalculators.TryGetValue(skillID, out var playerUnitSkillRangeCalculator)) return;

            playerUnitSkillRangeCalculator.UpdateSkillTargetingRange();
        }
        // Target �� ������ ��������, Skill�� Impact ������ ���� ��, Notify�մϴ�.
        public void UpdateSkillImpactRange(int playerUniqueID, int skillID, Vector2Int targetedPosition)
        {
            if (this.UniqueID != playerUniqueID) return;
            if (!this.PlayerUnitSkillRangeCalculators.TryGetValue(skillID, out var playerUnitSkillRangeCalculator)) return;

            playerUnitSkillRangeCalculator.UpdateSkillImpactRange(targetedPosition);
        }
        // Target �� ������ �������� Player�� Skill�� �����մϴ�.
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
