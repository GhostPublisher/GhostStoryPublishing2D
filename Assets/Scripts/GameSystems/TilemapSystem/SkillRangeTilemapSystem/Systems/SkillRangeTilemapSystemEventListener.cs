
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
                case InitialSetSkillRangeTilemapEvent_Raw:
                    var data01 = (InitialSetSkillRangeTilemapEvent_Raw)eventData;
                    this.SkillRangeTilemapSystem.InitialSetting(data01.Width, data01.Height);
                    break;
                case ActivateFilteredSkillRangeTilemapEvent:
                    var data02 = (ActivateFilteredSkillRangeTilemapEvent)eventData;
                    this.SkillRangeTilemapSystem.ActivateFilteredSkillRangeTilemap(data02.PlayerUniqueID, data02.SkillID, data02.CurrentPosition, data02.FilteredSkillRange, data02.SkillTargetPositions);
                    break;
                case ActiavteSkillImpactRangeTilemap:
                    var data03 = (ActiavteSkillImpactRangeTilemap)eventData;
                    this.SkillRangeTilemapSystem.ActiavteSkillImpactRangeTilemap(data03.MainTargetPosition, data03.FilteredSkillImpactRange, data03.AdditionalTargetPositions);
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