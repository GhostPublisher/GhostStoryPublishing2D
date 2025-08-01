using UnityEngine;
using UnityEngine.UI;

using Foundations.Architecture.ReferencesHandler;

using GameSystems.UnitSystem.SkillSystem;

namespace GameSystems.UIUXSystem
{
    public class PlayerUnitSkillIconUIUXContentSetter : MonoBehaviour
    {
        private SkillResourceDataDBHandler SkillResourceDataDBHandler;

        [SerializeField] private Image UnitIconImageSlot;

        private int mySkillID;

        public void InitialSetting(int skillID)
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            this.SkillResourceDataDBHandler = HandlerManager.GetStaticDataHandler<SkillResourceDataDBHandler>();

            this.mySkillID = skillID;
        }

        public void SetBaseImage()
        {
            if (!this.SkillResourceDataDBHandler.TryGetSkillResourceDataGroup(this.mySkillID, out var data)) return;

            this.UnitIconImageSlot.sprite = data.SkillImage;
        }
    }
}