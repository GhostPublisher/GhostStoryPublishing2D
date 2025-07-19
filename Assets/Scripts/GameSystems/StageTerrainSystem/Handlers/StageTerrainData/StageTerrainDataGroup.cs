using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameSystems.TerrainSystem
{
    // 각 스테이지 별, Tile 정보.
    [Serializable]
    [CreateAssetMenu(menuName = "ScriptableObject/TerrainData/StageTerrainDataGroup", fileName = "StageTerrainDataGroup")]
    public class StageTerrainDataGroup : ScriptableObject
    {
        [SerializeField] private List<StageTerrainData> stageTerrainDatas;

        public bool TryGetStageTerrainData(int stageID, out StageTerrainData stageTerrainData)
        {
            stageTerrainData = null;
            if (this.stageTerrainDatas == null) return false;

            foreach(var data in this.stageTerrainDatas)
            {
                if(data.StageID == stageID)
                {
                    stageTerrainData = data;
                    return true;
                }
            }
            return false;
        }
    }

    [Serializable]
    public class StageTerrainData
    {
        public int StageID;

        public int Width;
        public int Height;

        public TextAsset StageTerrainDataJsonFile;
    }

    [Serializable]
    public class TerrainData
    {
        public int GridPositionX;
        public int GridPositionY;

        public int VisibleBlockWeight;          // 시야 차단 가중치
        public int GroundBlockWeight;             // 이동 차단 가중치 ( Water, Air 도 가능 )

        public int CommandBlockWeight;          // 명령 차단 가중치
        public int SkillBlockWeight;            // 공격 차단 가중치
    }

    [Serializable]
    public class TerrainDataArrayWrraperGroup
    {
        public TerrainData[] TerrainDatas;
    }
}