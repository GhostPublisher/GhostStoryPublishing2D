using System.Collections.Generic;

namespace GameSystems.PlayerSystem.PlayerUnitSystem
{
    public class PlayerUnitFeatureInterfaceGroup
    {
        public int UniqueID;

        public IPlayerUnitManager PlayerUnitManager;
        public IPlayerUnitStatusController PlayerUnitStatusController;


        public IPlayerUnitSpriteRendererController PlayerUnitSpriteRendererController;
        public IPlayerUnitAnimationController PlayerUnitAnimationController;


        public IPlayerUnitHitReactionController PlayerUnitHitReactionController;
        public IPlayerUnitEffectController PlayerUnitEffectController;

        public IPlayerUnitVisibilityController PlayerUnitVisibilityController;
        public IPlayerUnitMoveRangeCalculator IPlayerUnitMoveRangeCalculator;
        public Dictionary<int, IPlayerUnitSkillRangeCalculator> PlayerUnitSkillRangeCalculators = new();

        public IPlayerUnitMoveController IPlayerUnitMoveController;
        public Dictionary<int, IPlayerSkillController> IPlayerSkillControllers = new();
    }
}