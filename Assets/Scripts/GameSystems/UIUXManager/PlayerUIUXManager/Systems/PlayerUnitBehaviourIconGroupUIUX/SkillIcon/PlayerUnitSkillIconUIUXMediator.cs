using UnityEngine;

namespace GameSystems.UIUXSystem
{
    public class PlayerUnitSkillIconUIUXMediator : MonoBehaviour
    {
        [SerializeField] private PlayerUnitSkillIconUIUXContentSetter PlayerUnitSkillIconUIUXContentSetter;
        [SerializeField] private PlayerUnitSkillIconUIUXViewController PlayerUnitSkillIconUIUXViewController;

        public void InitialSetting(PlayerUnitActionUIUXHandler playerUnitActionUIUXHandler, int uniqueID, int skillID)
        {
            this.PlayerUnitSkillIconUIUXContentSetter.InitialSetting(skillID);
            this.PlayerUnitSkillIconUIUXContentSetter.SetBaseImage();

            this.PlayerUnitSkillIconUIUXViewController.InitialSetting(playerUnitActionUIUXHandler, uniqueID, skillID);
        }
    }
}