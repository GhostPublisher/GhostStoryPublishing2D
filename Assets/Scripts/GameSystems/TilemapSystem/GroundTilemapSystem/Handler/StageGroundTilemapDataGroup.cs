using System;
using System.Collections.Generic;

using UnityEngine;

namespace GameSystems.TerrainSystem
{
    [Serializable]
    [CreateAssetMenu(menuName = "ScriptableObject/TerrainData/StageGroundTileMapDataGroup", fileName = "StageGroundTileMapDataGroup")]
    public class StageGroundTilemapDataGroup : ScriptableObject
    {
        [SerializeField] private List<StageGroundTileMapData> StageGroundTileMapDatas;

        public bool TryGetStageGroundTileMapData(int stageID , out StageGroundTileMapData stageGroundTileMapData)
        {
            stageGroundTileMapData = null;

            foreach (var data in this.StageGroundTileMapDatas)
            {
                if(data.StageID == stageID)
                {
                    stageGroundTileMapData = data;
                    return true;
                }
            }

            return false;
        }
    }

    [Serializable]
    public class StageGroundTileMapData
    {
        public int StageID;

        public GameObject GroundTileMapPrefab;
    }
}
