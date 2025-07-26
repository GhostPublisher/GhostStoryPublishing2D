using System;

using Foundations.Architecture.EventObserver;

namespace GameSystems.BattleSceneSystem
{
    [Serializable]
    public class InitialSetBattleSceneFlowControllerEvent : IEventData
    {
        public int StageID;
    }
}