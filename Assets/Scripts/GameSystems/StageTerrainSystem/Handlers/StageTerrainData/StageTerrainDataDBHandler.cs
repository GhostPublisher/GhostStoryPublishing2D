using System;
using System.Collections.Generic;

using UnityEngine;

using Foundations.Architecture.ReferencesHandler;

namespace GameSystems.TerrainSystem
{
    [Serializable]
    public class StageTerrainDataDBHandler : IStaticReferenceHandler
    {
        private StageTerrainDataGroup StageTerrainDataGroup;

        private bool isInitialized = false;

        private void LoadScriptableObject()
        {
            this.StageTerrainDataGroup = Resources.Load<StageTerrainDataGroup>("ScriptableObject/TerrainData/StageTerrainDataGroup");

            this.isInitialized = true;
        }

        // stageID에 해당되는 지형 좌표 데이터를 가져옴.
        public bool TryGetStageTileSapwnDatas(int stageID, out int width, out int height, out List<TerrainData> terrainDatas)
        {
            if (!this.isInitialized) this.LoadScriptableObject();

            width = 10;
            height = 10;
            terrainDatas = new();
            if (this.StageTerrainDataGroup == null) return false;

            // StageID에 해당되는 '맵 가로 / 세로 크기', '맵 지형 데이터 정보가 담긴 JsonFile'을 가져옴.
            if (!this.StageTerrainDataGroup.TryGetStageTerrainData(stageID, out var data)) return false;

            TextAsset tileJson = data.StageTerrainDataJsonFile;

            TerrainDataArrayWrraperGroup terrainDataArrayWrraperGroup = JsonUtility.FromJson<TerrainDataArrayWrraperGroup>(tileJson.text);
            if (terrainDataArrayWrraperGroup == null || terrainDataArrayWrraperGroup.TerrainDatas == null)
            {
                Debug.LogError($"[StageTerrainDataGroup] Json 파싱 실패 - StageID: {stageID}");
                return false;
            }

            width = data.Width;
            height = data.Height;
            terrainDatas = new List<TerrainData>(terrainDataArrayWrraperGroup.TerrainDatas);
            return true;
        }
    }
}