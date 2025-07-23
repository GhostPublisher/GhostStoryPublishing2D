using System;

using UnityEngine;

using Foundations.Architecture.ReferencesHandler;

namespace GameSystems.TerrainSystem
{
    [Serializable]
    public class StageGroundTilemapDataDBHandler : IStaticReferenceHandler
    {
        private string StageVisualResourcesDataSOPath = "ScriptableObject/StageVisualResources/StageVisualResourcesDataSO";

        // SO 로드시도.
        private bool TryLoadScriptableObject(out StageVisualResourcesDataSO stageVisualResourcesDataSO)
        {
            stageVisualResourcesDataSO = Resources.Load<StageVisualResourcesDataSO>(this.StageVisualResourcesDataSOPath);

            // 파일 못찾으면 false 리턴.
            if (stageVisualResourcesDataSO == null) return false;
            return true;
        }


        // stageID에 해당되는 VisualResoucesData를 가져옴. ( Floor 지형 이미지들 포함 )      
        public bool TryGetStageVisualResourcesData(int stageID, out StageVisualResourcesData stageVisualResourcesData)
        {
            // SO를 못찾을 경우 false 리턴.
            if (!this.TryLoadScriptableObject(out StageVisualResourcesDataSO stageVisualResourcesDataSO))
            {
                Debug.LogError($"[StageGroundTilemapDataDBHandler] SO 위치 못 찾음.");
                stageVisualResourcesData = null;
                return false;
            }

            // SO 안에 StageID에 해당되는 데이터가 없을 경우 false 리턴.
            if (!stageVisualResourcesDataSO.TryGetStageVisualResourcesData(stageID, out stageVisualResourcesData))
            {
                Debug.LogError($"[StageGroundTilemapDataDBHandler] SO 안에 StageID 해당 데이터가 없음. StageID : {stageID}");
                stageVisualResourcesData = null;
                return false;
            }

            return true;
        }
    }
}
