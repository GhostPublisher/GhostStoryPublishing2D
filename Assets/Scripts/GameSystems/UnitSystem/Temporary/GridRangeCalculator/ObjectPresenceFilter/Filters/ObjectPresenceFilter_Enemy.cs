using System.Collections.Generic;
using UnityEngine;

using Foundations.Architecture.ReferencesHandler;
using GameSystems.EnemySystem.EnemyUnitSystem;


namespace GameSystems.UnitSystem
{
    public class ObjectPresenceFilter_Enemy : IUtilityReferenceHandler, IObjectPresenceFilter
    {
        private EnemyUnitManagerDataDBHandler GeneratedEnemyUnitDataGroupHandler;

        public ObjectPresenceFilter_Enemy()
        {
            var lazyReferenceHandlerManager = LazyReferenceHandlerManager.Instance;

            this.GeneratedEnemyUnitDataGroupHandler = lazyReferenceHandlerManager.GetDynamicDataHandler<EnemyUnitManagerDataDBHandler>();
        }

        // Enemy 유닛이 있는 장소만 리턴.
        public bool TryGetPositionsContainingObject(HashSet<Vector2Int> range, out HashSet<Vector2Int> containUnitRange)
        {
            containUnitRange = new();
            bool anyFound = false;

            // Ally 유닛 제외.
            foreach (Vector2Int pos in range)
            {
                // 관련 유닛이 위치하였다면, 위치값 add.
                if (this.GeneratedEnemyUnitDataGroupHandler.TryGetEnemyUnitManagerData(pos, out var data))
                {
                    containUnitRange.Add(pos);
                    anyFound = true;
                }
            }

            return anyFound;
        }

        // Enemy 유닛이 없는 장소만 리턴.
        public bool TryGetPositionsExcludingObject(HashSet<Vector2Int> range, out HashSet<Vector2Int> nonContainUnitRange)
        {
            nonContainUnitRange = new();
            bool anyFound = false;

            foreach (Vector2Int pos in range)
            {
                // 관련 유닛이 위치하지 않았다면, 위치값 add.
                if (!this.GeneratedEnemyUnitDataGroupHandler.TryGetEnemyUnitManagerData(pos, out var data))
                {
                    nonContainUnitRange.Add(pos);
                    anyFound = true;
                }
            }

            return anyFound;
        }
    }
}