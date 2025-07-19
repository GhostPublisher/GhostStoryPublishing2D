using UnityEngine;

namespace GameSystems.UIUXSystem
{
    public class PlayerUnitIconUIUXMediator : MonoBehaviour
    {
        [SerializeField] private PlayerUnitIconUIUXContentSetter PlayerUnitIconUIUXContentSetter;
        [SerializeField] private PlayerUnitIconUIUXViewController PlayerUnitIconUIUXViewController;

        public void InitialSetting(PlayerUnitIconGroupUIUXData playerUnitIconGroupUIUXData, int unitID)
        {
            this.PlayerUnitIconUIUXContentSetter.InitialSetting(unitID);
            this.PlayerUnitIconUIUXContentSetter.SetBaseImage();

            this.PlayerUnitIconUIUXViewController.InitialSetting(playerUnitIconGroupUIUXData);
        }
    }
}