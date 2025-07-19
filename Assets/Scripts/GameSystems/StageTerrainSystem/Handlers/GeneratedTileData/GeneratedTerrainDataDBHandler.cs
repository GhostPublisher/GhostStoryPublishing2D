using System;
using System.Collections.Generic;

using UnityEngine;

using Foundations.Architecture.ReferencesHandler;

namespace GameSystems.TerrainSystem
{
    public class GeneratedTerrainDataDBHandler : IDynamicReferenceHandler
    {
        private GeneratedTerrainData[,] GeneratedTerrainDatas;

        public void InitialSetting(int width, int height)
        {
            this.GeneratedTerrainDatas = null;
            this.GeneratedTerrainDatas = new GeneratedTerrainData[width, height];
        }

        public void AddGeneratedTerrainData(GeneratedTerrainData generatedTerrainData)
        {
            this.GeneratedTerrainDatas[generatedTerrainData.GridPosition.x, generatedTerrainData.GridPosition.y] = generatedTerrainData;
        }

        public IEnumerable<GeneratedTerrainData> GetGeneratedTerrainDataAll()
        {
            int width = this.GeneratedTerrainDatas.GetLength(0);
            int height = this.GeneratedTerrainDatas.GetLength(1);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (this.GeneratedTerrainDatas[x, y] == null) continue;
                    else yield return this.GeneratedTerrainDatas[x, y];
                }
            }
        }

        public bool TryGetGeneratedTerrainData(Vector2Int pos, out GeneratedTerrainData generatedTerrainData)
        {
            generatedTerrainData = null;

            if (!IsValidTilePosition(pos))
                return false;

            generatedTerrainData = GeneratedTerrainDatas[pos.x, pos.y];
            return generatedTerrainData != null;
        }
        public bool IsValidTilePosition(Vector2Int pos)
        {
            if (this.GeneratedTerrainDatas == null
                || this.GeneratedTerrainDatas.GetLength(0) == 0
                || this.GeneratedTerrainDatas.GetLength(1) == 0)
                return false;

            return 0 <= pos.x && pos.x < this.GeneratedTerrainDatas.GetLength(0)
                && 0 <= pos.y && pos.y < this.GeneratedTerrainDatas.GetLength(1);
        }

        public void ClearGeneratedTerrainDataGroup()
        {
            Array.Clear(GeneratedTerrainDatas, 0, GeneratedTerrainDatas.Length);
        }
    }

    public class GeneratedTerrainData
    {
        public Vector2Int GridPosition;

        public TerrainData TerrainData;
    }
}