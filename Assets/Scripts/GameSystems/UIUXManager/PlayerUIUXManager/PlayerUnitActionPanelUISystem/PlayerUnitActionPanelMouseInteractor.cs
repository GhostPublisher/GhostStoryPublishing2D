using System;
using UnityEngine;

namespace GameSystems.UIUXSystem
{
    public class PlayerUnitActionPanelMouseInteractor : MonoBehaviour
    {
        private Action DisActivateBehaviourPanelUI;

        public void InitialSetting(Action action)
        {
            this.DisActivateBehaviourPanelUI = action;
        }

        private void Update()
        {
            if (Input.GetMouseButtonUp(1))
            {
                this.DisActivateBehaviourPanelUI?.Invoke();
            }
        }
    }
}