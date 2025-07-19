using System.Collections.Generic;

using UnityEngine;

namespace GameSystems.PlayerSystem.PlayerUnitSystem
{
    public interface IPlayerUnitSkillRangeCalculator
    {
        public int SkillID { get; }
        public void InitialSetting(PlayerUnitManagerData playerUnitManagerData);
        public void UpdateSkillTargetingRange();
        public void UpdateSkillImpactRange(Vector2Int targetedPosition);

        public HashSet<Vector2Int> GetSkillAppliedRange(Vector2Int targetedPosition);
    }
}
