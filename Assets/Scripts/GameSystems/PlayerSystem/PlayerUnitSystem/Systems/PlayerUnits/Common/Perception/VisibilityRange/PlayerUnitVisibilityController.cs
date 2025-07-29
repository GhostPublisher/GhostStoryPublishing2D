using UnityEngine;

using Foundations.Architecture.EventObserver;
using Foundations.Architecture.ReferencesHandler;
using GameSystems.UtilitySystem;

using GameSystems.TilemapSystem;
using GameSystems.TilemapSystem.FogTilemap;
using GameSystems.EnemySystem.EnemyVisibilitySystem;

namespace GameSystems.PlayerSystem.PlayerUnitSystem
{

    public class PlayerUnitVisibilityController : MonoBehaviour, IPlayerUnitVisibilityController
    {
        private IEventObserverNotifier EventObserverNotifier;

        private IFogTilemapController IFogTilemapController;

        private VisibilityRangeCalculator VisibilityRangeCalculator;

        private PlayerUnitManagerData myPlayerUnitManagerData;

        private void Awake()
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            this.IFogTilemapController = HandlerManager.GetDynamicDataHandler<TilemapSystemHandler>().IFogTilemapController;

            this.VisibilityRangeCalculator = HandlerManager.GetUtilityHandler<VisibilityRangeCalculator>();

            this.EventObserverNotifier = new EventObserverNotifier();
        }

        public void InitialSetting(PlayerUnitManagerData playerUnitManagerData)
        {
            this.myPlayerUnitManagerData = playerUnitManagerData;
        }

        private void OnDestroy()
        {
            IFogTilemapController.RemovePlayerVisibleData_ForFogVisibility(this.myPlayerUnitManagerData.UniqueID);

            RemovePlayerVisibleData_ForEnemyVisibility removePlayerVisibleData_ForEnemyVisibility = new();
            removePlayerVisibleData_ForEnemyVisibility.PlayerUniqueID = this.myPlayerUnitManagerData.UniqueID;

            this.EventObserverNotifier.NotifyEvent(removePlayerVisibleData_ForEnemyVisibility);
        }

        public void UpdateVisibleRange()
        {
            this.UpdateVisibleRange(this.myPlayerUnitManagerData.PlayerUnitGridPosition());
        }
        public void UpdateVisibleRange(Vector2Int targetPosition)
        {
            if (this.VisibilityRangeCalculator == null) return;

            var updatedVisibleRange = this.VisibilityRangeCalculator.GetFilteredVisibleRange_Player(targetPosition,
                this.myPlayerUnitManagerData.PlayerUnitStaticData.VisibleSize, this.myPlayerUnitManagerData.PlayerUnitStaticData.VisibleOvercomeWeight);

            IFogTilemapController.UpdatePlayerVisibleData_ForFogVisibility(this.myPlayerUnitManagerData.UniqueID, updatedVisibleRange);
            IFogTilemapController.UpdateFogVisibility();

            UpdatePlayerVisibleData_ForEnemyVisibility UpdatePlayerVisibleData_ForEnemyVisibility = new();
            UpdatePlayerVisibleData_ForEnemyVisibility.PlayerUniqueID = this.myPlayerUnitManagerData.UniqueID;
            UpdatePlayerVisibleData_ForEnemyVisibility.VisibleRange = updatedVisibleRange;
            UpdatePlayerVisibleData_ForEnemyVisibility.PhysicalVisionOvercomeWeight = this.myPlayerUnitManagerData.PlayerUnitStaticData.PhysicalVisionOvercomeWeight;
            UpdatePlayerVisibleData_ForEnemyVisibility.SpiritualVisionOvercomeWeight = this.myPlayerUnitManagerData.PlayerUnitStaticData.SpiritualVisionOvercomeWeight;

            this.EventObserverNotifier.NotifyEvent(UpdatePlayerVisibleData_ForEnemyVisibility);
        }
    }
}