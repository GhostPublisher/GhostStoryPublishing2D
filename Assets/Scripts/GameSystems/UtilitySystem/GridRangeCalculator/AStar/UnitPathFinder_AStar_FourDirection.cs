using System.Collections.Generic;
using UnityEngine;

using GameSystems.EnemySystem.EnemyUnitSystem;
using GameSystems.PlayerSystem;
using GameSystems.TerrainSystem;
using Foundations.Architecture.ReferencesHandler;

namespace GameSystems.UtilitySystem
{
    public class PathAbleDecider
    {
        private GeneratedTerrainDataDBHandler GeneratedTileDataGroupHandler;
        private PlayerUnitManagerDataDBHandler PlayerUnitManagerDataDBHandler;
        private EnemyUnitManagerDataDBHandler GeneratedEnemyUnitDataGroupHandler;

        public PathAbleDecider()
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;

            this.GeneratedTileDataGroupHandler = HandlerManager.GetDynamicDataHandler<GeneratedTerrainDataDBHandler>();
            this.PlayerUnitManagerDataDBHandler = HandlerManager.GetDynamicDataHandler<PlayerUnitManagerDataDBHandler>();
            this.GeneratedEnemyUnitDataGroupHandler = HandlerManager.GetDynamicDataHandler<EnemyUnitManagerDataDBHandler>();
        }

        // 기본적인 타일 통과 가능 판단 메소드 ( '지형'이 존재하는가?, 중첩 불가 객체가 해당 위치에 존재하는가? )
        // '지형' : 게임 내 유닛이 위치할 수 있는 좌표인지를 말합니다.
        // '중첩 불가 객체' : Player Unit, Enemy Unit 같은 느낌.
        public bool IsPassable_Base(Vector2Int pos)
        {
            // '지형 데이터'를 가져올 수 없다는 것 -> '해당 좌표는 존재하지 않는 좌표라는 것'
            if (!this.GeneratedTileDataGroupHandler.TryGetGeneratedTerrainData(pos, out var terrainData))
                return false;

            // 플레이어 존재 시 통과 불가. == false 리턴
            if (this.PlayerUnitManagerDataDBHandler.TryGetPlayerUnitManagerData(pos, out var _))
                return false;

            // Enemy 존재 시 통과 불가. == false 리턴
            if (this.GeneratedEnemyUnitDataGroupHandler.TryGetEnemyUnitManagerData(pos, out var _))
                return false;

            return true;
        }

