using System.Collections.Generic;

using UnityEngine;

namespace GameSystems.UnitSystem
{
    // RangeType에 따른 필요 없는 범위 삭제.
    public interface IObjectPresenceFilter
    {
        // 특정 유닛이 포함된 위치값만 리턴.
        public bool TryGetPositionsContainingObject(HashSet<Vector2Int> range, out HashSet<Vector2Int> availableRange);
        // 특정 유닛이 포함되지 않은 위치값만 리턴.

        public bool TryGetPositionsExcludingObject(HashSet<Vector2Int> range, out HashSet<Vector2Int> availableRange);
    }
}