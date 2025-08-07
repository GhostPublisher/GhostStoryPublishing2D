using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Tilemaps;

namespace GameSystems.TilemapSystem.SkillRangeTilemap
{
    public interface ISkillImpactRangeTilemapController
    {
        public void InitialSetting();
        public void ActivateSkillImpactRangeTilemap(Vector2Int mainTargetPosition, HashSet<Vector2Int> filteredSkillImpactRange, HashSet<Vector2Int> additionalTargetPositions);
        public void DisActivateSkillImpactRangeTilemap();
    }

    public class SkillImpactRangeTilemapController : MonoBehaviour, ISkillImpactRangeTilemapController
    {
        [SerializeField] private Tilemap SkillImpactRangeTilemap;

        [Header("SkillImpact Tiles")]
        [SerializeField] private TileBase MainTargetTile;
        [SerializeField] private TileBase ImpactRangeTile;
        [SerializeField] private TileBase AdditionalTargetTile;

        public void InitialSetting()
        {
            // 전부 비활성화.
            this.SkillImpactRangeTilemap.ClearAllTiles();
        }

        public void ActivateSkillImpactRangeTilemap(Vector2Int mainTargetPosition, HashSet<Vector2Int> filteredSkillImpactRange, HashSet<Vector2Int> additionalTargetPositions)
        {
            this.SkillImpactRangeTilemap.enabled = true;

            // 전부 비활성화.
            this.SkillImpactRangeTilemap.ClearAllTiles();

            // '기본 스킬 Impact 범위 - 장애물 범위 - 장애물에 의해 가려지는 범위'
            foreach (Vector2Int pos in filteredSkillImpactRange)
            {
                this.SkillImpactRangeTilemap.SetTile(new Vector3Int(pos.x, pos.y, 0), this.ImpactRangeTile);
            }

            // 플레이어 위치는 MainTargetTile 상태로.
            this.SkillImpactRangeTilemap.SetTile(new Vector3Int(mainTargetPosition.x, mainTargetPosition.y, 0), this.MainTargetTile);

            // '실질적으로 적용 가능한 Target 좌표들'
            foreach (Vector2Int pos in additionalTargetPositions)
            {
                this.SkillImpactRangeTilemap.SetTile(new Vector3Int(pos.x, pos.y, 0), this.AdditionalTargetTile);
            }
        }

        public void DisActivateSkillImpactRangeTilemap()
        {
            this.SkillImpactRangeTilemap.ClearAllTiles();

            this.SkillImpactRangeTilemap.enabled = false;
        }
    }
}