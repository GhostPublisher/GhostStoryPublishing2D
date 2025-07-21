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

        // TerrainData[]가 Json 형태로 저장된 파일입니다.
        public TextAsset StageTerrainDataJsonFile;
    }

    [Serializable]
    public class TerrainData
    {
        public int GridPositionX;
        public int GridPositionY;

        public TerrainType TerrainType;

        public int VisibleBlockWeight;          // 시야 차단 가중치
        public int GroundBlockWeight;           // 이동 차단 가중치 ( Water, Air 도 가능 )

        public int SkillBlockWeight;            // 공격 차단 가중치

        public TerrainData()
        {
            GridPositionX = 0;
            GridPositionY = 0;
            TerrainType = TerrainType.None;
            VisibleBlockWeight = 0;
            GroundBlockWeight = 0;
            SkillBlockWeight = 0;
        }
    }

    [Serializable]
    public enum TerrainType
    {
        None = 0,
    }

    [Serializable]
    public class TerrainDataArrayWrraperGroup
    {
        public TerrainData[] TerrainDatas;
    }
}