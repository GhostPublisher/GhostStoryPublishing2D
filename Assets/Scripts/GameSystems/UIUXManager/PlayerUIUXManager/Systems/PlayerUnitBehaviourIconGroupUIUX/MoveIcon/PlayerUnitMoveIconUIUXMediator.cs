using UnityEngine;

namespace GameSystems.UIUXSystem
{
    public class PlayerUnitMoveIconUIUXMediator : MonoBehaviour
    {
        [SerializeField] private PlayerUnitMoveIconUIUXViewController PlayerUnitMoveIconUIUXViewController;

        public void InitialSetting(PlayerUnitActionUIUXHandler playerUnitActionUIUXHandler, int uniqueID)
        {
            this.PlayerUnitMoveIconUIUXViewController.InitialSetting(playerUnitActionUIUXHandler, uniqueID);
        }
    }
}