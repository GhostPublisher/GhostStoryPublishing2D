using UnityEngine;

using Foundations.Architecture.ReferencesHandler;

namespace GameSystems.UnitSystem
{
    public class UnitSkillLinkDataDBHandler : IStaticReferenceHandler
    {
        private UnitSkillLinkDataGroup UnitSkillLinkDataGroup = null;

        public UnitSkillLinkDataDBHandler()
        {
            this.LoadScriptableObject();
        }
        private void LoadScriptableObject()
        {
            this.UnitSkillLinkDataGroup = Resources.Load<UnitSkillLinkDataGroup>("ScriptableObject/UnitSkillLink/UnitSkillLinkDataGroup");
        }

        public bool TryGetUnitSkillLinkData(int unitID, out UnitSkillLinkData unitSkillLinkData)
        {
            unitSkillLinkData = null;
            if (this.UnitSkillLinkDataGroup == null) return false;

            return this.UnitSkillLinkDataGroup.TryGetUnitSkillLinkData(unitID, out unitSkillLinkData);
        }
    }
}
