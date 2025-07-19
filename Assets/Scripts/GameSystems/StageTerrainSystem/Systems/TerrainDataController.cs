using System.Collections.Generic;

using UnityEngine;
using Foundations.Architecture.ReferencesHandler;

namespace GameSystems.TerrainSystem
{
    public class TerrainDataController : MonoBehaviour
    {
        private StageTerrainDataDBHandler StageTerrainDataDBHandler;

        private GeneratedTerrainDataDBHandler myGeneratedTerrainDataDBHandler;

        private void Awake()
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            this.StageTerrainDataDBHandler = HandlerManager.GetStaticDataHandler<StageTerrainDataDBHandler>();
            this.myGeneratedTerrainDataDBHandler = HandlerManager.GetDynamicDataHandler<GeneratedTerrainDataDBHandler>();
        }

        public void InitialSetting(int stageID)
        {
            if (!this.StageTerrainDataDBHandler.TryGetStageTileSapwnDatas(stageID, out int width, out int height, out var terrainDatas))
            {
                Debug.Log($"일단 관련 DB Json 가져오는거 실패한듯?");
            }

            this.InitialSetGeneratedTerrainData(width, height);
            this.AdditionalSetGeneratedTerrainData(terrainDatas);
        }
        public void InitialSetGeneratedTerrainData(int width, int height)
        {
            this.myGeneratedTerrainDataDBHandler.InitialSetting(width, height);

            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    TerrainData terrainData = new();

                    GeneratedTerrainData newGeneratedTerrainData = new();
                    newGeneratedTerrainData.GridPosition = new Vector2Int(x, y);
                    newGeneratedTerrainData.TerrainData = terrainData;

                    this.myGeneratedTerrainDataDBHandler.AddGeneratedTerrainData(newGeneratedTerrainData);
                }
            }
        }
        public void AdditionalSetGeneratedTerrainData(List<TerrainData> TerrainDatas)
        {
            foreach (var terrainData in TerrainDatas)
            {
//                Debug.Log($"X : {terrainData.GridPositionX}, Y : {terrainData.GridPositionY}");
//                Debug.Log($"AdditionalSetGeneratedTerrainData - 0");

                if (!this.myGeneratedTerrainDataDBHandler.TryGetGeneratedTerrainData(new Vector2Int(terrainData.GridPositionX, terrainData.GridPositionY), out var data)) continue;
//                Debug.Log($"AdditionalSetGeneratedTerrainData - 1");

                data.TerrainData.GridPositionX = terrainData.GridPositionX;
                data.TerrainData.GridPositionY = terrainData.GridPositionY;
                data.TerrainData.VisibleBlockWeight = terrainData.VisibleBlockWeight;
                data.TerrainData.GroundBlockWeight = terrainData.GroundBlockWeight;
                data.TerrainData.CommandBlockWeight = terrainData.CommandBlockWeight;
                data.TerrainData.SkillBlockWeight = terrainData.SkillBlockWeight;
            }
        }

        public void ClearTrerrainData()
        {
            this.myGeneratedTerrainDataDBHandler.ClearGeneratedTerrainDataGroup();
        }
    }
}