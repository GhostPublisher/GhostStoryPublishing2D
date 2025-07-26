using UnityEngine;

using Foundations.Architecture.EventObserver;

namespace GameSystems.TilemapSystem.SkillRangeTilemap
{
    public class SkillRangeTilemapSystemEventListener : MonoBehaviour, IEventObserverListener
    {
        private IEventObserverLinker EventObserverLinker;

        [SerializeField] private SkillRangeTilemapSystem SkillRangeTilemapSystem;

        private void Awake()
        {
            this.EventObserverLinker = new EventObserverLinker(this);
        }

        private void OnEnable()
        {
            this.EventObserverLinker.RegisterSubscriberListener<InitialSetSkillRangeTilemapEvent>();
            this.EventObserverLinker.RegisterSubscriberListener<InitialSetSkillRangeTilemapEvent_Raw>();
            this.EventObserverLinker.RegisterSubscriberListener<ActivateFilteredSkillRangeTilemapEvent>();
            this.EventObserverLinker.RegisterSubscriberListener<ActiavteSkillImpactRangeTilemap>();
            this.EventObserverLinker.RegisterSubscriberListener<DisActivateSkillRangeTilemap>();
        }

        private void OnDisable()
        {
            this.EventObserverLinker.Dispose();
        }

        public void UpdateData(IEventData eventData)
        {
            switch (eventData)
            {
                case InitialSetSkillRangeTilemapEvent:
                    var data01 = (InitialSetSkillRangeTilemapEvent)eventData;
                    this.SkillRangeTilemapSystem.InitialSetting(data01.StageID);
                    break;
                case InitialSetSkillRangeTilemapEvent_Raw:
                    var data02 = (InitialSetSkillRangeTilemapEvent_Raw)eventData;
                    this.SkillRangeTilemapSystem.InitialSetting(data02.Width, data02.Height);
                    break;
                case ActivateFilteredSkillRangeTilemapEvent:
                    var data03 = (ActivateFilteredSkillRangeTilemapEvent)eventData;
                    this.SkillRangeTilemapSystem.ActivateFilteredSkillRangeTilemap(data03.PlayerUniqueID, data03.SkillID, data03.CurrentPosition, data03.FilteredSkillRange, data03.SkillTargetPositions);
                    break;
                case ActiavteSkillImpactRangeTilemap:
                    var data04 = (ActiavteSkillImpactRangeTilemap)eventData;
                    this.SkillRangeTilemapSystem.ActiavteSkillImpactRangeTilemap(data04.MainTargetPosition, data04.FilteredSkillImpactRange, data04.AdditionalTargetPositions);
                    break;
                case DisActivateSkillRangeTilemap:
                    this.SkillRangeTilemapSystem.DisActivateSkillRangeTilemap();
                    break;
                default:
                    break;
            }
        }
    }
}