using System.Collections.Generic;
using UnityEngine;

using GameSystems.EnemySystem.EnemyUnitSystem;
using GameSystems.PlayerSystem;
using GameSystems.TerrainSystem;
using Foundations.Architecture.ReferencesHandler;

namespace GameSystems.UtilitySystem
{
    public class UnitPathFinder_GridBlock_Teleport : IUtilityReferenceHandler
    {
        private GeneratedTerrainDataDBHandler GeneratedTileDataGroupHandler;
        private PlayerUnitManagerDataDBHandler PlayerUnitManagerDataDBHandler;
        private EnemyUnitManagerDataDBHandler GeneratedEnemyUnitDataGroupHandler;

        public UnitPathFinder_GridBlock_Teleport()
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;

            this.GeneratedTileDataGroupHandler = HandlerManager.GetDynamicDataHandler<GeneratedTerrainDataDBHandler>();
            this.PlayerUnitManagerDataDBHandler = HandlerManager.GetDynamicDataHandler<PlayerUnitManagerDataDBHandler>();
            this.GeneratedEnemyUnitDataGroupHandler = HandlerManager.GetDynamicDataHandler<EnemyUnitManagerDataDBHandler>();
        }

        public List<Vector2Int> GetMovePath(Vector2Int start, Vector2Int goal, int groundOvercomeWeight)
        {
            List<Vector2Int> movePath = new();

            movePath.Add(start);

            if (this.IsPassable(goal, groundOvercomeWeight))
                movePath.Add(goal);

            return movePath;
        }

        // 타일 통과 가능성 판단 메소드
        public bool IsPassable(Vector2Int targetPosition, int groundOvercomeWeight)
        {
            // '지형 데이터'를 가져올 수 없으면 해당 위치는 'tile'이 없는 것 == false 리턴.
            if (!this.GeneratedTileDataGroupHandler.TryGetGeneratedTerrainData(targetPosition, out var terrainData))
                return false;

            // 해당 Terrain의 Ground 차단 가중치를 극복할 수 없으시 false 리턴.
            if (terrainData.TerrainData.GroundBlockWeight > groundOvercomeWeight)
                return false;

            // 플레이어 존재 시 통과 불가. == false 리턴
            if (this.PlayerUnitManagerDataDBHandler.TryGetPlayerUnitManagerData(targetPosition, out var _))
                return false;

            // Enemy 존재 시 통과 불가. == false 리턴
            if (this.GeneratedEnemyUnitDataGroupHandler.TryGetEnemyUnitManagerData(targetPosition, out var _))
                return false;

            return true;
        }
    }
}