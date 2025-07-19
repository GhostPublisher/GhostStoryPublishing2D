using System.Collections.Generic;
using UnityEngine;

using Foundations.Architecture.ReferencesHandler;
using GameSystems.TerrainSystem;


namespace GameSystems.UnitSystem
{
    public class ObjectPresenceFilter_Tile : IUtilityReferenceHandler, IObjectPresenceFilter
    {
        private GeneratedTerrainDataDBHandler GeneratedTileDataGroupHandler;

        public ObjectPresenceFilter_Tile()
        {
            var lazyReferenceHandlerManager = LazyReferenceHandlerManager.Instance;

            this.GeneratedTileDataGroupHandler = lazyReferenceHandlerManager.GetDynamicDataHandler<GeneratedTerrainDataDBHandler>();
        }

        // Tile 유닛이 있는 장소만 리턴.
        public bool TryGetPositionsContainingObject(HashSet<Vector2Int> range, out HashSet<Vector2Int> containUnitRange)
        {
            containUnitRange = new();
            bool anyFound = false;

            // Ally 유닛 제외.
            foreach (Vector2Int pos in range)
            {
                // 관련 유닛이 위치하였다면, 위치값 add.
                if (this.GeneratedTileDataGroupHandler.TryGetGeneratedTerrainData(pos, out var data))
                {
                    containUnitRange.Add(pos);
                    anyFound = true;
                }
            }

            return anyFound;
        }

        // Tile 유닛이 없는 장소만 리턴.
        public bool TryGetPositionsExcludingObject(HashSet<Vector2Int> range, out HashSet<Vector2Int> nonContainUnitRange)
        {
            nonContainUnitRange = new();
            bool anyFound = false;

            foreach (Vector2Int pos in range)
            {
                // 관련 유닛이 위치하지 않았다면, 위치값 add.
                if (!this.GeneratedTileDataGroupHandler.TryGetGeneratedTerrainData(pos, out var data))
                {
                    nonContainUnitRange.Add(pos);
                    anyFound = true;
                }
            }

            return anyFound;
        }
    }
}