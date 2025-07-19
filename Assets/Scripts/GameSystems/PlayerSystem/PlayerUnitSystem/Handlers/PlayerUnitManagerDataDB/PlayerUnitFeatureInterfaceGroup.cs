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

        public Dictionary<int, IPlayerUnitSkillRangeCalculator> PlayerUnitSkillRangeCalculators = new();
    }
}