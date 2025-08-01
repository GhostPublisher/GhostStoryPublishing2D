using System.Collections;

using UnityEngine;

using Foundations.Architecture.ReferencesHandler;

namespace GameSystems.UIUXSystem
{
    public interface IPlayerUnitActionPanelUIMediator
    {
        public IEnumerator Show_PlayerUnitActionPanel();
        public IEnumerator Hide_PlayerUnitActionPanel();

        public void Show_PlayerUnitBehaviourPanel(int playerUniqueID);

        public void Update_PlayerUnitIconUIUX();
        public void Update_PlayerUnit_ActionableState();

        public void Clear_PlayerUnitActionPanel();
    }

    public class PlayerUnitActionPanelUIMediator : MonoBehaviour, IPlayerUnitActionPanelUIMediator
    {
        [SerializeField] private PlayerUnitIconGroupUIUXGenerator PlayerUnitIconGroupUIUXGenerator;
        [SerializeField] private PlayerUnitIconGroupUIUXViewController PlayerUnitIconGroupUIUXViewController;
        [SerializeField] private PlayerUnitActionPanelMouseInteractor PlayerUnitActionPanelMouseInteractor;

        [SerializeField] private PlayerUnitActionUIUXHandler myPlayerUnitActionUIUXHandler;

        private void Awake()
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            var UIUXSystem = HandlerManager.GetDynamicDataHandler<UIUXSystemHandler>();

            this.myPlayerUnitActionUIUXHandler = new();
            this.myPlayerUnitActionUIUXHandler.IPlayerUnitActionPanelUIMediator = this;
            UIUXSystem.PlayerUnitActionUIUXHandler = myPlayerUnitActionUIUXHandler;

            this.PlayerUnitIconGroupUIUXGenerator.InitialSetting(this.myPlayerUnitActionUIUXHandler);
            this.PlayerUnitIconGroupUIUXViewController.InitialSetting(this.myPlayerUnitActionUIUXHandler);
            this.PlayerUnitActionPanelMouseInteractor.InitialSetting(this.myPlayerUnitActionUIUXHandler);
        }

        // Player Turn ����
        // Ȥ�� Ư�� �̺�Ʈ�� ���� HIde�� UIUX�� ������ϴ� �뵵.
        public IEnumerator Show_PlayerUnitActionPanel()
        {
            // �����Ǿ� �ִ� Player Unit �������� �����ؼ� PlayerUnit Icon UIUX ����.
            this.PlayerUnitIconGroupUIUXGenerator.UpdateGeneratedPlayerUnitIconUIUX();

            this.PlayerUnitActionPanelMouseInteractor.enabled = false;

            yield return this.StartCoroutine(this.PlayerUnitIconGroupUIUXViewController.Show_UIUX_Coroutine());

            // ��ȣ�ۿ� ���� ���
            this.myPlayerUnitActionUIUXHandler.IsInteractived = true;
        }
        public void Update_PlayerUnitIconUIUX()
        {
            // �����Ǿ� �ִ� Player Unit �������� �����ؼ� PlayerUnit Icon UIUX ����.
            this.PlayerUnitIconGroupUIUXGenerator.UpdateGeneratedPlayerUnitIconUIUX();
        }

        public IEnumerator Hide_PlayerUnitActionPanel()
        {
            this.PlayerUnitActionPanelMouseInteractor.enabled = false;
            // ��ȣ�ۿ� �Ұ� ���
            this.myPlayerUnitActionUIUXHandler.IsInteractived = false;

            yield return this.StartCoroutine(this.PlayerUnitIconGroupUIUXViewController.HIde_UIUX_Coroutine());
        }

        // �ܺ� ��ɿ��� �����ϴ� UIUX ��� ��û.
        // �ڷ�ƾ ��� ����.
        public void Show_PlayerUnitBehaviourPanel(int playerUniqueID)
        {
            // ��� ����.
            this.PlayerUnitIconGroupUIUXViewController.TogglePlayerBehaviourUIUX(playerUniqueID);

            // ��ȣ�ۿ� ���� ���
            this.myPlayerUnitActionUIUXHandler.IsInteractived = true;
            // �ִϸ��̼� ���.
            this.PlayerUnitIconGroupUIUXViewController.Show_BehaviourUIUX();
            // �ش� Panel UIUX ��� ��� Ȱ��ȭ.
            this.PlayerUnitActionPanelMouseInteractor.ActivateMouseInteractor();
        }

        // Player Unit�� Move Cost�� Skill Cost�� ���� Actionable�� ���� ǥ�� ���� �ڵ��Դϴ�.
        public void Update_PlayerUnit_ActionableState()
        {
            foreach(var playerUnitIconGroupUIUXData in this.myPlayerUnitActionUIUXHandler.PlayerUnitIconGroupUIUXDatas.Values)
            {
                playerUnitIconGroupUIUXData.IPlayerUnitIconUIUXViewController.Update_UnitActionableState();
                playerUnitIconGroupUIUXData.IPlayerUnitMoveIconUIUXViewController.Update_MoveActionableState();

                foreach(var playerUnitSkillIconUIUXViewController in playerUnitIconGroupUIUXData.IPlayerUnitSkillIconUIUXViewControllers.Values)
                {
                    playerUnitSkillIconUIUXViewController.Update_SkillActionableState();
                }
            }
        }


        // ������ UIUX�� �����ϰ� Hide �ִϸ��̼��� ����. ( ��������� Test�뵵 ����� ���� ���� )
        public void Clear_PlayerUnitActionPanel()
        {
            this.PlayerUnitIconGroupUIUXGenerator.ClearPlayerUnitIconGroupUIUXAndData();

            this.myPlayerUnitActionUIUXHandler.IsInteractived = false;
            this.PlayerUnitActionPanelMouseInteractor.enabled = false;

            this.StartCoroutine(this.PlayerUnitIconGroupUIUXViewController.HIde_UIUX_Coroutine());
        }
    }

    // PlayerUnitActionPanelMouseInteractor
    // - '���콺 ��Ŭ��'�� ����, '���� Ŭ���� ���� ���' ����� �����մϴ�.
    // - 'PlayerUntiActionPanel UI�� 'Show_All' �� ���¿����� �۵��մϴ�.
    // - 'Ŭ���� ������Ʈ�� Ȱ��/��Ȱ��ȭ' �Ͽ�, �ٸ� ���콺 ���� Ŭ������ '���콺 ��Ŭ��'�� �浹���� �ʵ��� �����մϴ�.
}