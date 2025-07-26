using System;
using System.Collections.Generic;

using UnityEngine;

namespace GameSystems.StageVisualSystem
{
    [Serializable]
    [CreateAssetMenu(menuName = "ScriptableObject/StageVisualResources/StageVisualResourcesDataSO", fileName = "StageVisualResourcesDataSO")]
    public class StageVisualResourcesDataSO : ScriptableObject
    {
        [SerializeField] private List<StageVisualResourcesData> StageVisualResourcesDatas;

        public bool TryGetStageVisualResourcesData(int stageID , out StageVisualResourcesData stageVisualResourcesData)
        {
            stageVisualResourcesData = null;

            foreach (var data in this.StageVisualResourcesDatas)
            {
                if(data.StageID == stageID)
                {
                    stageVisualResourcesData = data;
                    return true;
                }
            }

            return false;
        }
    }

    [Serializable]
    public class StageVisualResourcesData
    {
        public int StageID;

        public GameObject GroundTileMapPrefab;
        public GameObject StructureTileMapPrefab;
    }
}
