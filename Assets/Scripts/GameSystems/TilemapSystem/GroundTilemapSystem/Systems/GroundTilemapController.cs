using UnityEngine;

using Foundations.Architecture.ReferencesHandler;

namespace GameSystems.TerrainSystem
{
    public class GroundTilemapController : MonoBehaviour
    {
        private StageGroundTilemapDataDBHandler StageGroundTilemapDataDBHandler;

        [SerializeField] private Transform GroundTileMapObjectParent;

        private void Awake()
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            this.StageGroundTilemapDataDBHandler = HandlerManager.GetStaticDataHandler<StageGroundTilemapDataDBHandler>();
        }

        public void InitialSetGroundTilemap(int stageID)
        {
            this.StageGroundTilemapDataDBHandler.TryGetStageGroundTileMapData(stageID, out var data);

            this.GenerateGroundTilemap(data.GroundTileMapPrefab);
        }
        public void GenerateGroundTilemap(GameObject tilemapPrefab)
        {
            Instantiate(tilemapPrefab, this.GroundTileMapObjectParent);
        }

        public void ClearGroundTilemap()
        {
            foreach (Transform child in GroundTileMapObjectParent)
            {
                GameObject.Destroy(child.gameObject);
            }
        }
    }
}