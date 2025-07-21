using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Tilemaps;

using Foundations.Architecture.ReferencesHandler;

using GameSystems.TerrainSystem;

namespace GameSystems.TilemapSystem.MovementTilemap
{
    public interface IMovementTilemapController
    {
        public void OnPointerEnter(Vector2Int gridPosition);
        public void OnPointerExit(Vector2Int gridPosition);

        public void DisActivateMovementTileMap();
    }

    public class MovementTilemapController : MonoBehaviour, IMovementTilemapController
    {
        [SerializeField] private Tilemap MovementTilemap;

        [Header("Movement Tiles")]
        [SerializeField] private TileBase BaseTile;
        [SerializeField] private TileBase moveableTile;

        [SerializeField] private TileBase MouseEnterTile;       // ��ȣ�ۿ� Tile���� ������.

        private int Width;
        private int Height;

        public void InitialSetting(int stageID)
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            var StageTileSpawnDataDBHandler = HandlerManager.GetStaticDataHandler<StageTerrainDataDBHandler>();

            // ���� ������ 0 x 0 ũ�⸦ ��ȯ����.
            StageTileSpawnDataDBHandler.TryGetStageTerrainSize(stageID, out int width, out int height);

            this.InitialSetting(width, height);
        }
        public void InitialSetting(int width, int height)
        {
            this.Width = width;
            this.Height = height;
        }

        public void ActivateMovementTileMap(Vector2Int currentPosition, HashSet<Vector2Int> moveableRange)
        {
            this.MovementTilemap.enabled = true;

            // ���� ��Ȱ��ȭ.
            for (int x = 0; x < this.Width; x++)
            {
                for (int y = 0; y < this.Height; y++)
                {
                    this.MovementTilemap.SetTile(new Vector3Int(x, y, 0), this.BaseTile);
                }
            }

            // �̵� ���� ���� ǥ��.
            foreach (Vector2Int pos in moveableRange)
            {
                this.MovementTilemap.SetTile(new Vector3Int(pos.x, pos.y, 0), this.moveableTile);
            }

            // �÷��̾� ��ġ�� null ���·�.
            this.MovementTilemap.SetTile(new Vector3Int(currentPosition.x, currentPosition.y, 0), null);  
        }

        public void DisActivateMovementTileMap()
        {
            this.MovementTilemap.ClearAllTiles();
            this.MovementTilemap.enabled = false;
        }

        public void OnPointerEnter(Vector2Int gridPosition)
        {
            this.MovementTilemap.SetTile(new Vector3Int(gridPosition.x, gridPosition.y, 0), this.MouseEnterTile);
        }

        public void OnPointerExit(Vector2Int gridPosition)
        {
            this.MovementTilemap.SetTile(new Vector3Int(gridPosition.x, gridPosition.y, 0), this.moveableTile);
        }
    }
}