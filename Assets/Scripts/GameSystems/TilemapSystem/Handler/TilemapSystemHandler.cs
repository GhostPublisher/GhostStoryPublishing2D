using Foundations.Architecture.ReferencesHandler;

using GameSystems.TilemapSystem.FogTilemap;

namespace GameSystems.TilemapSystem
{
    public class TilemapSystemHandler : IDynamicReferenceHandler
    {
        public IFogTilemapController IFogTilemapController;

        public FogTilemapData FogTilemapData;
    }
}

