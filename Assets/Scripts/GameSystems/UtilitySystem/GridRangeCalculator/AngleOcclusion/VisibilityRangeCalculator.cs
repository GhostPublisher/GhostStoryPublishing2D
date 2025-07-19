using System.Collections.Generic;
using UnityEngine;

using GameSystems.EnemySystem.EnemyUnitSystem;
using GameSystems.PlayerSystem;
using GameSystems.TerrainSystem;
using Foundations.Architecture.ReferencesHandler;

namespace GameSystems.UtilitySystem
{
    public class VisibilityRangeCalculator : IUtilityReferenceHandler
    {
        private GeneratedTerrainDataDBHandler GeneratedTerrainDataDBHandler;
        private PlayerUnitManagerDataDBHandler PlayerUnitManagerDataDBHandler;
        private EnemyUnitManagerDataDBHandler GeneratedEnemyUnitDataDBHandler;

        private GridAngleOcclusionFilter GridAngleOcclusionFilter;

        public VisibilityRangeCalculator()
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;

            this.GeneratedTerrainDataDBHandler = HandlerManager.GetDynamicDataHandler<GeneratedTerrainDataDBHandler>();
            this.PlayerUnitManagerDataDBHandler = HandlerManager.GetDynamicDataHandler<PlayerUnitManagerDataDBHandler>();
            this.GeneratedEnemyUnitDataDBHandler = HandlerManager.GetDynamicDataHandler<EnemyUnitManagerDataDBHandler>();

            this.GridAngleOcclusionFilter = HandlerManager.GetUtilityHandler<GridAngleOcclusionFilter>();
        }

        // 필터링 된 Player의 시야 값을 구함.
        public HashSet<Vector2Int> GetFilteredVisibleRange_Player(Vector2Int relativePosition, int visibleSize, int visibleOvercomeWeight)
        {
            HashSet<Vector2Int> rawVisibleRange = this.GetRawVisibleRange(relativePosition, visibleSize);
            HashSet<Vector2Int> obstacleRange = new();

            foreach (Vector2Int pos in rawVisibleRange)
            {
                // 극복할 수 없었을 시, 장애물로 추가.
                if(!this.CanOvercomeBlock_Player(pos, visibleOvercomeWeight))
                    obstacleRange.Add(pos);
            }

            // 실질적으로 '범위'를 가리는 Range와 가리는 범위 값을 가져옴.
            var appliedOcclusionMap = this.GridAngleOcclusionFilter.GetObstacleAngleRanges(relativePosition, obstacleRange);
            // 가려지지 않은 Range 들을 가져옴. ( 최초로 시야를 방해하는 위치 값은 포함 )
            return this.GridAngleOcclusionFilter.GetUseableRange(relativePosition, rawVisibleRange, appliedOcclusionMap);
        }
        // 필터링 된 Enemy의 시야 값을 구함.
        public HashSet<Vector2Int> GetFilteredVisibleRange_Enemy(Vector2Int relativePosition, int visibleSize, int visibleOvercomeWeight)
        {
            HashSet<Vector2Int> rawVisibleRange = this.GetRawVisibleRange(relativePosition, visibleSize);
            HashSet<Vector2Int> obstacleRange = new();

            foreach (Vector2Int pos in rawVisibleRange)
            {
                // 극복할 수 없었을 시, 장애물로 추가.
                if (!this.CanOvercomeBlock_Enemy(pos, visibleOvercomeWeight))
                    obstacleRange.Add(pos);
            }

            // 실질적으로 '범위'를 가리는 Range와 가리는 범위 값을 가져옴.
            var appliedOcclusionMap = this.GridAngleOcclusionFilter.GetObstacleAngleRanges(relativePosition, obstacleRange);
            // 가려지지 않은 Range 들을 가져옴. ( 최초로 시야를 방해하는 위치 값은 포함 )
            return this.GridAngleOcclusionFilter.GetUseableRange(relativePosition, rawVisibleRange, appliedOcclusionMap);
        }

        // 시야 기본 크기는 Diamond 확장형태 + 확장 Size를 통해 구해집니다
        private HashSet<Vector2Int> GetRawVisibleRange(Vector2Int relativePosition, int visibleSize)
        {
            HashSet<Vector2Int> visibleRange = new();

            for (int dx = -visibleSize; dx <= visibleSize; dx++)
            {
                for (int dy = -visibleSize; dy <= visibleSize; dy++)
                {
                    // 맨해튼 거리 조건 검사
                    if (Mathf.Abs(dx) + Mathf.Abs(dy) <= visibleSize)
                    {
                        visibleRange.Add(new Vector2Int(relativePosition.x + dx, relativePosition.y + dy));
                    }
                }
            }

            return visibleRange;
        }

        // 매개변수로 받은 지역의 '차단 가중치'를 '극복'할 수 있는지에 대한 메소드입니다.
        private bool CanOvercomeBlock_Player(Vector2Int pos, int visibleOvercomeWeight)
        {
            // 값이 없으면, 지형이 없는 것 -> 지형이 없어도 눈에는 보일 수 있도록 하자.
            if (!this.GeneratedTerrainDataDBHandler.TryGetGeneratedTerrainData(pos, out var terrianData))
                return true;

            // 극복할 수 없었으면 false.
            if (terrianData.TerrainData.VisibleBlockWeight > visibleOvercomeWeight)
                return false;

            // 해당 위치에 'Enemy Unit'이 존재하면서 차단 가중치를 극복할 수 없었으면 false.
            if (this.GeneratedEnemyUnitDataDBHandler.TryGetEnemyUnitManagerData(pos, out var enemyUnitData) &&
                enemyUnitData.EnemyUnitStaticData.VisibleBlockWeight > visibleOvercomeWeight)
                return false;

            // 해당 없을 시 true.
            return true;
        }

        // 매개변수로 받은 지역의 '차단 가중치'를 '극복'할 수 있는지에 대한 메소드입니다.
        private bool CanOvercomeBlock_Enemy(Vector2Int pos, int visibleOvercomeWeight)
        {
            // 값이 없으면, 지형이 없는 것 -> 지형이 없어도 눈에는 보일 수 있도록 하자.
            if (!this.GeneratedTerrainDataDBHandler.TryGetGeneratedTerrainData(pos, out var terrianData))
                return true;

            // 극복할 수 없었으면 false.
            if (terrianData.TerrainData.VisibleBlockWeight > visibleOvercomeWeight)
                return false;

            // 해당 위치에 'Player Unit'이 존재하면서 차단 가중치를 극복할 수 없었으면 false.
            if (this.PlayerUnitManagerDataDBHandler.TryGetPlayerUnitManagerData(pos, out var playerUnitData) &&
                playerUnitData.PlayerUnitStaticData.VisibleBlockWeight > visibleOvercomeWeight)
                return false;

            // 해당 없을 시 true.
            return true;
        }
    }
}