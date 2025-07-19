/*using System.Collections.Generic;

using UnityEngine;

using Foundations.Architecture.ReferencesHandler;

namespace GameSystems.UnitSystem
{
    public class GridRangeCalculator : IUtilityReferenceHandler
    {
        public HashSet<Vector2Int> GetRange(Vector2Int relativePos, ExtendPatternType extendPatternType, int extendSize)
        {
            HashSet<Vector2Int> uniquePositions = new();

            switch (extendPatternType)
            {
                case ExtendPatternType.Single:
                    uniquePositions.Add(relativePos);
                    break;
                case ExtendPatternType.Square:
                    uniquePositions = this.GetRange_Square(relativePos, extendSize);
                    break;
                case ExtendPatternType.Diamond:
                    uniquePositions = this.GetRange_Diamond(relativePos, extendSize);
                    break;
                case ExtendPatternType.Cone:
                    break;
                case ExtendPatternType.Global:
                    break;
                case ExtendPatternType.Rectangle_R1:
                    break;
                case ExtendPatternType.Rectangle_C1:
                    break;
                default:
                    break;
            }

            return uniquePositions;
        }

        private HashSet<Vector2Int> GetRange_Diamond(Vector2Int relativePos, int ExtendSize)
        {
            HashSet<Vector2Int> uniquePositions = new();

            for (int dx = -ExtendSize; dx <= ExtendSize; dx++)
            {
                for (int dy = -ExtendSize; dy <= ExtendSize; dy++)
                {
                    // 맨해튼 거리 조건 검사
                    if (Mathf.Abs(dx) + Mathf.Abs(dy) <= ExtendSize)
                    {
                        uniquePositions.Add(new Vector2Int(relativePos.x + dx, relativePos.y + dy));
                    }
                }
            }

            return uniquePositions;
        }

        private HashSet<Vector2Int> GetRange_Square(Vector2Int relativePos, int ExtendSize)
        {
            HashSet<Vector2Int> uniquePositions = new();

            for (int dx = -ExtendSize; dx <= ExtendSize; dx++)
            {
                for (int dy = -ExtendSize; dy <= ExtendSize; dy++)
                {
                    uniquePositions.Add(relativePos + new Vector2Int(dx, dy));
                }
            }

            return uniquePositions;
        }
    }

    public enum UnitDirection
    {
        Up,     // ( 0, 1 )
        Right,  // ( 1, 0 )
        Down,   // ( 0, -1 )
        Left    // ( -1, 0 )
    }
}*/