using UnityEngine;

using Foundations.Architecture.EventObserver;

namespace GameSystems.PlayerSystem.PlayerUnitSystem
{
    public class PlayerUnitManagerEventListener : MonoBehaviour, IEventObserverListener
    {
        private IEventObserverLinker EventObserverLinker;

        [SerializeField] private PlayerUnitManager PlayerUnitManager;

        private void Awake()
        {
            this.EventObserverLinker = new EventObserverLinker(this);
        }

        private void OnEnable()
        {
            this.EventObserverLinker.RegisterSubscriberListener<UpdatePlayerUnitVisibleRange>();
            this.EventObserverLinker.RegisterSubscriberListener<MoveToTargetPosition_PlayerUnit>();

            this.EventObserverLinker.RegisterSubscriberListener<UpdateMoveableRange_PlayerUnit>();

            this.EventObserverLinker.RegisterSubscriberListener<UpdateSkillTargetingRange_PlayerUnit>();
            this.EventObserverLinker.RegisterSubscriberListener<UpdateSkillImpactRange_PlayerUnit>();

            this.EventObserverLinker.RegisterSubscriberListener<OperateSkill_PlayerUnit>();
        }

        private void OnDisable()
        {
            this.EventObserverLinker.Dispose();
        }

        public void UpdateData(IEventData eventData)
        {
            switch (eventData)
            {
                case UpdatePlayerUnitVisibleRange:
                    this.PlayerUnitManager.UpdateVisibleRange();
                    break;
                case MoveToTargetPosition_PlayerUnit:
                    var data01 = (MoveToTargetPosition_PlayerUnit)eventData;
                    this.PlayerUnitManager.OperatePlayerUnitMove(data01.PlayerUniqueID, data01.TargetPosition);
                    break;
                case UpdateMoveableRange_PlayerUnit:
                    var data02 = (UpdateMoveableRange_PlayerUnit)eventData;
                    this.PlayerUnitManager.UpdateMoveableRange(data02.PlayerUniqueID);
                    break;
                case UpdateSkillTargetingRange_PlayerUnit:
                    var data03 = (UpdateSkillTargetingRange_PlayerUnit)eventData;
                    this.PlayerUnitManager.UpdateSkillTargetingRange(data03.PlayerUniqueID, data03.SkillID);
                    break;
                case UpdateSkillImpactRange_PlayerUnit:
                    var data04 = (UpdateSkillImpactRange_PlayerUnit)eventData;
                    this.PlayerUnitManager.UpdateSkillImpactRange(data04.PlayerUniqueID, data04.SkillID, data04.TargetedPosition);
                    break;
                case OperateSkill_PlayerUnit:
                    var data05 = (OperateSkill_PlayerUnit)eventData;
                    this.PlayerUnitManager.OperateSkill(data05.PlayerUniqueID, data05.SkillID, data05.TargetedPosition);
                    break;
                default:
                    break;
            }
        }
    }
}
