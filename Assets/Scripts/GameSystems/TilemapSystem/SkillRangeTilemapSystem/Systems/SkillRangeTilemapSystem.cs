using System;
using System.Collections.Generic;

using UnityEngine;

namespace GameSystems.TilemapSystem.SkillRangeTilemap
{
    public interface ISkillRangeTilemapSystem
    {
        public void DisActivateSkillRangeTilemap();
        public void DisActivateSkillImpactRangeTilemap();
    }

    public class SkillRangeTilemapSystem : MonoBehaviour, ISkillRangeTilemapSystem
    {
        [SerializeField] private FilteredSkillRangeTilemapController FilteredSkillRangeTilemapController;
        [SerializeField] private FilteredSkillRangeTilemapMouseInteractor FilteredSkillRangeTilemapMouseInteractor;

        [SerializeField] private SkillImpactRangeTilemapController SkillImpactRangeTilemapController;

        private void Awake()
        {
            this.FilteredSkillRangeTilemapMouseInteractor.InitialSetting(this);
        }

        public void InitialSetting(int stageID)
        {
            this.FilteredSkillRangeTilemapController.InitialSetting(stageID);
        }
        public void InitialSetting(int width, int height)
        {
            this.FilteredSkillRangeTilemapController.InitialSetting(width, height);
        }

        public void ActivateFilteredSkillRangeTilemap(int playerUnitID, int skillID,  Vector2Int currentPosition, HashSet<Vector2Int> filteredSkillRange, HashSet<Vector2Int> skillTargetPositions)
        {
            this.FilteredSkillRangeTilemapController.ActivateFilteredSkillRangeTileMap(currentPosition, filteredSkillRange, skillTargetPositions);
            this.FilteredSkillRangeTilemapMouseInteractor.ActivateSkillImpactTileMap(playerUnitID, skillID, skillTargetPositions);
        }

        public void ActiavteSkillImpactRangeTilemap(Vector2Int mainTargetPosition, HashSet<Vector2Int> filteredSkillImpactRange, HashSet<Vector2Int> additionalTargetPositions)
        {
            this.SkillImpactRangeTilemapController.ActivateSkillImpactRangeTilemap(mainTargetPosition, filteredSkillImpactRange, additionalTargetPositions);
        }

        public void DisActivateSkillRangeTilemap()
        {
            this.FilteredSkillRangeTilemapController.DisActivateFilteredSkillRangeTilemap();
            this.FilteredSkillRangeTilemapMouseInteractor.DisActivateMovementTileMap();

            this.SkillImpactRangeTilemapController.DisActivateSkillImpactRangeTilemap();
        }

        public void DisActivateSkillImpactRangeTilemap()
        {
            this.SkillImpactRangeTilemapController.DisActivateSkillImpactRangeTilemap();
        }
    }
}