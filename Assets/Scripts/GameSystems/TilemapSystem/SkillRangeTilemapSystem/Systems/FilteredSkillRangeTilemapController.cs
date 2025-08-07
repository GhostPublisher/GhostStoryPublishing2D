using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Tilemaps;

namespace GameSystems.TilemapSystem.SkillRangeTilemap
{
    public interface IFilteredSkillRangeTilemapController
    {
        public void InitialSetting();
        public void ActivateFilteredSkillRangeTileMap(Vector2Int currentPosition, HashSet<Vector2Int> filteredSkillRange, HashSet<Vector2Int> SkillTargetPositions);
        public void DisActivateFilteredSkillRangeTilemap();
    }

    public class FilteredSkillRangeTilemapController : MonoBehaviour, IFilteredSkillRangeTilemapController
    {
        [SerializeField] private Tilemap FilteredSkillRangeTilemap;

        [Header("FilteredSkillRange Tiles")]
        [SerializeField] private TileBase BaseTile;
        [SerializeField] private TileBase currentPositionTile;
        [SerializeField] private TileBase SkillTargetTile;  

        public void InitialSetting()
        {
            // 전부 비활성화.
            this.FilteredSkillRangeTilemap.ClearAllTiles();
        }

        public void ActivateFilteredSkillRangeTileMap(Vector2Int currentPosition, HashSet<Vector2Int> filteredSkillRange, HashSet<Vector2Int> SkillTargetPositions)
        {
            this.FilteredSkillRangeTilemap.enabled = true;

            // 전부 비활성화.
            this.FilteredSkillRangeTilemap.ClearAllTiles();

            // '기본 스킬 범위 - 장애물 범위 - 장애물에 의해 가려지는 범위'
            foreach (Vector2Int pos in filteredSkillRange)
            {
                this.FilteredSkillRangeTilemap.SetTile(new Vector3Int(pos.x, pos.y, 0), this.BaseTile);
            }

            // 플레이어 위치는 null 상태로.
            this.FilteredSkillRangeTilemap.SetTile(new Vector3Int(currentPosition.x, currentPosition.y, 0), null);

            // '실질적으로 적용 가능한 SkillTarget 좌표들'
            foreach (Vector2Int pos in SkillTargetPositions)
            {
                this.FilteredSkillRangeTilemap.SetTile(new Vector3Int(pos.x, pos.y, 0), this.SkillTargetTile);
            }
        }

        public void DisActivateFilteredSkillRangeTilemap()
        {
            this.FilteredSkillRangeTilemap.ClearAllTiles();
            this.FilteredSkillRangeTilemap.enabled = false;
        }
    }
}