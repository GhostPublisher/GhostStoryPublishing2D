/*using System.Collections.Generic;

using UnityEngine;

using GameSystems.EnemySystem.EnemyUnitSystem;

using Foundations.Architecture.ReferencesHandler;
using GameSystems.UtilitySystem;
using GameSystems.TerrainSystem;

namespace GameSystems.UnitSystem
{
    public class EnemyMovePathFinder_StrategyTeleport : IMovePathFinder, IUtilityReferenceHandler
    {
        protected UnitPathFinder_GridBlock_Teleport unitPathFinder_GridBlock_Teleport;

        // Teleport는 새로운 이동 규칙이 있을 듯.
        // 예를 들어, 이동하고자 하는 위치에 유닛이 있을 때, -> Target 위치에서 가장 가까운 위치로이동한다. or 이동하지 않고 다른 일한다. ( 두번째꺼는 상황판단에서 할듯? )
        public bool TryGetMovePath(BehaviourStrategy behaviourStrategy, EnemyUnitManagerData generatedEnemyUnitData,  int groundOvercomeWeight, out HashSet<Vector2Int> movePath)
        {
            switch (behaviourStrategy)
            {
                case BehaviourStrategy.ToProtectee:
                    // 내가 '기억하고 있는' 보호 대상 위치값들을 리턴. ( MemoryData )
                    if (this.TryGetProtecteePositions(generatedEnemyUnitData, out var protecteePositions))
                    {
                        // 가장 가까운 '보호 대상'으로 이동하는 최단거리 리턴.
                        return this.TryGetPath_NearestTargetPosition(generatedEnemyUnitData.UnitGridPosition, protecteePositions,
                            groundOvercomeWeight, out movePath);
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
                        return this.TryGetPath_NearestTargetPosition(generatedEnemyUnitData.UnitGridPosition, directDetectedPlayerUnitPositions,
                            groundOvercomeWeight, out movePath);
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

        private bool TryGetProtecteePositions(EnemyUnitManagerData generatedEnemyUnitData, out HashSet<Vector2Int> targetPositions) { throw new System.NotImplementedException(); }
        private bool TryGetDirectDetectedPlayerUnitPositions(EnemyUnitManagerData generatedEnemyUnitData, out HashSet<Vector2Int> targetPositions) { throw new System.NotImplementedException(); }

        public bool TryGetPath_NearestTargetPosition(Vector2Int currentPosition, HashSet<Vector2Int> targetPositions, int groundOvercomeWeight, out HashSet<Vector2Int> movePath)
        {
            throw new System.NotImplementedException();
        }

        public bool TryGetPath_SingleTargetPosition(Vector2Int currentPosition, Vector2Int targetPosition, int groundOvercomeWeight, out HashSet<Vector2Int> movePath)
        {
            throw new System.NotImplementedException();
        }
    }
}*/