using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Tilemaps;

using Foundations.Architecture.ReferencesHandler;

using GameSystems.UtilitySystem;

using GameSystems.TerrainSystem;

namespace GameSystems.TilemapSystem.MapVisibilityTilemap
{
    public interface IMapVisibilityTilemapController
    {
        public void InitialSetting(int stageID);
        // Test 수행 용, Private로 존재해도 됨.
        public void InitialSetting(int width, int height);

        public void UpdatePlayerVisibleData_ForFogVisibility(int uniqueID, HashSet<Vector2Int> newVisibleRange);
        public void RemovePlayerVisibleData_ForFogVisibility(int uniqueID);

        public void UpdateScannerVisibleData_ForFogVisibility(int uniqueID, HashSet<Vector2Int> newVisibleRange);
        public void RemoveScannerVisibleData_ForFogVisibility(int uniqueID);

        public void UpdateScannerRawVisibleData_ForFogVisibility(int uniqueID, Vector2Int targetPosition, int visibleSize, int visibleOvercomeWeight);

        public void UpdateFogVisibility();

        public void ClearFogData();

        public void HideAllFog_Test();
        public void ShowAllFog_Test();
    }

    // '시야 기능'을 사용하는 '객체'의 UniqueID와 '시야' 기능을 갖는다.

    public class MapVisibilityTilemapController : MonoBehaviour, IMapVisibilityTilemapController
    {
        [SerializeField] private Tilemap MapVisibilityTilemap;

        [Header("Fog Tiles")]
        [SerializeField] private TileBase HiddenTile;
        [SerializeField] private TileBase RevealedTile;

        private Dictionary<int, HashSet<Vector2Int>> playerUnitVisibilityDatas = new();
        private Dictionary<int, HashSet<Vector2Int>> scannerVisibilityDatas = new();

        private MapVisibilityTilemapData myMapVisibilityTilemapData = new();

        private void Awake()
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            var TilemapDataHandler = HandlerManager.GetDynamicDataHandler<TilemapSystemHandler>();

            TilemapDataHandler.IMapVisibilityTilemapController = this;
            TilemapDataHandler.MapVisibilityTilemapData = this.myMapVisibilityTilemapData;
        }

        public void InitialSetting(int stageID)
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            var StageTileSpawnDataDBHandler = HandlerManager.GetStaticDataHandler<StageTerrainDataDBHandler>();

            // 만약, Stage 정보가 없으면 임시로 0, 0을 리턴해줌.
            StageTileSpawnDataDBHandler.TryGetStageTerrainSize(stageID, out int width, out int height);
            this.InitialSetting(width, height);
        }
        public void InitialSetting(int width, int height)
        {
            this.myMapVisibilityTilemapData.InitialSetting(width, height);

            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    this.MapVisibilityTilemap.SetTile(new Vector3Int(x, y, 0), this.HiddenTile);
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
            var length = this.myMapVisibilityTilemapData.GetLength();

            // 전부 비활성화.
            for (int x = 0; x < length.Item1; ++x)
            {
                for (int y = 0; y < length.Item2; ++y)
                {
                    // false이면 해당 좌표는 범위 밖인 거임.
                    if (!this.myMapVisibilityTilemapData.TryGetFogState(x, y, out FogState fogState)) continue;

                    // 이 경우 말고 다 Revealed 한 상태로 만듬.
                    if (fogState == FogState.Hidden) continue;

                    this.myMapVisibilityTilemapData.SetFogState(x, y, FogState.Revealed);
                    this.MapVisibilityTilemap.SetTile(new Vector3Int(x, y, 0), this.RevealedTile);
                }
            }

            // Player 시야 Visible 처리
            foreach (var list in playerUnitVisibilityDatas.Values)
            {
                foreach(var pos in list)
                {
                    // false이면 해당 좌표는 범위 밖인 거임.
                    if (!this.myMapVisibilityTilemapData.TryGetFogState(pos.x, pos.y, out FogState fogState)) continue;

                    this.myMapVisibilityTilemapData.SetFogState(pos.x, pos.y, FogState.Visible);
                    this.MapVisibilityTilemap.SetTile(new Vector3Int(pos.x, pos.y, 0), null);
                }
            }

            // 3. Visible 처리
            foreach (var list in scannerVisibilityDatas.Values)
            {
                foreach (var pos in list)
                {
                    // false이면 해당 좌표는 범위 밖인 거임.
                    if (!this.myMapVisibilityTilemapData.TryGetFogState(pos.x, pos.y, out FogState fogState)) continue;

                    this.myMapVisibilityTilemapData.SetFogState(pos.x, pos.y, FogState.Visible);
                    this.MapVisibilityTilemap.SetTile(new Vector3Int(pos.x, pos.y, 0), null);
                }
            }


        }

        public void ClearFogData()
        {
            if (this.myMapVisibilityTilemapData == null) return;

            this.myMapVisibilityTilemapData.ClearFogTilemapData();
            this.myMapVisibilityTilemapData = null;

            this.MapVisibilityTilemap.ClearAllTiles();
        }

        public void HideAllFog_Test()
        {
            // 전부 비활성화.
            this.MapVisibilityTilemap.ClearAllTiles();
        }
        public void ShowAllFog_Test()
        {
            var length = this.myMapVisibilityTilemapData.GetLength();

            // 전부 활성화.
            for (int x = 0; x < length.Item1; ++x)
            {
                for (int y = 0; y < length.Item2; ++y)
                {
                    // false이면 해당 좌표는 범위 밖인 거임.
                    if (!this.myMapVisibilityTilemapData.TryGetFogState(x, y, out FogState fogState)) continue;

                    // 이 경우 말고 다 Revealed 한 상태로 만듬.
                    if (fogState == FogState.Hidden)
                        this.MapVisibilityTilemap.SetTile(new Vector3Int(x, y, 0), this.HiddenTile);
                    else if (fogState == FogState.Revealed)
                        this.MapVisibilityTilemap.SetTile(new Vector3Int(x, y, 0), this.RevealedTile);
                    else { }
                }
            }
        }
    }
}
