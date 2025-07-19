using System.Collections.Generic;
using UnityEngine;

using GameSystems.EnemySystem.EnemyUnitSystem;
using GameSystems.PlayerSystem;
using GameSystems.TerrainSystem;
using Foundations.Architecture.ReferencesHandler;

namespace GameSystems.UtilitySystem
{
    public interface IUnitMoveRangeCalculator_BFS
    {
        public HashSet<Vector2Int> GetMoveRange(Vector2Int startPosition, int maxStep, int moveovercomeWeight);
    }

    public class UnitMoveRangeCalculator_BFS_FourDirection : IUnitMoveRangeCalculator_BFS, IUtilityReferenceHandler
    {
        private GeneratedTerrainDataDBHandler GeneratedTileDataGroupHandler;
        private PlayerUnitManagerDataDBHandler PlayerUnitManagerDataDBHandler;
        private EnemyUnitManagerDataDBHandler GeneratedEnemyUnitDataGroupHandler;

        public UnitMoveRangeCalculator_BFS_FourDirection()
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;

            this.GeneratedTileDataGroupHandler = HandlerManager.GetDynamicDataHandler<GeneratedTerrainDataDBHandler>();
            this.PlayerUnitManagerDataDBHandler = HandlerManager.GetDynamicDataHandler<PlayerUnitManagerDataDBHandler>();
            this.GeneratedEnemyUnitDataGroupHandler = HandlerManager.GetDynamicDataHandler<EnemyUnitManagerDataDBHandler>();
        }

        // 이동가능 범위를 리턴함.
        public HashSet<Vector2Int> GetMoveRange(Vector2Int startPosition, int maxStep, int moveOvercomeWeight)
        {
            HashSet<Vector2Int> moveableRange = new();

            HashSet<Vector2Int> visited = new();
            Queue<(Vector2Int position, int step)> queue = new();

            moveableRange.Add(startPosition);
            queue.Enqueue((startPosition, 0));
            visited.Add(startPosition);

            while (queue.Count > 0)
            {
                var (currentPos, currentStep) = queue.Dequeue();
                if (currentStep >= maxStep) continue;

                foreach (var dir in this.FourWay)
                {
                    var next = currentPos + dir;

                    // 이미 방문한 지역일 시, 넘어감.
                    if (visited.Contains(next)) continue;

                    visited.Add(next);

                    // 해당 지역에 '지형이 없을' 시, 넘어감.
                    if (!this.GeneratedTileDataGroupHandler.TryGetGeneratedTerrainData(next, out var _)) continue;
                    // 해당 지역을 '통과할 수 없을' 시, 넘어감.
                    if (!this.IsPassable(next, moveOvercomeWeight)) continue;

                    moveableRange.Add(next);
                    queue.Enqueue((next, currentStep + 1));
                }
            }

            // 이거 의미없을지도.
            return moveableRange;
        }

        // 타일 통과 가능성 판단 메소드
        private bool IsPassable(Vector2Int pos, int moveovercomeWeight)
        {
            // '지형 데이터'를 가져올 수 없으면 해당 위치는 'tile'이 없는 것 == false 리턴.
            // 그런데 해당 지역의 존재 여부는 먼저 확인한 상태라 이 부분에서 false가 나올리는 없긴함.
            if (!this.GeneratedTileDataGroupHandler.TryGetGeneratedTerrainData(pos, out var tileData))
                return false;

            // 해당 지형을 극복할 수 없으면 지나갈 수 없음 == false 리턴.
            if (tileData.TerrainData.GroundBlockWeight > moveovercomeWeight)
                return false;

            // 플레이어 존재 시 통과 불가. == false 리턴
            if (this.PlayerUnitManagerDataDBHandler.TryGetPlayerUnitManagerData(pos, out var _))
                return false;

            // Enemy 존재 시 통과 불가. == false 리턴
            if (this.GeneratedEnemyUnitDataGroupHandler.TryGetEnemyUnitManagerData(pos, out var _))
                return false;

            return true;
        }

        private readonly Vector2Int[] FourWay = {
            new(1, 0), new(-1, 0), new(0, 1), new(0, -1)
        };
    }
}