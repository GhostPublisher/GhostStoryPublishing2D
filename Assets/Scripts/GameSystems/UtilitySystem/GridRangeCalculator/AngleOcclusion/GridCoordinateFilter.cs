/*using System.Collections.Generic;
using UnityEngine;

using GameSystems.EnemySystem.EnemyUnitSystem;
using GameSystems.PlayerSystem;
using GameSystems.TerrainSystem;
using Foundations.Architecture.ReferencesHandler;

namespace GameSystems.UtilitySystem
{
    public class GridCoordinateFilter : IUtilityReferenceHandler
    {
        private GeneratedTerrainDataDBHandler GeneratedTerrainDataDBHandler;
        private PlayerUnitManagerDataDBHandler PlayerUnitManagerDataDBHandler;
        private EnemyUnitManagerDataDBHandler GeneratedEnemyUnitDataDBHandler;

        public GridCoordinateFilter()
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;

            this.GeneratedTerrainDataDBHandler = HandlerManager.GetDynamicDataHandler<GeneratedTerrainDataDBHandler>();
            this.PlayerUnitManagerDataDBHandler = HandlerManager.GetDynamicDataHandler<PlayerUnitManagerDataDBHandler>();
            this.GeneratedEnemyUnitDataDBHandler = HandlerManager.GetDynamicDataHandler<EnemyUnitManagerDataDBHandler>();
        }

        public HashSet<Vector2Int> GetEnemyPositions(HashSet<Vector2Int> filteredRange)
        {
            HashSet<Vector2Int> result = new();

            foreach (Vector2Int pos in filteredRange)
            {
                if (!this.GeneratedEnemyUnitDataDBHandler.TryGetEnemyUnitManagerData(pos, out var _)) continue;

                result.Add(pos);
            }

            return result;
        }
    }
}*/