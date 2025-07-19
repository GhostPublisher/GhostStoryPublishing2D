using System.Collections.Generic;
using UnityEngine;

using Foundations.Architecture.ReferencesHandler;
using GameSystems.PlayerSystem;


namespace GameSystems.UnitSystem
{
    public class ObjectPresenceFilter_Player : IUtilityReferenceHandler, IObjectPresenceFilter
    {
        private PlayerUnitManagerDataDBHandler PlayerUnitManagerDataDBHandler;

        public ObjectPresenceFilter_Player()
        {
            var lazyReferenceHandlerManager = LazyReferenceHandlerManager.Instance;

            this.PlayerUnitManagerDataDBHandler = lazyReferenceHandlerManager.GetDynamicDataHandler<PlayerUnitManagerDataDBHandler>();
        }

        // Player 유닛이 있는 장소만 리턴.
        public bool TryGetPositionsContainingObject(HashSet<Vector2Int> range, out HashSet<Vector2Int> excludedRange)
        {
            excludedRange = new();
            bool hasObstacle = false;

            // Ally 유닛 제외.
            foreach (Vector2Int pos in range)
            {
                if (this.PlayerUnitManagerDataDBHandler.TryGetPlayerUnitManagerData(pos, out var data))
                {
                    excludedRange.Add(pos);
                    hasObstacle = true;
                }
            }

            return hasObstacle;
        }

        // Player 유닛이 없는 장소만 리턴.
        public bool TryGetPositionsExcludingObject(HashSet<Vector2Int> range, out HashSet<Vector2Int> nonContainUnitRange)
        {
            nonContainUnitRange = new();
            bool anyFound = false;

            foreach (Vector2Int pos in range)
            {
                // 관련 유닛이 위치하지 않았다면, 위치값 add.
                if (!this.PlayerUnitManagerDataDBHandler.TryGetPlayerUnitManagerData(pos, out var data))
                {
                    nonContainUnitRange.Add(pos);
                    anyFound = true;
                }
            }

            return anyFound;
        }
    }
}