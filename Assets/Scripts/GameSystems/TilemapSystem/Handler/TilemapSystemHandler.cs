using Foundations.Architecture.ReferencesHandler;

using GameSystems.TilemapSystem.MapVisibilityTilemap;
using GameSystems.TilemapSystem.SkillRangeTilemap;

namespace GameSystems.TilemapSystem
{
    public class TilemapSystemHandler : IDynamicReferenceHandler
    {
        public IMapVisibilityTilemapController IMapVisibilityTilemapController;
        public MapVisibilityTilemapData MapVisibilityTilemapData;

        public ISkillRangeTilemapSystem ISkillRangeTilemapSystem;

    }
}

