using System;

using UnityEngine;

using Foundations.Architecture.ReferencesHandler;

namespace GameSystems.TerrainSystem
{
    [Serializable]
    public class StageGroundTilemapDataDBHandler : IStaticReferenceHandler
    {
        private StageGroundTilemapDataGroup StageGroundTilemapDataGroup;

        private bool isInitialized = false;

        private void LoadScriptableObject()
        {
            this.StageGroundTilemapDataGroup = Resources.Load<StageGroundTilemapDataGroup>("ScriptableObject/TerrainData/StageGroundTilemapDataGroup");

            this.isInitialized = true;
        }

        // stageID에 해당되는 TileMap GameObject Prefab을 가져옴.
        public bool TryGetStageGroundTileMapData(int stageID, out StageGroundTileMapData stageGroundTileMapData)
        {
            if (!this.isInitialized) this.LoadScriptableObject();

            stageGroundTileMapData = null;
            if (this.StageGroundTilemapDataGroup == null) return false;

            return this.StageGroundTilemapDataGroup.TryGetStageGroundTileMapData(stageID, out stageGroundTileMapData);
        }
    }
}
