using System.Collections.Generic;

using UnityEngine;

using Foundations.Architecture.ReferencesHandler;

namespace GameSystems.UtilitySystem
{
    public class GridRangeCalculator_MovePattern : IUtilityReferenceHandler
    {
        /// <summary>
        /// 지정된 시작 위치에서 방향에 따라 이동 가능한 기본 범위를 BFS로 계산합니다.
        /// </summary>
        public HashSet<Vector2Int> GetBaseRange(Vector2Int startPos, MovementPatternType patternType, int maxStep)
        {
            HashSet<Vector2Int> visited = new();
            Queue<(Vector2Int pos, int step)> queue = new();

            visited.Add(startPos);
            queue.Enqueue((startPos, 0));

            Vector2Int[] directions = GetDirections(patternType);
            while (queue.Count > 0)
            {
                var (current, step) = queue.Dequeue();
                if (step >= maxStep) continue;

                foreach (var dir in directions)
                {
                    var next = current + dir;
                    if (visited.Add(next))
                        queue.Enqueue((next, step + 1));
                }
            }

            visited.Remove(startPos); // 필요 시 제외
            return visited;
        }

        /// <summary>
        /// baseRanges에서 obstacleRanges를 제외한 위치로만 이동 가능한 위치를 BFS로 계산합니다.
        /// </summary>
        public HashSet<Vector2Int> GetAvailableRanges( HashSet<Vector2Int> baseRanges, HashSet<Vector2Int> obstacleRanges,
            Vector2Int startPos, MovementPatternType patternType, int moveDistance )
        {
            HashSet<Vector2Int> available = new();
            HashSet<Vector2Int> visited = new();
            Queue<(Vector2Int pos, int step)> queue = new();

            if (!baseRanges.Contains(startPos)) return available;

            queue.Enqueue((startPos, 0));
            visited.Add(startPos);
            available.Add(startPos);

            Vector2Int[] directions = GetDirections(patternType);
            while (queue.Count > 0)
            {
                var (current, step) = queue.Dequeue();
                if (step >= moveDistance) continue;

                foreach (var dir in directions)
                {
                    var next = current + dir;
                    if (visited.Contains(next)) continue;
                    if (!baseRanges.Contains(next)) continue;
                    if (obstacleRanges.Contains(next)) continue;

                    visited.Add(next);
                    available.Add(next);
                    queue.Enqueue((next, step + 1));
                }
            }

            available.Remove(startPos); // 필요 시 제외
            return available;
        }

        private Vector2Int[] GetDirections(MovementPatternType patternType)
        {
            return patternType switch
            {
                MovementPatternType.FourWay => FourWay,
                MovementPatternType.DiagonalOnly => DiagonalOnly,
                MovementPatternType.EightWay => EightWay,
                MovementPatternType.KnightJump => KnightMoves,
                _ => System.Array.Empty<Vector2Int>()
            };
        }

        private readonly Vector2Int[] FourWay = {
            new(1, 0), new(-1, 0), new(0, 1), new(0, -1)
        };

        private readonly Vector2Int[] DiagonalOnly = {
            new(1, 1), new(1, -1), new(-1, 1), new(-1, -1)
        };

        private readonly Vector2Int[] EightWay = {
            new(1, 0), new(-1, 0), new(0, 1), new(0, -1),
            new(1, 1), new(1, -1), new(-1, 1), new(-1, -1)
        };

        private readonly Vector2Int[] KnightMoves = {
            new(1, 2), new(2, 1), new(-1, 2), new(-2, 1),
            new(1, -2), new(2, -1), new(-1, -2), new(-2, -1)
        };
    }

    public enum MovementPatternType
    {
        FourWay,        // 상하좌우 4방위
        DiagonalOnly,   // 대각선 4방위
        EightWay,       // 8방위 포함
        KnightJump,     // 체스 나이트
    }
}