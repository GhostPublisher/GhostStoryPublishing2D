using System;
using System.Collections.Generic;

using UnityEngine;

namespace GameSystems.UnitSystem.SkillSystem
{
    [Serializable]
    [CreateAssetMenu(menuName = "ScriptableObject/Skill/SkillResourceDataGroup", fileName = "SkillResourceDataGroup")]
    public class SkillResourceDataGroup : ScriptableObject
    {
        [SerializeField] private List<SkillResourceData> SkillResourceDatas;

        public bool TryGetSkillResourceData(int skillID, out SkillResourceData skillResourceData)
        {
            skillResourceData = null;
            if (this.SkillResourceDatas.Count == 0) return false;

            foreach (var data in this.SkillResourceDatas)
            {
                if(data.SkillID == skillID)
                {
                    skillResourceData = data;
                    return true;
                }
            }

            return false;
        }
    }

    [Serializable]
    public class SkillResourceData
    {
        public int SkillID;
        public Sprite SkillImage;
    }
}