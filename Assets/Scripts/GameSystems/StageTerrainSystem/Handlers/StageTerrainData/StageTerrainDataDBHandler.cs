using System;
using System.Collections.Generic;

using UnityEngine;

using Foundations.Architecture.ReferencesHandler;

namespace GameSystems.TerrainSystem
{
    [Serializable]
    public class StageTerrainDataDBHandler : IStaticReferenceHandler
    {
        // 해당 위치에 '지형 데이터 DB 역할'을 하는 ScriptableObject( == SO)가 존재합니다.
        private string StageTerrainDataDBScriptableObjectPath = "ScriptableObject/TerrainData/StageTerrainDataGroup";

        // SO 로드시도.
        private bool TryLoadScriptableObject(out StageTerrainDataGroup stageTerrainDataGroup)
        {
            stageTerrainDataGroup = Resources.Load<StageTerrainDataGroup>(this.StageTerrainDataDBScriptableObjectPath);

            // 파일 못찾으면 false 리턴.
            if (stageTerrainDataGroup == null) return false;
            return true;
        }

        // stageID에 해당되는 지형 좌표 데이터를 가져옴.
        public bool TryGetStageTileSapwnDatas(int stageID, out int width, out int height, out List<TerrainData> terrainDatas)
        {
            // SO를 못찾을 경우 false 리턴.
            if (!this.TryLoadScriptableObject(out StageTerrainDataGroup stageTerrainDataGroup))
            {
                Debug.LogError($"[StageTerrainDataDBHandler] SO 위치 못 찾음.");
                width = 0;
                height = 0;
                terrainDatas = null;
                return false;
            }

            // SO 안에 StageID에 해당되는 데이터가 없을 경우 false 리턴.
            if (!stageTerrainDataGroup.TryGetStageTerrainData(stageID, out var stageTerrainData))
            {
                Debug.LogError($"[StageTerrainDataDBHandler] SO 안에 StageID 해당 데이터가 없음. StageID : {stageID}");
                width = 0;
                height = 0;
                terrainDatas = null;
                return false;
            }

            // SO 안에 StageID에 해당되는 데이터는 넣었는데, Json 파일을 누락했을 경우.
            if (stageTerrainData.StageTerrainDataJsonFile == null)
            {
                Debug.LogError($"[StageTerrainDataDBHandler] JsonFile이 비어 있음. StageID : {stageID}");
                width = 0;
                height = 0;
                terrainDatas = null;
                return false;
            }

            // StageID에 해당되는 '맵 지형 데이터 정보가 담긴 JsonFile'을 가져와서 Parsing 작업.
            var terrainDataArrayWrraperGroup = JsonUtility.FromJson<TerrainDataArrayWrraperGroup>(stageTerrainData.StageTerrainDataJsonFile.text);
            if (terrainDataArrayWrraperGroup == null || terrainDataArrayWrraperGroup.TerrainDatas == null)
            {
                Debug.LogError($"[StageTerrainDataDBHandler] Json 파싱 실패. StageID: {stageID}");
                width = 0;
                height = 0;
                terrainDatas = null;
                return false;
            }

            width = stageTerrainData.Width;
            height = stageTerrainData.Height;
            terrainDatas = new List<TerrainData>(terrainDataArrayWrraperGroup.TerrainDatas);
            return true;
        }

        // StageID에 해당되는 맵 사이즈를 가져옴.
        public bool TryGetStageTerrainSize(int stageID, out int width, out int height)
        {
            // SO를 못찾을 경우 false 리턴.
            if (!this.TryLoadScriptableObject(out StageTerrainDataGroup stageTerrainDataGroup))
            {
                Debug.LogError($"[StageTerrainDataDBHandler] SO 위치 못 찾음.");
                width = 0;
                height = 0;
                return false;
            }

            // SO 안에 StageID에 해당되는 데이터가 없을 경우 false 리턴.
            if (!stageTerrainDataGroup.TryGetStageTerrainData(stageID, out var stageTerrainData))
            {
                Debug.LogError($"[StageTerrainDataDBHandler] SO 안에 StageID 해당 데이터가 없음. StageID : {stageID}");
                width = 0;
                height = 0;
                return false;
            }

            width = stageTerrainData.Width;
            height = stageTerrainData.Height;
            return true;
        }
    }
}