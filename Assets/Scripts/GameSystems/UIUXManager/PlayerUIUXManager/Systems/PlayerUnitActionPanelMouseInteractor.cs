using System;
using UnityEngine;

namespace GameSystems.UIUXSystem
{
    public interface IPlayerUnitActionPanelMouseInteractor
    {
        public void ActivateMouseInteractor();
        public void DIsActivateMouseInteractor();
    }

    public class PlayerUnitActionPanelMouseInteractor : MonoBehaviour, IPlayerUnitActionPanelMouseInteractor
    {
        private PlayerUnitActionUIUXHandler myPlayerUnitActionUIUXHandler;

        public void InitialSetting(PlayerUnitActionUIUXHandler playerUnitActionUIUXHandler)
        {
            this.myPlayerUnitActionUIUXHandler = playerUnitActionUIUXHandler;

            this.myPlayerUnitActionUIUXHandler.IPlayerUnitActionPanelMouseInteractor = this;
        }

        private void Update()
        {
            if (Input.GetMouseButtonUp(1))
            {
                // Half만 보이도록 애니메이션 실행.
                this.myPlayerUnitActionUIUXHandler.IPlayerUnitIconGroupUIUXViewController.Hide_BehaviourUIUX();
                // Update가 작동하지 않도록, 컴포넌트 비활성화.
                this.DIsActivateMouseInteractor();
            }
        }

        public void ActivateMouseInteractor()
        {
            this.enabled = true;
        }
        public void DIsActivateMouseInteractor()
        {
            this.enabled = false;
        }
    }
}