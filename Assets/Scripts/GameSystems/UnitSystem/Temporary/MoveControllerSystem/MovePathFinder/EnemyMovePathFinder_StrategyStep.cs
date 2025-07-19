/*using System.Collections.Generic;

using UnityEngine;

using GameSystems.EnemySystem.EnemyUnitSystem;

using Foundations.Architecture.ReferencesHandler;
using GameSystems.UtilitySystem;
using GameSystems.TerrainSystem;

namespace GameSystems.UnitSystem
{
    public interface IMovePathFinder
    {
        public bool TryGetMovePath(BehaviourStrategy behaviourStrategy, EnemyUnitManagerData generatedEnemyUnitData, int groundOvercomeWeight, out HashSet<Vector2Int> movePath);
        public bool TryGetPath_NearestTargetPosition(Vector2Int currentPosition, HashSet<Vector2Int> targetPositions,
            int groundOvercomeWeight, out HashSet<Vector2Int> movePath);
        public bool TryGetPath_SingleTargetPosition(Vector2Int currentPosition, Vector2Int targetPosition,
            int groundOvercomeWeight, out HashSet<Vector2Int> movePath);
    }

    public class EnemyMovePathFinder_StrategyStep : IMovePathFinder, IUtilityReferenceHandler
    {
        protected UnitPathFinder_AStar_FourDirection unitAStarPathFinderLogic;

        public EnemyMovePathFinder_StrategyStep()
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            this.unitAStarPathFinderLogic = HandlerManager.GetUtilityHandler<UnitPathFinder_AStar_FourDirection>();
        }

        public bool TryGetMovePath(BehaviourStrategy behaviourStrategy, EnemyUnitManagerData generatedEnemyUnitData, int groundOvercomeWeight, out HashSet<Vector2Int> movePath)
        {
            switch (behaviourStrategy)
            {
                case BehaviourStrategy.ToProtectee:
                    // 내가 '기억하고 있는' 보호 대상 위치값들을 리턴. ( MemoryData )
                    if (this.TryGetProtecteePositions(generatedEnemyUnitData, out var protecteePositions))
                    {
                        // 가장 가까운 '보호 대상'으로 이동하는 최단거리 리턴.
                        return this.TryGetPath_NearestTargetPosition(generatedEnemyUnitData.UnitGridPosition,
                            protecteePositions, groundOvercomeWeight, out movePath);
                    }
                    else
                    {
                        movePath = null;
                        return false;
                    }
                case BehaviourStrategy.ToDetectedUnit:
                    // 내가 '보고있는' Player Unit 위치값들을 리턴. ( DirectData )
                    if (this.TryGetDirectDetectedPlayerUnitPositions(generatedEnemyUnitData, out var directDetectedPlayerUnitPositions))
                    {
                        // 가장 가까이에 보이는 'Player Unit'을 향해 이동.
                        return this.TryGetPath_NearestTargetPosition(generatedEnemyUnitData.UnitGridPosition,
                            directDetectedPlayerUnitPositions, groundOvercomeWeight, out movePath);
                    }
                    else
                    {
                        movePath = null;
                        return false;
                    }
                default:
                    movePath = null;
                    return false;
            }
        }

        // 전달받은 '내위치'에서 전달받은 '목표위치들' 중 가장 가까운 위치까지의 최단거리 경로를 반환.
        public bool TryGetPath_NearestTargetPosition(Vector2Int currentPosition, HashSet<Vector2Int> targetPositions,
            int groundOvercomeWeight, out HashSet<Vector2Int> movePath)
        {
            if (targetPositions.Count == 0)
            {
                movePath = null;
                return false;
            }

            (Vector2Int, HashSet<Vector2Int>) resultPath = new();
            resultPath.Item2 = new();

            List<Vector2Int> tempPath = new();
            int pathCount = 999;

            foreach (Vector2Int targetPosition in targetPositions)
            {
                tempPath.Clear();
                tempPath = this.unitAStarPathFinderLogic.GetMovePath(currentPosition, targetPosition, groundOvercomeWeight);

                if (pathCount > tempPath.Count)
                {
                    resultPath.Item1 = targetPosition;
                    resultPath.Item2 = new(tempPath);
                    pathCount = resultPath.Item2.Count;
                }
            }

            // 이동경로 크기가 1이면, 자기 자신의 위치값만을 말한다. 이동할 필요 없음.
            if (resultPath.Item2.Count <= 1)
            {
                movePath = null;
                return false;
            }

            // 정상 경로 반환.
            movePath = resultPath.Item2;
            return true;
        }

        // 매개변수로 받은 '내 위치 -> 목표 위치'까지의 최단 경로를 TryGetValue 형식으로 반환한다.
        public bool TryGetPath_SingleTargetPosition(Vector2Int currentPosition, Vector2Int targetPosition,
            int groundOvercomeWeight, out HashSet<Vector2Int> movePath)
        {
            var path = this.unitAStarPathFinderLogic.GetMovePath(currentPosition, targetPosition, groundOvercomeWeight);

            // 이동경로 크기가 1이면, 자기 자신의 위치값만을 말한다. 이동할 필요 없음.
            if (path.Count <= 1)
            {
                movePath = null;
                return false;
            }

            movePath = new HashSet<Vector2Int>(path);
            return true;
        }




        // 내가 '기억하고 있는' 보호 대상 위치값들을 리턴. ( MemoryData )
        private bool TryGetProtecteePositions(EnemyUnitManagerData generatedEnemyUnitData, out HashSet<Vector2Int> targetPositions)
        {
            targetPositions = null;

            if (generatedEnemyUnitData.MemorizedDatas.TryGetValue(typeof(MemorizedDataGroup_ProtecteeUnit), out var memoryData))
            {
                if (memoryData is MemorizedDataGroup_ProtecteeUnit protecteeUnits)
                {
                    foreach (var data in protecteeUnits.MemorizedData_ProtecteeUnits.Values)
                    {
                        targetPositions.Add(data.UnitPosition);
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        // 내가 '보고있는' Player Unit 위치값들을 리턴. ( DirectData )
        private bool TryGetDirectDetectedPlayerUnitPositions(EnemyUnitManagerData generatedEnemyUnitData, out HashSet<Vector2Int> targetPositions)
        {
            targetPositions = null;

            if (generatedEnemyUnitData.DirectDetectionDatas.TryGetValue(typeof(DirectDetectionData_PlayerUnit), out var directData))
            {
                if (directData is DirectDetectionData_PlayerUnit playerUnits)
                {
                    foreach (var data in playerUnits.DetectedPlayerDatas.Values)
                    {
                        targetPositions.Add(data);
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}*/