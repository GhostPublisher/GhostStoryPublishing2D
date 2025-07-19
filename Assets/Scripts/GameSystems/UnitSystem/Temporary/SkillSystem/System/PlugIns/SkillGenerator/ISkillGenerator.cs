using UnityEngine;

namespace GameSystems.UnitSystem
{
    public interface ISkillGenerator
    {
        public void InitialSetting();
        public void GenerateSkill(Vector2Int currentPosition, Vector2Int targetPosition);
    }
}