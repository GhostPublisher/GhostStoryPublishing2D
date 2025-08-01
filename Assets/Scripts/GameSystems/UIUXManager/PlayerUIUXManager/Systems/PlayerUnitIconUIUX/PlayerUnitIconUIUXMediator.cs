using UnityEngine;

namespace GameSystems.UIUXSystem
{
    public class PlayerUnitIconUIUXMediator : MonoBehaviour
    {
        [SerializeField] private PlayerUnitIconUIUXContentSetter PlayerUnitIconUIUXContentSetter;
        [SerializeField] private PlayerUnitIconUIUXViewController PlayerUnitIconUIUXViewController;

        public void InitialSetting(PlayerUnitActionUIUXHandler playerUnitActionUIUXHandler, int uniqueID, int unitID)
        {
            this.PlayerUnitIconUIUXContentSetter.InitialSetting(unitID);
            this.PlayerUnitIconUIUXContentSetter.SetBaseImage();

            this.PlayerUnitIconUIUXViewController.InitialSetting(playerUnitActionUIUXHandler, uniqueID);
        }
    }
}