using UnityEngine;

namespace GameSystems.UIUXSystem
{
    public class PlayerUnitSkillIconUIUXMediator : MonoBehaviour
    {
        [SerializeField] private PlayerUnitSkillIconUIUXContentSetter PlayerUnitSkillIconUIUXContentSetter;
        [SerializeField] private PlayerUnitSkillIconUIUXViewController PlayerUnitSkillIconUIUXViewController;

        public void InitialSetting(int uniqueID, int skillID)
        {
            this.PlayerUnitSkillIconUIUXContentSetter.InitialSetting(skillID);
            this.PlayerUnitSkillIconUIUXContentSetter.SetBaseImage();

            this.PlayerUnitSkillIconUIUXViewController.InitialSetting(uniqueID, skillID);
        }
    }
}