        /*public bool IsPassable_Overcome(Vector2Int pos, int groundOvercomeWeight, int waterOvercomeWeight, int )
        {

        }*/
    }

    public class UnitPathFinder_AStar_FourDirection : IUtilityReferenceHandler
    {
        private GeneratedTerrainDataDBHandler GeneratedTileDataGroupHandler;
        private PlayerUnitManagerDataDBHandler PlayerUnitManagerDataDBHandler;
        private EnemyUnitManagerDataDBHandler GeneratedEnemyUnitDataGroupHandler;

        public UnitPathFinder_AStar_FourDirection()
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;

            this.GeneratedTileDataGroupHandler = HandlerManager.GetDynamicDataHandler<GeneratedTerrainDataDBHandler>();
            this.PlayerUnitManagerDataDBHandler = HandlerManager.GetDynamicDataHandler<PlayerUnitManagerDataDBHandler>();
            this.GeneratedEnemyUnitDataGroupHandler = HandlerManager.GetDynamicDataHandler<EnemyUnitManagerDataDBHandler>();
        }

        // 목표 지점이 Passable하지 않으면 제외하고 리턴함.
        public List<Vector2Int> GetMovePath(Vector2Int start, Vector2Int goal, int groundOvercomeWeight)
        {
            var openSet = new PriorityQueue<Vector2Int, int>();
            openSet.Enqueue(start, 0);

            var cameFrom = new Dictionary<Vector2Int, Vector2Int>();
            var gScore = new Dictionary<Vector2Int, int> { [start] = 0 };

            Vector2Int closestToGoal = start;
            int closestDistance = Heuristic(start, goal);


            while (openSet.Count > 0)
            {
                var current = openSet.Dequeue();

                int currentDistance = Heuristic(current, goal);
                if (currentDistance < closestDistance)
                {
                    closestToGoal = current;
                    closestDistance = currentDistance;
                }

                if (current == goal)
                    return ReconstructPath(cameFrom, current);

                foreach (var dir in this.FourWay)
                {
                    var neighbor = current + dir;

                    // 지나갈 수 없는 타일은 스킵 || 통과 가능성 체크
                    // 새 기준: 타일 유효성, 통과 가능성 체크
                    if (!IsPassable(neighbor, groundOvercomeWeight))
                        continue;

                    int tentativeG = gScore[current] + 1; // 항상 1씩만 증가

                    if (!gScore.TryGetValue(neighbor, out var prevG) || tentativeG < prevG)
                    {
                        cameFrom[neighbor] = current;
                        gScore[neighbor] = tentativeG;
                        int fScore = tentativeG + Heuristic(neighbor, goal);
                        openSet.Enqueue(neighbor, fScore);
                    }
                }
            }

            return ReconstructPath(cameFrom, closestToGoal); // 가장 근접했던 좌표 반환.
        }

        // 타일 통과 가능성 판단 메소드
        private bool IsPassable(Vector2Int pos, int groundOvercomeWeight)
        {
            // '지형 데이터'를 가져올 수 없으면 해당 위치는 'tile'이 없는 것 == false 리턴.
            if (!this.GeneratedTileDataGroupHandler.TryGetGeneratedTerrainData(pos, out var terrainData))
                return false;

            // 해당 Terrain의 차단 가중치를 극복 할 수 없으 시, false 리턴.
            if (terrainData.TerrainData.GroundBlockWeight > groundOvercomeWeight)
                return false;

            // 플레이어 존재 시 통과 불가. == false 리턴
            if (this.PlayerUnitManagerDataDBHandler.TryGetPlayerUnitManagerData(pos, out var _))
                return false;

            // Enemy 존재 시 통과 불가. == false 리턴
            if (this.GeneratedEnemyUnitDataGroupHandler.TryGetEnemyUnitManagerData(pos, out var _))
                return false;

            return true;
        }


        // 경로를 되짚어가며 재구성하는 메서드
        private List<Vector2Int> ReconstructPath(Dictionary<Vector2Int, Vector2Int> cameFrom, Vector2Int current)
        {
            List<Vector2Int> path = new List<Vector2Int> { current };

            // 출발지에 도달할 때까지 거슬러 올라감
            while (cameFrom.ContainsKey(current))
            {
                current = cameFrom[current];
                path.Add(current);
            }

            path.Reverse(); // 역순으로 기록되었기 때문에 순서를 뒤집음
            return path;
        }

        // 두 지점 간의 맨해튼 거리(예상 이동 비용)를 반환하는 휴리스틱 함수
        private int Heuristic(Vector2Int a, Vector2Int b)
        {
            return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
        }

        private readonly Vector2Int[] FourWay = {
            new(1, 0), new(-1, 0), new(0, 1), new(0, -1)
        };

        public class PriorityQueue<TElement, TPriority> where TPriority : System.IComparable<TPriority>
        {
            private readonly List<(TElement element, TPriority priority)> list = new();

            public int Count => list.Count;

            public void Enqueue(TElement element, TPriority priority)
            {
                list.Add((element, priority));
            }

            public TElement Dequeue()
            {
                int minIndex = 0;
                for (int i = 1; i < list.Count; i++)
                    if (list[i].priority.CompareTo(list[minIndex].priority) < 0)
                        minIndex = i;
                var elem = list[minIndex].element;
                list.RemoveAt(minIndex);
                return elem;
            }
        }
    }
}