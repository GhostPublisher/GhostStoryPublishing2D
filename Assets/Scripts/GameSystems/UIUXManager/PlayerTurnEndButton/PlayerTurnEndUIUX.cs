using UnityEngine;

using Foundations.Architecture.ReferencesHandler;

namespace GameSystems.UIUXSystem
{
    public class PlayerTurnEndUIUX : MonoBehaviour
    {
        [SerializeField] private GameObject CheckingBox;
        private bool IsActivated = false;

        private void Awake()
        {
            this.CheckingBox.SetActive(false);
        }

        public void ActivateCheckingBox()
        {
            if (!this.IsActivated)
            {
                this.IsActivated = true;
                this.CheckingBox.SetActive(true);
            }
        }
        public void DisActivateCheckingBox()
        {
            if (this.IsActivated)
            {
                this.IsActivated = false;
                this.CheckingBox.SetActive(false);
            }
        }

        public void ConfirmPlayerTurnEnd()
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            var BattleSceneSystemHandler = HandlerManager.GetDynamicDataHandler<BattleSceneSystem.BattleSceneSystemHandler>();

            BattleSceneSystemHandler.IBattleSceneFlowController.OperateBattleSceneFlow_PlayerTurnEndSetting();

            this.IsActivated = false;
            this.CheckingBox.SetActive(false);
        }

        public void CancelPlayerTurnEnd()
        {
            this.IsActivated = false;
            this.CheckingBox.SetActive(false);
        }
    }
}