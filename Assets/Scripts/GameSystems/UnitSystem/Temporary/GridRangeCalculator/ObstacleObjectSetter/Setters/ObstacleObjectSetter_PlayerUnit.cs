/*using System.Collections.Generic;
using UnityEngine;

using Foundations.Architecture.ReferencesHandler;
using GameSystems.PlayerSystem;

namespace GameSystems.UnitSystem
{
    public class ObstacleObjectSetter_PlayerUnit : IUtilityReferenceHandler, IObstacleObjectSetter
    {
        private PlayerUnitManagerDataDBHandler PlayerUnitManagerDataDBHandler;

        public ObstacleObjectSetter_PlayerUnit()
        {
            var lazyReferenceHandlerManager = LazyReferenceHandlerManager.Instance;

            this.PlayerUnitManagerDataDBHandler = lazyReferenceHandlerManager.GetDynamicDataHandler<PlayerUnitManagerDataDBHandler>();
        }

        public bool TryGetObstacleObject(HashSet<Vector2Int> baseRanges, WeightType weightType, int overcomeValue, out HashSet<Vector2Int> obstacleRanges)
        {
            obstacleRanges = new HashSet<Vector2Int>();
            bool hasObstacle = false;

            foreach (Vector2Int pos in baseRanges)
            {
                // '���� ���� ����ġ > ��ų �غ� ����ġ'���� ũ��.
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
            if (this.PlayerUnitManagerDataDBHandler.TryGetPlayerUnitManagerData(pos, out var data))
            {
                switch (weightType)
                {
                    case WeightType.Command:
                        return data.PlayerUnitDynamicDataGroup.CommandBlockWeight;
                    case WeightType.Skill:
                        return data.PlayerUnitDynamicDataGroup.SkillBlockWeight;
                    default:
                        return 0;
                }
            }
            else
                return 0;
        }
    }
}*/