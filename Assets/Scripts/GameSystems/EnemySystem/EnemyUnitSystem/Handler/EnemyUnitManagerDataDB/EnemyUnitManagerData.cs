using System;
using System.Collections.Generic;

using UnityEngine;

using GameSystems.UnitSystem;

namespace GameSystems.EnemySystem.EnemyUnitSystem
{
    [Serializable]
    public class EnemyUnitStaticData
    {
        [SerializeField] public int UniqueID;

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

        // 공격 차단 가중치
        [SerializeField] public int SkillBlockWeight;

        // 유닛 HP.
        [SerializeField] public int DefaultHPCost;
//        [SerializeField] public int MaxHPCost;
        // HP 회복량
//        [SerializeField] public int DefaultHPRegenCost;
//        [SerializeField] public int MaxHPRegenCost;

        // 이동 코스트
        [SerializeField] public int DefaultMoveCost;
//        [SerializeField] public int MaxMoveCost;
        // 이동 코스트 회복량.
//        [SerializeField] public int DefaultMoveRegenCost;
//        [SerializeField] public int MaxMoveRegenCost;

        // 스킬 코스트
        [SerializeField] public int DefaultSkillCost;
        //        [SerializeField] public int MaxSkillCost;
        // 스킬 코스트 회복량
        //        [SerializeField] public int DefaultSkillRegenCost;
        //        [SerializeField] public int MaxSkillRegenCost;

        // 어차피 유닛과 스킬이 1 대 1 대응? 이기에 상관없음. ( 그냥 Unit에 Skill Cost를 박아 넣어놔도 상관은 없음 )
        // 그런데 차후에 유닛과 스킬의 Table을 분류하고 싶은 경우,
        // Linker를 통한 Table 연결. or Skill을 Table화 시키지 말고 Interface와 EnemyUnitDynamicData의 역참조를 통해 Cost를 동적으로 연결해 놓으면 되김함.
        // 그런데 이후, 행동판단로직 아래에 '인식-가공' 로직이 들어가면 이거 필요없을듯??
        [SerializeField] public int Skill01ActionCost;
        [SerializeField] public int Skill02ActionCost;
        [SerializeField] public int Skill03ActionCost;
    }

    [Serializable]
    public class EnemyUnitDynamicData
    {
        [SerializeField] public int UniqueID;

        [SerializeField] public int CurrentMoveCost;
        [SerializeField] public int CurrentSkillCost;
        [SerializeField] public int CurrentHPCost;

        private Dictionary<Type, IEnemyUnitRangeData> EnemyUnitRangeDatas;
        private Dictionary<Type, IEnemyUnitCurrentMemoryData> EnemyUnitCurrentDetectedDatas;
        private Dictionary<Type, IEnemyUnitMemoryData> EnemyUnitMemoryDatas;

        private Dictionary<SkillSlotType, ISkillFilteredRangeData> SkillFilteredRangeDatas;
        private Dictionary<SkillSlotType, ISkillValidTargetRangeData> SkillValidTargetRangeDatas;

        [SerializeField] public bool IsOperationOver;

        public EnemyUnitDynamicData()
        {
            this.EnemyUnitRangeDatas = new();
            this.EnemyUnitCurrentDetectedDatas = new();
            this.EnemyUnitMemoryDatas = new();

            this.SkillFilteredRangeDatas = new();
            this.SkillValidTargetRangeDatas = new();
        }

        // Dictionary<Type, IEnemyUnitRangeData>
        public bool TryGetEnemyUnitRangeData<T>(out T returnData) where T : class, IEnemyUnitRangeData
        {
            Type key = typeof(T);

            if (this.EnemyUnitRangeDatas.TryGetValue(key, out var data))
            {
                returnData = data as T;
                return true;
            }

            returnData = null;
            return false;
        }
        // 덮어쓰기.
        public void SetEnemyUnitRangeData<T>(T enemyUnitRangeData) where T : class, IEnemyUnitRangeData
        {
            if (enemyUnitRangeData == null)
                throw new ArgumentNullException(nameof(enemyUnitRangeData));

            this.EnemyUnitRangeDatas[typeof(T)] = enemyUnitRangeData;
        }

