
using Foundations.Architecture.ReferencesHandler;

namespace GameSystems.PlayerSystem
{
    public class PlayerSystemHandler : IDynamicReferenceHandler
    {
        public PlayerSpawnSystem.IPlayerUnitSpawnController IPlayerUnitSpawnController;
    }
}