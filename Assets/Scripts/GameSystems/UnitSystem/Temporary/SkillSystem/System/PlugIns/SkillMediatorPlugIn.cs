/*
using UnityEngine;

namespace GameSystems.UnitSystem
{
    public interface ISkillRangeDataProcessor
    {
        public void InitialSetting(GeneratedSkillData generatedSkillData);
        public void UpdateSkillRange(Vector2Int currentPosition);
    }

    public class SkillMediatorPlugIn : MonoBehaviour, ISkillMediatorPlugIn
    {
        [SerializeField] private int SkillRegisterNumber_;
        [SerializeField] private int SkillID;
        [SerializeField] private SkillData SkillData;

        [SerializeField] private GameObject InterfaceContainer;
        private ISkillRangeDataProcessor skillRangeDataProcessor;
        private ISkillGenerator skillGenerator;

        private GeneratedSkillData myGeneratedSkillData;

        public int SkillRegisterNumber => this.SkillRegisterNumber_;

        public void GenerateSkill(Vector2Int currentPosition)
        {
            throw new System.NotImplementedException();
        }

        public void InitialSetting(IGeneratedSkillDataGroup generatedSkillDataGroup)
        {
            this.myGeneratedSkillData = new();
            this.myGeneratedSkillData.SkillID = this.SkillID;
            this.myGeneratedSkillData.SkillData = this.SkillData;
            generatedSkillDataGroup.GeneratedSkillDatas.Add(this.SkillRegisterNumber_, this.myGeneratedSkillData);

            this.skillRangeDataProcessor = this.InterfaceContainer.GetComponent<ISkillRangeDataProcessor>();
            this.skillRangeDataProcessor.InitialSetting(this.myGeneratedSkillData);

            this.skillGenerator = this.InterfaceContainer.GetComponent<ISkillGenerator>();
            this.skillGenerator.InitialSetting();
        }

        public void UpdateSkillRange(Vector2Int currentPosition)
        {
            this.skillRangeDataProcessor.UpdateSkillRange(currentPosition);
        }
    }
}*/