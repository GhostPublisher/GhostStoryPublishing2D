/*using System.Collections.Generic;

using UnityEngine;

namespace GameSystems.UnitSystem
{
    public interface ISkillMediatorPlugIn
    {
        public int SkillRegisterNumber { get; }
        public void InitialSetting(IGeneratedSkillDataGroup generatedSkillDataGroup);
        public void UpdateSkillRange(Vector2Int currentPosition);
        public void GenerateSkill(Vector2Int currentPosition);
    }

    // 각 유닛의 인스펙터에 명시한, 스킬 등록변호를 Key 값으로 사용.
    public class SkillMediatorPlugInHub : MonoBehaviour
    {
        [SerializeField] private GameObject InterfaceContainer;
        private Dictionary<int, ISkillMediatorPlugIn> skillMediatorPlugIns;

        public void InitialSetting(IGeneratedSkillDataGroup generatedSkillDataGroup)
        {
            this.skillMediatorPlugIns = new();

            foreach (var comp in this.InterfaceContainer.GetComponents<MonoBehaviour>())
            {
                if (comp is ISkillMediatorPlugIn plugIn)
                {
                    plugIn.InitialSetting(generatedSkillDataGroup);
                    this.skillMediatorPlugIns[plugIn.SkillRegisterNumber] = plugIn;
                }
            }
        }

        public void UpdateSkillRangeAll(Vector2Int currentPosition)
        {
            foreach (var plugIn in this.skillMediatorPlugIns.Values)
            {
                plugIn.UpdateSkillRange(currentPosition);
            }
        }

        public void UpdateSkillRange(int skillID, Vector2Int currentPosition)
        {
            if (!this.skillMediatorPlugIns.TryGetValue(skillID, out var plugIn))
            {
                Debug.Log($"해당 플러그인 없음");
            }

            plugIn.UpdateSkillRange(currentPosition);
        }

        public void GenerateSkill(int skillID, Vector2Int currentPosition)
        {
            if (!this.skillMediatorPlugIns.TryGetValue(skillID, out var plugIn))
            {
                Debug.Log($"해당 플러그인 없음");
            }

            plugIn.GenerateSkill(currentPosition);
        }
    }
}*/