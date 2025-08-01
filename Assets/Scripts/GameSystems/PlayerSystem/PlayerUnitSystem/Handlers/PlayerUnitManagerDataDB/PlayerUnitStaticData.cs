using System;
using System.Collections.Generic;

using UnityEngine;

using GameSystems.UnitSystem;

namespace GameSystems.PlayerSystem.PlayerUnitSystem
{
    [Serializable]
    public class PlayerUnitStaticData
    {
        [SerializeField] public int UniqueID;

        // 유닛 ID
        [SerializeField] public int UnitID;

        // 시야 관련 데이터.
        // Grid 격자모형 Diamond 확장 형태의 확장 크기. ( 시야 확장 크기 )
        [SerializeField] public int VisibleSize;
        // 시야 극복 가중치.
        [SerializeField] public int VisibleOvercomeWeight;
        // 물리 유닛 인식 가중치.
        [SerializeField] public int PhysicalVisionOvercomeWeight;
        // 영적 유닛 인식 가중치.
        [SerializeField] public int SpiritualVisionOvercomeWeight;

        // 이동 관련 데이터.
        // Ground 이동 극복 가중치.
        [SerializeField] public int GroundOvercomeWeight;

        // 시야 차단 가중치.
        [SerializeField] public int VisibleBlockWeight;
        // 물리적 엄폐? 차단 가중치. ( 물리적으로 감지되는 가중치 )
        [SerializeField] public int PhysicalVisionBlockWeight;
        // 영적 엄폐? 차단 가중치. ( 영적으로 감지되는 가중치 )
        [SerializeField] public int SpiritualVisionBlockWeight;


//        [SerializeField] public int CommandBlockWeight;          // 명령 차단 가중치
        // 공격 차단 가중치 ( 공격 가능 여부가 아닌, 해당 유닛 뒤에 존재하는 유닛을 공격할 수 있는지에 대한 가중치 )
        [SerializeField] public int SkillBlockWeight;            


        // 유닛 HP.
        [SerializeField] public int HPCost_Default;

        [SerializeField] public int BehaviourCost_Default;

        [SerializeField] public int MoveCost;

        [SerializeField] public List<SkillCostData> SkillCostDatas;

        public int GetSkillCost(int skillID) { return this.SkillCostDatas.Find(data => data.SkillID == skillID).SkillCost; }
    }

    [Serializable]
    public class SkillCostData
    {
        public int SkillID;
        public int SkillCost;
    }
}