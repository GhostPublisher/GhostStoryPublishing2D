
using UnityEngine;

using Foundations.Architecture.ReferencesHandler;


namespace GameSystems.TilemapSystem.MovementTilemap
{
    // 이건 이동을 수행할 때, '시야'에 없는 지역으로도 이동할 수 있도록 할지 안할지 등에 대한 Filtering 부분이다.
    // 이거 상의 후에 하는게 좋을 듯.
    public class MovementTilemapDataFilter : MonoBehaviour
    {
        private TilemapDataGroupHandler TilemapDataGroupHandler;

        private void Awake()
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            this.TilemapDataGroupHandler = HandlerManager.GetDynamicDataHandler<TilemapDataGroupHandler>();
        }

/*        public HashSet<Vector2Int> OperateMoveableRangeFiltering(HashSet<Vector2Int> moveableRange)
        {
            HashSet<Vector2Int> filteredMoveableRange = new();

            foreach (Vector2Int pos in moveableRange)
            {
                if (this.TilemapDataGroupHandler.FogTilemapData.TryGetFogState(pos.x, pos.y, out FogTilemap.FogState fogState)
                    fogState == FogTilemap.FogState.) continue;


            }
        }*/
    }
}