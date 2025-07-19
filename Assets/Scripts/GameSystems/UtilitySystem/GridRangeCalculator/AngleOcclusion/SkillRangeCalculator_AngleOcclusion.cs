using System.Collections.Generic;
using UnityEngine;

using GameSystems.EnemySystem.EnemyUnitSystem;
using GameSystems.PlayerSystem;
using GameSystems.TerrainSystem;
using Foundations.Architecture.ReferencesHandler;

namespace GameSystems.UtilitySystem
{
    public class SkillRangeCalculator_AngleOcclusion : IUtilityReferenceHandler
    {
        private GeneratedTerrainDataDBHandler GeneratedTerrainDataDBHandler;
        private PlayerUnitManagerDataDBHandler PlayerUnitManagerDataDBHandler;
        private EnemyUnitManagerDataDBHandler GeneratedEnemyUnitDataDBHandler;

        private GridAngleOcclusionFilter GridAngleOcclusionFilter;

        public SkillRangeCalculator_AngleOcclusion()
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;

            this.GeneratedTerrainDataDBHandler = HandlerManager.GetDynamicDataHandler<GeneratedTerrainDataDBHandler>();
            this.PlayerUnitManagerDataDBHandler = HandlerManager.GetDynamicDataHandler<PlayerUnitManagerDataDBHandler>();
            this.GeneratedEnemyUnitDataDBHandler = HandlerManager.GetDynamicDataHandler<EnemyUnitManagerDataDBHandler>();

            this.GridAngleOcclusionFilter = HandlerManager.GetUtilityHandler<GridAngleOcclusionFilter>();
        }

        // Player Skill이 실질적으로 적용될 수 있는 범위 값을 구함.
        public HashSet<Vector2Int> GetFilteredSkillRange_Player(Vector2Int relativePosition, HashSet<Vector2Int> rawSkillRange, int skillOvercomeWeight)
        {
            HashSet<Vector2Int> obstacleRange = new();

            foreach (Vector2Int pos in rawSkillRange)
            {
                // 극복할 수 없었을 시, 장애물로 추가.
                if (!this.CanOvercomeBlock_Player(pos, skillOvercomeWeight))
                    obstacleRange.Add(pos);
            }

            // 실질적으로 '범위'를 가리는 Range와 가리는 범위 값을 가져옴.
            var appliedOcclusionMap = this.GridAngleOcclusionFilter.GetObstacleAngleRanges(relativePosition, obstacleRange);
            // 가려지지 않은 Range 들을 가져옴. ( 최초로 범위를 방해하는 위치 값은 포함 )
            return this.GridAngleOcclusionFilter.GetUseableRange(relativePosition, rawSkillRange, appliedOcclusionMap);
        }
        // 매개변수로 받은 좌표의 '차단 가중치'를 '극복'할 수 있는지에 대한 메소드입니다.
        private bool CanOvercomeBlock_Player(Vector2Int pos, int skillOvercomeWeight)
        {
            // 값이 없으면, 지형이 없는 것 -> 지형이 없으면 상호작용 불가능 하게 표시.
            if (!this.GeneratedTerrainDataDBHandler.TryGetGeneratedTerrainData(pos, out var terrianData))
                return false;

            // 극복할 수 없었으면 false.
            if (terrianData.TerrainData.SkillBlockWeight > skillOvercomeWeight)
                return false;

            // 해당 위치에 'Enemy Unit'이 존재하면서 차단 가중치를 극복할 수 없었으면 false.
            if (this.GeneratedEnemyUnitDataDBHandler.TryGetEnemyUnitManagerData(pos, out var enemyUnitData) &&
                enemyUnitData.EnemyUnitStaticData.SkillBlockWeight > skillOvercomeWeight)
                return false;

            // 해당 없을 시 true.
            return true;
        }

        // Enemy Skill이 실질적으로 적용될 수 있는 범위 값을 구함.
        public HashSet<Vector2Int> GetFilteredSkillRange_Enemy(Vector2Int relativePosition, HashSet<Vector2Int> rawSkillRange, int skillOvercomeWeight)
        {
            HashSet<Vector2Int> obstacleRange = new();

            foreach (Vector2Int pos in rawSkillRange)
            {
                // 극복할 수 없었을 시, 장애물로 추가.
                if (!this.CanOvercomeBlock_Enemy(pos, skillOvercomeWeight))
                    obstacleRange.Add(pos);
            }

            // 실질적으로 '범위'를 가리는 Range와 가리는 범위 값을 가져옴.
            var appliedOcclusionMap = this.GridAngleOcclusionFilter.GetObstacleAngleRanges(relativePosition, obstacleRange);
            // 가려지지 않은 Range 들을 가져옴. ( 최초로 범위를 방해하는 위치 값은 포함 )
            return this.GridAngleOcclusionFilter.GetUseableRange(relativePosition, rawSkillRange, appliedOcclusionMap);
        }
        // 매개변수로 받은 좌표의 '차단 가중치'를 '극복'할 수 있는지에 대한 메소드입니다.
        private bool CanOvercomeBlock_Enemy(Vector2Int pos, int skillOvercomeWeight)
        {
            // 값이 없으면, 지형이 없는 것 -> 지형이 없으면 해당 위치 공격 불가능.
            if (!this.GeneratedTerrainDataDBHandler.TryGetGeneratedTerrainData(pos, out var terrianData))
                return false;

            // 극복할 수 없었으면 false.
            if (terrianData.TerrainData.SkillBlockWeight > skillOvercomeWeight)
                return false;

            // 해당 위치에 'Player Unit'이 존재하면서 차단 가중치를 극복할 수 없었으면 false.
            if (this.PlayerUnitManagerDataDBHandler.TryGetPlayerUnitManagerData(pos, out var playerUnitData) &&
                playerUnitData.PlayerUnitStaticData.SkillBlockWeight > skillOvercomeWeight)
                return false;

            // 해당 없을 시 true.
            return true;
        }
    }
}