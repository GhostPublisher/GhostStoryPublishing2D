using System.Collections.Generic;

using UnityEngine;

using Foundations.Architecture.ReferencesHandler;

namespace GameSystems.TilemapSystem.SkillRangeTilemap
{
    public interface ISkillRangeTilemapSystem
    {
        public void InitialSetting();


        public void ActivateFilteredSkillRangeTilemap(int playerUnitID, int skillID, Vector2Int currentPosition, HashSet<Vector2Int> filteredSkillRange, HashSet<Vector2Int> skillTargetPositions);
        public void ActiavteSkillImpactRangeTilemap(Vector2Int mainTargetPosition, HashSet<Vector2Int> filteredSkillImpactRange, HashSet<Vector2Int> additionalTargetPositions);
    }

    public class SkillRangeTilemapSystem : MonoBehaviour, ISkillRangeTilemapSystem
    {
        [SerializeField] private FilteredSkillRangeTilemapController FilteredSkillRangeTilemapController;
        [SerializeField] private SkillImpactRangeTilemapController SkillImpactRangeTilemapController;

        [SerializeField] private SkillRangeTilemapMouseInteractor SkillRangeTilemapMouseInteractor;

        private void Awake()
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            var TilemapSystemHandler = HandlerManager.GetDynamicDataHandler<TilemapSystemHandler>();

            TilemapSystemHandler.ISkillRangeTilemapSystem = this;
        }

        public void InitialSetting()
        {
            this.FilteredSkillRangeTilemapController.InitialSetting();
            this.SkillImpactRangeTilemapController.InitialSetting();

            this.SkillRangeTilemapMouseInteractor.InitialSetting(this.FilteredSkillRangeTilemapController, this.SkillImpactRangeTilemapController);
        }
        public void ActivateFilteredSkillRangeTilemap(int playerUnitID, int skillID,  Vector2Int currentPosition, HashSet<Vector2Int> filteredSkillRange, HashSet<Vector2Int> skillTargetPositions)
        {
            this.FilteredSkillRangeTilemapController.ActivateFilteredSkillRangeTileMap(currentPosition, filteredSkillRange, skillTargetPositions);

            // 마우스 상호작용 활성화 및 마우스 상호작용 조건 설정.
            this.SkillRangeTilemapMouseInteractor.ActivateMouseInteraction(playerUnitID, skillID, skillTargetPositions);
        }

        public void ActiavteSkillImpactRangeTilemap(Vector2Int mainTargetPosition, HashSet<Vector2Int> filteredSkillImpactRange, HashSet<Vector2Int> additionalTargetPositions)
        {
            this.SkillImpactRangeTilemapController.ActivateSkillImpactRangeTilemap(mainTargetPosition, filteredSkillImpactRange, additionalTargetPositions);
        }
    }
}