        // Dictionary<Type, IEnemyUnitCurrentDetectedData>
        public bool TryGetEnemyUnitCurrentDetectedData<T>(out T returnData) where T : class, IEnemyUnitCurrentMemoryData
        {
            Type key = typeof(T);

            if (this.EnemyUnitCurrentDetectedDatas.TryGetValue(key, out var data))
            {
                returnData = data as T;
                return true;
            }

            returnData = null;
            return false;
        }
        // 덮어쓰기.
        public void SetEnemyUnitCurrentDetectedData<T>(T enemyUnitCurrentDetectedData) where T : class, IEnemyUnitCurrentMemoryData
        {
            if (enemyUnitCurrentDetectedData == null)
                throw new ArgumentNullException(nameof(enemyUnitCurrentDetectedData));

            this.EnemyUnitCurrentDetectedDatas[typeof(T)] = enemyUnitCurrentDetectedData;
        }

        // Dictionary<Type, IEnemyUnitMemoryData>
        public bool TryGetEnemyUnitMemoryData<T>(out T returnData) where T : class, IEnemyUnitCurrentMemoryData
        {
            Type key = typeof(T);

            if (this.EnemyUnitMemoryDatas.TryGetValue(key, out var data))
            {
                returnData = data as T;
                return true;
            }

            returnData = null;
            return false;
        }
        // 덮어쓰기.
        public void SetEnemyUnitMemoryData<T>(T enemyUnitMemoryDatas) where T : class, IEnemyUnitMemoryData
        {
            if (enemyUnitMemoryDatas == null)
                throw new ArgumentNullException(nameof(enemyUnitMemoryDatas));

            this.EnemyUnitMemoryDatas[typeof(T)] = enemyUnitMemoryDatas;
        }

        // 만약, Range 범위 형식 개념이 변경되면 이것도 Type 기반으로 변경하자. Type 기반으로 변경하기 전에는 인터페이스 멤버가 동일한 상태.
        // Dictionary<int, ISkillFilteredRange>
        public bool TryGetSkillFilteredRangeData(SkillSlotType skillSlotType, out ISkillFilteredRangeData skillFilteredRangeData)
        {
            if (this.SkillFilteredRangeDatas.TryGetValue(skillSlotType, out var data))
            {
                skillFilteredRangeData = data;
                return true;
            }

            skillFilteredRangeData = null;
            return false;
        }
        // 덮어쓰기.
        public void SetSkillFilteredRangeData(SkillSlotType skillSlotType, ISkillFilteredRangeData skillFilteredRangeData)
        {
            this.SkillFilteredRangeDatas[skillSlotType] = skillFilteredRangeData;
        }


        //  Dictionary<int, ISkillValidTargetRange>
        public bool TryGetSkillValidTargetRangeData(SkillSlotType skillSlotType, out ISkillValidTargetRangeData skillValidTargetRangeData)
        {
            if (this.SkillValidTargetRangeDatas.TryGetValue(skillSlotType, out var data))
            {
                skillValidTargetRangeData = data;
                return true;
            }

            skillValidTargetRangeData = null;
            return false;
        }
        // 덮어쓰기.
        public void SetSkillValidTargetRangeData(SkillSlotType skillSlotType, ISkillValidTargetRangeData skillValidTargetRangeData)
        {
            this.SkillValidTargetRangeDatas[skillSlotType] = skillValidTargetRangeData;
        }
    }

    public interface IEnemyUnitRangeData { }
    public interface IEnemyUnitCurrentMemoryData { }
    public interface IEnemyUnitMemoryData { }

    public interface ISkillFilteredRangeData
    {
        public HashSet<Vector2Int> FilteredRange { get; set; }
    }
    public interface ISkillValidTargetRangeData
    {
        public HashSet<Vector2Int> ValidTargetPositions { get; set; }
    }


    [Serializable]
    public class EnemyUnitFeatureInterfaceGroup
    {
        public int UniqueID;

        public IEnemyUnitManager EnemyUnitManager;
        public IEnemyUnitStatusController EnemyUnitStatusController;

        public IEnemyUnitSpriteRendererController EnemyUnitSpriteRendererController;
        public IEnemyUnitAnimationController EnemyUnitAnimationController;

        public IEnemyHitReactionController EnemyHitReactionController;
        public IEnemyUnitEffectController EnemyUnitEffectController;

        public IEnemyUnitMoveController EnemyUnitMoveController;

        public Dictionary<SkillSlotType, IEnemyUnitSkillRangeDataPreprocessor> EnemyUnitSkillRangeDataPreprocessors = new();
        public Dictionary<SkillSlotType, IEnemyUnitSkillController> EnemyUnitSkillControllers = new();
    }
}