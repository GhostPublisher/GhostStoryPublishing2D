using UnityEngine;

using Foundations.Architecture.ReferencesHandler;

namespace GameSystems.UnitSystem.SkillSystem
{
    public class SkillResourceDataDBHandler : IStaticReferenceHandler
    {
        private SkillResourceDataGroup SkillPrefabResourceDataGroup = null;

        public SkillResourceDataDBHandler()
        {
            this.LoadScriptableObject();
        }
        private void LoadScriptableObject()
        {
            this.SkillPrefabResourceDataGroup = Resources.Load<SkillResourceDataGroup>("ScriptableObject/Skill/SkillResourceDataGroup");
        }

        public bool TryGetSkillResourceDataGroup(int skillID, out SkillResourceData skillResourceData)
        {
            skillResourceData = null;
            if (this.SkillPrefabResourceDataGroup == null) return false;

            return this.SkillPrefabResourceDataGroup.TryGetSkillResourceData(skillID, out skillResourceData);
        }
    }
}