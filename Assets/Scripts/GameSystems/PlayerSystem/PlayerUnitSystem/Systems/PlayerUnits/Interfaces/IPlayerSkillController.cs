using System.Collections.Generic;

using UnityEngine;

namespace GameSystems.PlayerSystem.PlayerUnitSystem
{
    public interface IPlayerSkillController
    {
        public int SkillID { get; }
        public void InitialSetting(PlayerUnitManagerData playerUnitManagerData);

        // 단일 공격.
        public void OperateSkill(Vector2Int targetedPosition);

    }
}
