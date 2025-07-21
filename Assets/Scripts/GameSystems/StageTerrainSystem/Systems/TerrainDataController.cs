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

        // StageID를 통한 지형 데이터 설정.
        // GeneratedTerrainDataDBHandler 필드 멤버 정의, 데이터 할당.
        public void InitialSetGeneratedTerrainData(int stageID)
        {
            // StageID를 통해서, 지형 데이터 Data를 가져옵니다.
            if (!this.StageTerrainDataDBHandler.TryGetStageTileSapwnDatas(stageID, out int width, out int height, out var terrainDatas)) return;

            // 배열 크기 정의.
            this.myGeneratedTerrainDataDBHandler.InitialSetting(width, height);

            foreach (var terrainData in terrainDatas)
            {
                GeneratedTerrainData newGeneratedTerrainData = new();
                newGeneratedTerrainData.GridPosition = new Vector2Int(terrainData.GridPositionX, terrainData.GridPositionY);
                newGeneratedTerrainData.TerrainData = terrainData;

                this.myGeneratedTerrainDataDBHandler.AddGeneratedTerrainData(newGeneratedTerrainData);
            }
        }
        public void ClearTrerrainData()
        {
            this.myGeneratedTerrainDataDBHandler.ClearGeneratedTerrainDataGroup();
        }

        // Test를 위해서 모든 좌표를 Default로 지정합니다.
        public void InitialSetGeneratedTerrainData_Test(int width, int height)
        {
            // 배열 크기 정의.
            this.myGeneratedTerrainDataDBHandler.InitialSetting(width, height);

            for(int x = 0; x < width; ++x)
            {
                for(int y = 0; y < height; ++y)
                {
                    TerrainData terrainData = new();
                    terrainData.GridPositionX = x;
                    terrainData.GridPositionY = y;

                    GeneratedTerrainData newGeneratedTerrainData = new();
                    newGeneratedTerrainData.GridPosition = new Vector2Int(x, y);
                    newGeneratedTerrainData.TerrainData = terrainData;

                    this.myGeneratedTerrainDataDBHandler.AddGeneratedTerrainData(newGeneratedTerrainData);
                }
            }
        }
        // Test를 위해서 특정 좌표에 특정 값을 덮어쓰는 유형입니다.
        public void OverwriteSetGeneratedTerrainData_Test(List<TerrainData> terrainDatas)
        {
            foreach (var data in terrainDatas)
            {
                if(this.myGeneratedTerrainDataDBHandler.TryGetGeneratedTerrainData(new Vector2Int(data.GridPositionX, data.GridPositionY), out var terrainData))
                {
                    terrainData.TerrainData = data;
                }
            }
        }
    }
}