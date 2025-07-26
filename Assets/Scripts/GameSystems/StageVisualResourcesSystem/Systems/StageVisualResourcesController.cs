using UnityEngine;

using Foundations.Architecture.ReferencesHandler;

namespace GameSystems.StageVisualSystem
{
    public class StageVisualResourcesController : MonoBehaviour
    {
        private StageVisualResourcesDataDBHandler StageGroundTilemapDataDBHandler;

        [SerializeField] private Transform GridTransformParent;

        private void Awake()
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            this.StageGroundTilemapDataDBHandler = HandlerManager.GetStaticDataHandler<StageVisualResourcesDataDBHandler>();
        }

        public void InitialSetStageVisualResources(int stageID)
        {
            this.StageGroundTilemapDataDBHandler.TryGetStageVisualResourcesData(stageID, out var data);

            this.GenerateVisualResourcesTilemap(data);
        }
        public void GenerateVisualResourcesTilemap(StageVisualResourcesData stageVisualResourcesData)
        {
            if(stageVisualResourcesData.GroundTileMapPrefab != null)
                Instantiate(stageVisualResourcesData.GroundTileMapPrefab, this.GridTransformParent);

            if (stageVisualResourcesData.StructureTileMapPrefab != null)
                Instantiate(stageVisualResourcesData.StructureTileMapPrefab, this.GridTransformParent);
        }

        public void ClearFloorTilemap()
        {
            foreach (Transform child in GridTransformParent)
            {
                GameObject.Destroy(child.gameObject);
            }
        }
    }
}