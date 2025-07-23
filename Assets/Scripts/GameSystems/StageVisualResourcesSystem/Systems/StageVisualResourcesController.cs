using UnityEngine;

using Foundations.Architecture.ReferencesHandler;

namespace GameSystems.TerrainSystem
{
    public class StageVisualResourcesController : MonoBehaviour
    {
        private StageGroundTilemapDataDBHandler StageGroundTilemapDataDBHandler;

        [SerializeField] private Transform FloorTileMapObjectParent;

        private void Awake()
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            this.StageGroundTilemapDataDBHandler = HandlerManager.GetStaticDataHandler<StageGroundTilemapDataDBHandler>();
        }

        public void InitialSetStageVisualResources(int stageID)
        {
            this.StageGroundTilemapDataDBHandler.TryGetStageVisualResourcesData(stageID, out var data);

            this.GenerateFloorTilemap(data.GroundTileMapPrefab);
        }
        public void GenerateFloorTilemap(GameObject tilemapPrefab)
        {
            Instantiate(tilemapPrefab, this.FloorTileMapObjectParent);
        }

        public void ClearFloorTilemap()
        {
            foreach (Transform child in FloorTileMapObjectParent)
            {
                GameObject.Destroy(child.gameObject);
            }
        }
    }
}