using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Tilemaps;


namespace GameSystems.TilemapSystem.MovementTilemap
{
    public interface IMovementTilemapSystem
    {
        public void DisActivateMovementTilemap();

        public void OnPointerEnter(Vector2Int gridPosition);
        public void OnPointerExit(Vector2Int gridPosition);
    }

    public class MovementTilemapSystem : MonoBehaviour, IMovementTilemapSystem
    {
        [SerializeField] private MovementTilemapMouseInteractor MovementTilemapMouseInteractor;
        [SerializeField] private MovementTilemapController MovementTilemapController;

        [SerializeField] private TilemapRenderer TilemapRenderer;

        private void Awake()
        {
            this.MovementTilemapMouseInteractor.InitialSetting(this);
        }

        public void InitialSetting(int stageID)
        {
            this.MovementTilemapController.InitialSetting(stageID);
        }
        public void InitialSetting(int width, int height)
        {
            this.MovementTilemapController.InitialSetting(width, height);
        }

        public void ActivateMovementTilemap(int playerUniqueID, Vector2Int currentPosition, HashSet<Vector2Int> moveableRange)
        {
            this.MovementTilemapMouseInteractor.ActivateMovementTileMap(playerUniqueID, moveableRange);
            this.MovementTilemapController.ActivateMovementTileMap(currentPosition, moveableRange);
        }

        public void DisActivateMovementTilemap()
        {
            this.MovementTilemapMouseInteractor.DisActivateMovementTileMap();
            this.MovementTilemapController.DisActivateMovementTileMap();
        }

        public void OnPointerEnter(Vector2Int gridPosition)
        {
            this.MovementTilemapController.OnPointerEnter(gridPosition);
        }

        public void OnPointerExit(Vector2Int gridPosition)
        {
            this.MovementTilemapController.OnPointerExit(gridPosition);
        }
    }
}