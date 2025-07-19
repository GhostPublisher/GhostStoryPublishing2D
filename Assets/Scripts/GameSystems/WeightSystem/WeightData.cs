/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GameSystems.StageMapSystem.TileSystem;

namespace GameSystems.WeightSystem
{
    public class PositionWeightDataGroup
    {
        private PositionWeightData[,] PositionWeightData;

        public void InitialSetting(int width, int height)
        {
            this.PositionWeightData = null;
            this.PositionWeightData = new PositionWeightData[width, height];
        }

        public void AddPositionWeightData(Vector2Int position)
        {
            // 범위가 아니면 넘기기.
            if (!this.IsValidPosition(position)) return;

            if (this.PositionWeightData[position.x, position.y] == null)
                this.PositionWeightData[position.x, position.y] = new PositionWeightData(position);
        }


        public bool TryGetPositionWeightData(Vector2Int position, out PositionWeightData positionWeightData)
        {
            if (!this.IsValidPosition(position))
            {
                positionWeightData = null;
                return false;
            }

            if (this.PositionWeightData[position.x, position.y] != null)
            {
                positionWeightData = this.PositionWeightData[position.x, position.y];
                return true;
            }

            positionWeightData = null;
            return false;
        }
        // 좌표가 
        public bool IsValidPosition(Vector2Int position)
        {
            if (position.x < 0 || position.x >= this.PositionWeightData.GetLength(0) ||
                position.y < 0 || position.y >= this.PositionWeightData.GetLength(1))
                return false;

            return true;
        }
    }

    public class PositionWeightData
    {
        public Vector2Int Position;

        public TerrainType TerrainType;

        public int VisibleBlockWeight;          // 시야 차단 가중치
        public int CommandBlockWeight;          // 명령 차단 가중치
        public int SkillBlockWeight;            // 공격 차단 가중치
        public int MoveBlockWeight;             // 이동 차단 가중치

        public bool IsEmpty;

        public PositionWeightData(Vector2Int position)
        {
            Position = position;
        }
    }
}*/