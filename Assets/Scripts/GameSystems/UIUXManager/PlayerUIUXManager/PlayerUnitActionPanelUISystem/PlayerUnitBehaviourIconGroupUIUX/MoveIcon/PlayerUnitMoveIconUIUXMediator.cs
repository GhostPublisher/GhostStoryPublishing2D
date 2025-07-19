using UnityEngine;

namespace GameSystems.UIUXSystem
{
    public class PlayerUnitMoveIconUIUXMediator : MonoBehaviour
    {
        [SerializeField] private PlayerUnitMoveIconUIUXViewController PlayerUnitMoveIconUIUXViewController;

        public void InitialSetting(int uniqueID)
        {
            this.PlayerUnitMoveIconUIUXViewController.InitialSetting(uniqueID);
        }
    }
}