using System;
using System.Collections.Generic;

using UnityEngine;

namespace GameSystems.UnitSystem
{
    [Serializable]
    [CreateAssetMenu(menuName = "ScriptableObject/UnitSkillLink/UnitSkillLinkDataGroup", fileName = "UnitSkillLinkDataGroup")]
    public class UnitSkillLinkDataGroup : ScriptableObject
    {
        [SerializeField] private List<UnitSkillLinkData> UnitSkillLinkDatas;

        public bool TryGetUnitSkillLinkData(int unitID, out UnitSkillLinkData unitSkillLinkData)
        {
            unitSkillLinkData = null;
            if (this.UnitSkillLinkDatas.Count == 0) return false;

            foreach(var linkData in this.UnitSkillLinkDatas)
            {
                if(linkData.UnitID == unitID)
                {
                    unitSkillLinkData = linkData;
                    return true;
                }
            }

            return false;
        }
    }

    [Serializable]
    public class UnitSkillLinkData
    {
        public int UnitID;
        public List<int> SkillIDs;
    }
}