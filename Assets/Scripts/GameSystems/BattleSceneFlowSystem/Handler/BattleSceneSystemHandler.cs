using Foundations.Architecture.ReferencesHandler;

namespace GameSystems.BattleSceneSystem
{
    public class BattleSceneSystemHandler : IDynamicReferenceHandler
    {
        public IBattleSceneFlowController IBattleSceneFlowController { get; set; }



        public BattleSceneData BattleSceneData { get; set; }
    }
}