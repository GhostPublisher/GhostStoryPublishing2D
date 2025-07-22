using UnityEngine;

using Foundations.Architecture.ReferencesHandler;

namespace GameSystems.UtilitySystem 
{
    public class IsometricCoordinateConvertor : IUtilityReferenceHandler
    {
        private float GridSize_Width = 1f;
        private float GridSize_Hieght = 0.5f;

        public Vector2Int ConvertWorldToGrid(Vector3 worldPosition)
        {
            float halfTileWidth = GridSize_Width / 2f;
            float halfTileHeight = GridSize_Hieght / 2f;

            float gx = (worldPosition.x / halfTileWidth + worldPosition.y / halfTileHeight) / 2f;
            float gy = (worldPosition.y / halfTileHeight - worldPosition.x / halfTileWidth) / 2f;

            return new Vector2Int(Mathf.RoundToInt(gx), Mathf.RoundToInt(gy));
        }

        public Vector3 ConvertGridToWorld(Vector2 gridPosition)
        {
            float worldX = (gridPosition.x - gridPosition.y) * (GridSize_Width / 2f);
            float worldY = (gridPosition.x + gridPosition.y) * (GridSize_Hieght / 2f);
            return new Vector3(worldX, worldY, 0f);
        }
    }
}
