using UnityEngine;

using Foundations.Architecture.EventObserver;
using Foundations.Architecture.ReferencesHandler;
using GameSystems.UtilitySystem;

using GameSystems.TilemapSystem.FogTilemap;
using GameSystems.EnemySystem.EnemyVisibilitySystem;

namespace GameSystems.PlayerSystem.PlayerUnitSystem
{

    public class PlayerUnitVisibilityController : MonoBehaviour, IPlayerUnitVisibilityController
    {
        private IEventObserverNotifier EventObserverNotifier;

        private VisibilityRangeCalculator VisibilityRangeCalculator;

        private PlayerUnitManagerData playerUnitManagerData;

        private void Awake()
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            this.VisibilityRangeCalculator = HandlerManager.GetUtilityHandler<VisibilityRangeCalculator>();

            this.EventObserverNotifier = new EventObserverNotifier();
        }

        public void InitialSetting(PlayerUnitManagerData playerUnitManagerData)
        {
            this.playerUnitManagerData = playerUnitManagerData;
        }

        private void OnDestroy()
        {
            RemovePlayerVisibleData_ForFogVisibility removePlayerVisibleData_ForFogVisibility = new();
            removePlayerVisibleData_ForFogVisibility.PlayerUniqueID = this.playerUnitManagerData.UniqueID;

            RemovePlayerVisibleData_ForEnemyVisibility removePlayerVisibleData_ForEnemyVisibility = new();
            removePlayerVisibleData_ForEnemyVisibility.PlayerUniqueID = this.playerUnitManagerData.UniqueID;

            this.EventObserverNotifier.NotifyEvent(removePlayerVisibleData_ForFogVisibility);
            this.EventObserverNotifier.NotifyEvent(removePlayerVisibleData_ForEnemyVisibility);
        }

        public void UpdateVisibleRange(Vector2Int targetPosition)
        {
            if (this.VisibilityRangeCalculator == null) return;

            var updatedVisibleRange = this.VisibilityRangeCalculator.GetFilteredVisibleRange_Player(targetPosition,
                this.playerUnitManagerData.PlayerUnitStaticData.VisibleSize, this.playerUnitManagerData.PlayerUnitStaticData.VisibleOvercomeWeight);

            UpdatePlayerVisibleData_ForFogVisibility UpdatePlayerVisibleData_ForFogVisibility = new();
            UpdatePlayerVisibleData_ForFogVisibility.PlayerUniqueID = this.playerUnitManagerData.UniqueID;
            UpdatePlayerVisibleData_ForFogVisibility.VisibleRange = updatedVisibleRange;

            UpdatePlayerVisibleData_ForEnemyVisibility UpdatePlayerVisibleData_ForEnemyVisibility = new();
            UpdatePlayerVisibleData_ForEnemyVisibility.PlayerUniqueID = this.playerUnitManagerData.UniqueID;
            UpdatePlayerVisibleData_ForEnemyVisibility.VisibleRange = updatedVisibleRange;
            UpdatePlayerVisibleData_ForEnemyVisibility.PhysicalVisionOvercomeWeight = this.playerUnitManagerData.PlayerUnitStaticData.PhysicalVisionOvercomeWeight;
            UpdatePlayerVisibleData_ForEnemyVisibility.SpiritualVisionOvercomeWeight = this.playerUnitManagerData.PlayerUnitStaticData.SpiritualVisionOvercomeWeight;

            this.EventObserverNotifier.NotifyEvent(UpdatePlayerVisibleData_ForFogVisibility);
            this.EventObserverNotifier.NotifyEvent(UpdatePlayerVisibleData_ForEnemyVisibility);
        }
    }
}