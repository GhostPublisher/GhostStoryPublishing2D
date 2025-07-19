using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using Foundations.Architecture.ReferencesHandler;

namespace GameSystems.UtilitySystem
{
    // 장애물에 의해 가려지는 범위 삭제.
    public interface IMainRangeFilter
    {
        public Dictionary<Vector2Int, (float minAngle, float maxAngle)> GetObstacleAngleRanges(Vector2Int currentPosition, HashSet<Vector2Int> obstacleRange);
        public HashSet<Vector2Int> GetUseableRange(Vector2Int currentPosition, HashSet<Vector2Int> baseRange, Dictionary<Vector2Int, (float minAngle, float maxAngle)> angleMap);
    }

    public class GridAngleOcclusionFilter : IUtilityReferenceHandler, IMainRangeFilter
    {
        /// <summary>
        /// 장애물별로 (장애물좌표, [minAngle, maxAngle])을 사전으로 반환
        /// 포함관계일 때만 좁은 범위는 제거
        /// </summary>
        public Dictionary<Vector2Int, (float minAngle, float maxAngle)> GetObstacleAngleRanges(Vector2Int currentPosition, HashSet<Vector2Int> obstacleRange)
        {
            // 1. 각 장애물에 대한 각도 범위 구하기
            Dictionary<Vector2Int, (float minAngle, float maxAngle)> angleMap = new();

            foreach (var obstacle in obstacleRange)
            {
                GetObstacleAngularRange(currentPosition, obstacle, out float minA, out float maxA);
                angleMap[obstacle] = (minA, maxA);
            }

/*            foreach (var data in angleMap)
            {
                Debug.Log($"Pos : {data.Key}, MinaAngle : {data.Value.minAngle}, MaxAngle : {data.Value.maxAngle}");
            }*/

            // 2. 완전히 포함된 경우만 좁은 범위 제거
            var obstacles = angleMap.Keys.ToList();
            var removeList = new HashSet<Vector2Int>();
            for (int i = 0; i < obstacles.Count; i++)
            {
                var oi = obstacles[i];
                var (minA_i, maxA_i) = angleMap[oi];
                for (int j = 0; j < obstacles.Count; j++)
                {
                    if (i == j) continue;
                    var oj = obstacles[j];
                    var (minA_j, maxA_j) = angleMap[oj];

                    // oi의 각도 범위가 oj의 각도 범위에 완전히 포함되는 경우 oi를 제거
                    if (IsAngleRangeInside(minA_i, maxA_i, minA_j, maxA_j))
                    {
                        removeList.Add(oi);
                        break;
                    }
                }
            }
            foreach (var rm in removeList)
                angleMap.Remove(rm);

            return angleMap;
        }
        // 장애물의 4개 꼭짓점에서 내 위치로의 각도 구해 최소/최대 각도 리턴 (0~2PI)
        private void GetObstacleAngularRange(Vector2Int from, Vector2Int obstacle, out float minAngle, out float maxAngle)
        {
            Vector2[] corners = new Vector2[]
            {
                new Vector2(obstacle.x - 0.5f, obstacle.y - 0.5f),
                new Vector2(obstacle.x + 0.5f, obstacle.y - 0.5f),
                new Vector2(obstacle.x + 0.5f, obstacle.y + 0.5f),
                new Vector2(obstacle.x - 0.5f, obstacle.y + 0.5f)
            };

            minAngle = float.PositiveInfinity;
            maxAngle = float.NegativeInfinity;

            foreach (var corner in corners)
            {
                float angle = Mathf.Atan2(corner.y - from.y, corner.x - from.x);
                if (angle < 0) angle += 2 * Mathf.PI;
                if (angle < minAngle) minAngle = angle;
                if (angle > maxAngle) maxAngle = angle;
            }
        }
        // 범위1이 범위2에 "완전히 포함"되는지 (wrap-around 지원)
        private bool IsAngleRangeInside(float minA, float maxA, float minB, float maxB)
        {
            if (minB <= maxB)
            {
                // 일반구간
                return minA >= minB && maxA <= maxB;
            }
            else
            {
                // B가 랩어라운드 (ex: 350°~10°)
                return (minA >= minB && minA <= 2 * Mathf.PI) || (maxA >= 0 && maxA <= maxB);
            }
        }

        /// <summary>
        /// 주어진 각 장애물 각도 범위들을 이용해 baseRange 중 차단되지 않은 것만 반환
        /// </summary>
        public HashSet<Vector2Int> GetUseableRange(Vector2Int currentPosition, HashSet<Vector2Int> baseRange, Dictionary<Vector2Int, (float minAngle, float maxAngle)> angleMap)
        {
            var visibleTiles = new HashSet<Vector2Int>();
            foreach (var tile in baseRange)
            {
                if (tile == currentPosition)
                {
                    visibleTiles.Add(tile);
                    continue;
                }
                float angle = Mathf.Atan2(tile.y - currentPosition.y, tile.x - currentPosition.x);
                if (angle < 0) angle += 2 * Mathf.PI;

                bool blocked = false;
                foreach (var kvp in angleMap)
                {
                    var obsPos = kvp.Key;
                    var (minAngle, maxAngle) = kvp.Value;

                    // 각도 범위 포함
                    bool inAngle = InAngleRange(angle, minAngle, maxAngle);

                    // 거리 비교: tile이 장애물보다 멀리 있어야 가려짐
                    float distToObstacle = (obsPos - currentPosition).sqrMagnitude;
                    float distToTile = (tile - currentPosition).sqrMagnitude;

                    if (inAngle && distToTile > distToObstacle)
                    {
                        blocked = true;
                        break;
                    }
                }
                if (!blocked)
                    visibleTiles.Add(tile);
            }
            return visibleTiles;
        }
        // angle이 [minA, maxA] 구간에 포함되는지 (wrap-around 지원)
        private bool InAngleRange(float angle, float minA, float maxA)
        {
            float epsilon = 0.0001f;

            if (minA <= maxA)
                return minA + epsilon < angle && angle < maxA - epsilon;
            else
                return angle > minA + epsilon || angle < maxA - epsilon;
        }
    }
}