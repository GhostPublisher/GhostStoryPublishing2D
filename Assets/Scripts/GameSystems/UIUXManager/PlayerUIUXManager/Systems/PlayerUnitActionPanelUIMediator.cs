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

        // Player Turn 시작
        // 혹은 특정 이벤트에 의해 HIde된 UIUX를 재출력하는 용도.
        public IEnumerator Show_PlayerUnitActionPanel()
        {
            // 생성되어 있는 Player Unit 정보들을 참고해서 PlayerUnit Icon UIUX 갱신.
            this.PlayerUnitIconGroupUIUXGenerator.UpdateGeneratedPlayerUnitIconUIUX();

            this.PlayerUnitActionPanelMouseInteractor.enabled = false;

            yield return this.StartCoroutine(this.PlayerUnitIconGroupUIUXViewController.Show_UIUX_Coroutine());

            // 상호작용 가능 명시
            this.myPlayerUnitActionUIUXHandler.IsInteractived = true;
        }
        public void Update_PlayerUnitIconUIUX()
        {
            // 생성되어 있는 Player Unit 정보들을 참고해서 PlayerUnit Icon UIUX 갱신.
            this.PlayerUnitIconGroupUIUXGenerator.UpdateGeneratedPlayerUnitIconUIUX();
        }

        public IEnumerator Hide_PlayerUnitActionPanel()
        {
            this.PlayerUnitActionPanelMouseInteractor.enabled = false;
            // 상호작용 불가 명시
            this.myPlayerUnitActionUIUXHandler.IsInteractived = false;

            yield return this.StartCoroutine(this.PlayerUnitIconGroupUIUXViewController.HIde_UIUX_Coroutine());
        }

        // 외부 기능에서 접근하는 UIUX 출력 요청.
        // 코루틴 대기 없음.
        public void Show_PlayerUnitBehaviourPanel(int playerUniqueID)
        {
            // 토글 수행.
            this.PlayerUnitIconGroupUIUXViewController.TogglePlayerBehaviourUIUX(playerUniqueID);

            // 상호작용 가능 명시
            this.myPlayerUnitActionUIUXHandler.IsInteractived = true;
            // 애니메이션 출력.
            this.PlayerUnitIconGroupUIUXViewController.Show_BehaviourUIUX();
            // 해당 Panel UIUX 취소 기능 활성화.
            this.PlayerUnitActionPanelMouseInteractor.ActivateMouseInteractor();
        }

        // Player Unit의 Move Cost와 Skill Cost에 대한 Actionable에 대한 표시 갱신 코드입니다.
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


        // 생성된 UIUX를 삭제하고 Hide 애니메이션을 수행. ( 현재까지는 Test용도 말고는 쓸일 없음 )
        public void Clear_PlayerUnitActionPanel()
        {
            this.PlayerUnitIconGroupUIUXGenerator.ClearPlayerUnitIconGroupUIUXAndData();

            this.myPlayerUnitActionUIUXHandler.IsInteractived = false;
            this.PlayerUnitActionPanelMouseInteractor.enabled = false;

            this.StartCoroutine(this.PlayerUnitIconGroupUIUXViewController.HIde_UIUX_Coroutine());
        }
    }

    // PlayerUnitActionPanelMouseInteractor
    // - '마우스 우클릭'을 통한, '현재 클릭한 유닛 취소' 기능을 수행합니다.
    // - 'PlayerUntiActionPanel UI가 'Show_All' 된 상태에서만 작동합니다.
    // - '클래스 컴포넌트를 활성/비활성화' 하여, 다른 마우스 조작 클래스의 '마우스 우클릭'과 충돌하지 않도록 관리합니다.
}