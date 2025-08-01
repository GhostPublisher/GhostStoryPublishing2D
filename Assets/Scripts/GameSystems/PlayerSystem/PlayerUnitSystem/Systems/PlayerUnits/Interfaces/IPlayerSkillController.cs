using System.Collections;

using UnityEngine;

namespace GameSystems.PlayerSystem.PlayerUnitSystem
{
    public interface IPlayerSkillController
    {
        public int SkillID { get; }

        public void InitialSetting(PlayerUnitManagerData playerUnitManagerData);

        // 단일 선택 공격.
        public IEnumerator OperateSkill_Coroutine(Vector2Int targetedPosition);

        // 다중 선택 공격의 경우
        // , Skill Tilemap에서 Mouse 클릭 지정부터 추가 or 수정해야 할 듯 ㅠㅠ.
        // 스킬에 지정 개수 멤버 넣어 선택 횟수 제한하고, Hashset<vector2Int>로 넘기면 될듯.
        // 스킬 작동 부분은 Hashset 반복.
    }
}
