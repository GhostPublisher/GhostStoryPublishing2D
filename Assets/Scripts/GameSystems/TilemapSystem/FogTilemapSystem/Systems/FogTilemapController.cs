using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Tilemaps;

using Foundations.Architecture.ReferencesHandler;

using GameSystems.UtilitySystem;

using GameSystems.TerrainSystem;

namespace GameSystems.TilemapSystem.FogTilemap
{
    public class FogTilemapController : MonoBehaviour
    {
        [SerializeField] private Tilemap FogTilemap;

        [Header("Fog Tiles")]
        [SerializeField] private TileBase HiddenTile;
        [SerializeField] private TileBase RevealedTile;

        private Dictionary<int, HashSet<Vector2Int>> playerUnitVisibilityDatas = new();
        private Dictionary<int, HashSet<Vector2Int>> scannerVisibilityDatas = new();

        private FogTilemapData myFogTilemapData = null;

        public void InitialSetting(int stageID)
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            var StageTileSpawnDataDBHandler = HandlerManager.GetStaticDataHandler<StageTerrainDataDBHandler>();

            // 만약, Stage 정보가 없으면 임시로 10, 10을 리턴해줌.
            StageTileSpawnDataDBHandler.TryGetStageTileSapwnDatas(stageID, out int width, out int height, out var _);

            this.InitialSetting(width, height);
        }
        public void InitialSetting(int width, int height)
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            var TilemapDataGroup = HandlerManager.GetDynamicDataHandler<TilemapDataGroupHandler>();

            this.myFogTilemapData = new FogTilemapData(width, height);
            TilemapDataGroup.FogTilemapData = this.myFogTilemapData;

            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    this.FogTilemap.SetTile(new Vector3Int(x, y, 0), this.HiddenTile);
                }
            }
        }

        // Player Unit의 시야 갱신
        public void UpdatePlayerVisibleData_ForFogVisibility(int uniqueID, HashSet<Vector2Int> newVisibleRange)
        {
            this.playerUnitVisibilityDatas[uniqueID] = newVisibleRange;
        }
        public void RemovePlayerVisibleData_ForFogVisibility(int uniqueID)
        {
            this.playerUnitVisibilityDatas.Remove(uniqueID);
        }

        // Scanner의 시야 갱신
        public void UpdateScannerVisibleData_ForFogVisibility(int uniqueID, HashSet<Vector2Int> newVisibleRange)
        {
            this.scannerVisibilityDatas[uniqueID] = newVisibleRange;
        }
        public void RemoveScannerVisibleData_ForFogVisibility(int uniqueID)
        {
            this.scannerVisibilityDatas.Remove(uniqueID);
        }
        // Scanner의 Raw 시야 갱신
        public void UpdateScannerRawVisibleData_ForFogVisibility(int uniqueID, Vector2Int targetPosition, int visibleSize, int visibleOvercomeWeight)
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            var FogVisibleRangeCalculator = HandlerManager.GetUtilityHandler<VisibilityRangeCalculator>();

            var newVisibleRange = FogVisibleRangeCalculator.GetFilteredVisibleRange_Player(targetPosition, visibleSize, visibleOvercomeWeight);
            this.scannerVisibilityDatas[uniqueID] = newVisibleRange;
        }

        public void UpdateFogVisibility()
        {
            var length = this.myFogTilemapData.GetLength();

            // 전부 비활성화.
            for (int x = 0; x < length.Item1; ++x)
            {
                for (int y = 0; y < length.Item2; ++y)
                {
                    // false이면 해당 좌표는 범위 밖인 거임.
                    if (!this.myFogTilemapData.TryGetFogState(x, y, out FogState fogState)) continue;

                    // 이 경우 말고 다 Revealed 한 상태로 만듬.
                    if (fogState == FogState.Hidden) continue;

                    this.myFogTilemapData.SetFogState(x, y, FogState.Revealed);
                    this.FogTilemap.SetTile(new Vector3Int(x, y, 0), this.RevealedTile);
                }
            }

            // 3. Visible 처리
            foreach (var list in playerUnitVisibilityDatas.Values)
            {
                foreach(var pos in list)
                {
                    // false이면 해당 좌표는 범위 밖인 거임.
                    if (!this.myFogTilemapData.TryGetFogState(pos.x, pos.y, out FogState fogState)) continue;

                    this.myFogTilemapData.SetFogState(pos.x, pos.y, FogState.Visible);
                    FogTilemap.SetTile(new Vector3Int(pos.x, pos.y, 0), null);
                }
            }

            // 3. Visible 처리
            foreach (var list in scannerVisibilityDatas.Values)
            {
                foreach (var pos in list)
                {
                    // false이면 해당 좌표는 범위 밖인 거임.
                    if (!this.myFogTilemapData.TryGetFogState(pos.x, pos.y, out FogState fogState)) continue;

                    this.myFogTilemapData.SetFogState(pos.x, pos.y, FogState.Visible);
                    FogTilemap.SetTile(new Vector3Int(pos.x, pos.y, 0), null);
                }
            }


        }

        public void ClearFogData()
        {
            if (this.myFogTilemapData == null) return;

            this.myFogTilemapData.ClearFogTilemapData();
            this.myFogTilemapData = null;

            this.FogTilemap.ClearAllTiles();
        }

        public void HideAllFog()
        {
            // 전부 비활성화.
            this.FogTilemap.ClearAllTiles();
        }
        public void ShowAllFog()
        {
            var length = this.myFogTilemapData.GetLength();

            // 전부 활성화.
            for (int x = 0; x < length.Item1; ++x)
            {
                for (int y = 0; y < length.Item2; ++y)
                {
                    // false이면 해당 좌표는 범위 밖인 거임.
                    if (!this.myFogTilemapData.TryGetFogState(x, y, out FogState fogState)) continue;

                    // 이 경우 말고 다 Revealed 한 상태로 만듬.
                    if (fogState == FogState.Hidden)
                        this.FogTilemap.SetTile(new Vector3Int(x, y, 0), this.HiddenTile);
                    else if (fogState == FogState.Revealed)
                        this.FogTilemap.SetTile(new Vector3Int(x, y, 0), this.RevealedTile);
                    else { }
                }
            }
        }
    }
}
