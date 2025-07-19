/*using System.Collections.Generic;
using UnityEngine;

using Foundations.Architecture.ReferencesHandler;
using GameSystems.TerrainSystem;

namespace GameSystems.UnitSystem
{
    public class ObstacleObjectSetter_Tile : IUtilityReferenceHandler, IObstacleObjectSetter
    {
        private GeneratedTerrainDataDBHandler GeneratedTileDataGroupHandler;

        public ObstacleObjectSetter_Tile()
        {
            var lazyReferenceHandlerManager = LazyReferenceHandlerManager.Instance;

            this.GeneratedTileDataGroupHandler = lazyReferenceHandlerManager.GetDynamicDataHandler<GeneratedTerrainDataDBHandler>();
        }

        public bool TryGetObstacleObject(HashSet<Vector2Int> baseRanges, WeightType weightType, int overcomeValue, out HashSet<Vector2Int> obstacleRanges)
        {
            obstacleRanges = new();
            bool hasObstacle = false;

            foreach (Vector2Int pos in baseRanges)
            {
                // '지형 차단 가중치 > 스킬 극복 가중치'보다 크면.
                if (this.GetWeight(weightType, pos) > overcomeValue)
                {
                    obstacleRanges.Add(pos);
                    hasObstacle = true;
                }
            }

            return hasObstacle;
        }

        private int GetWeight(WeightType weightType, Vector2Int pos)
        {
            if (this.GeneratedTileDataGroupHandler.TryGetGeneratedTerrainData(pos, out var data))
            {
                switch (weightType)
                {
                    case WeightType.Visible:
                        return data.TerrainData.VisibleBlockWeight;
                    case WeightType.Command:
                        return data.TerrainData.CommandBlockWeight;
                    case WeightType.Skill:
                        return data.TerrainData.SkillBlockWeight;
                    default:
                        return 0;
                }
            }
            else
                return 0;
        }
    }
}*/