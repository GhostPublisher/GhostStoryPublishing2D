using System.Collections.Generic;
using UnityEngine.EventSystems;

using UnityEngine;
using UnityEngine.Tilemaps;

using Foundations.Architecture.ReferencesHandler;
using Foundations.Architecture.EventObserver;

using GameSystems.PlayerSystem.PlayerUnitSystem;

namespace GameSystems.TilemapSystem.SkillRangeTilemap
{
    public class FilteredSkillRangeTilemapMouseInteractor : MonoBehaviour
    {
        private ISkillRangeTilemapSystem SkillRangeTilemapSystem;

        [SerializeField] private Tilemap FilteredSkillRangeTilemap;
        [SerializeField] private Vector3 mousePositionOffset;

        private int currentPlayerUniqueID;
        private int currentSkillID;
        private HashSet<Vector2Int> skillTargetPositions;

        private bool isActivated = false;
        private Vector2Int currentGridPosition;

        public void InitialSetting(ISkillRangeTilemapSystem SkillRangeTilemapSystem)
        {
            this.SkillRangeTilemapSystem = SkillRangeTilemapSystem;
        }

        private void Update()
        {
            // 활성화 요청이 없었거나, 스킬 상호작용범위 값이 없을 때는 그냥 리턴.
            if (!isActivated || this.skillTargetPositions == null ) return;

            if (EventSystem.current.IsPointerOverGameObject()) return;

            // 마우스 현재 좌표를 Grid 좌표로 변환.
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cellPos = this.FilteredSkillRangeTilemap.WorldToCell(worldPos + mousePositionOffset);
            Vector2Int gridPos = new Vector2Int(cellPos.x, cellPos.y);

            // 좌표가 동일하면 넘어가기.
            if (this.currentGridPosition != gridPos)
            {
                // 좌표가 변경되었을 경우.
                // 이전 좌표 값이 활성화된 상태였을 경우, PointerExit.
                if (this.skillTargetPositions.Contains(currentGridPosition))
                {
                    this.SkillRangeTilemapSystem.DisActivateSkillImpactRangeTilemap();
                }

                // 새로운 좌표 값이 활성화된 상태였을 경우, PointerExit.
                if (this.skillTargetPositions.Contains(gridPos))
                {
                    this.RequestActivateSkillImpactRange_OnMousePointerEnter(gridPos);
                }

                this.currentGridPosition = gridPos;
            }

            if (Input.GetMouseButtonUp(0) && this.skillTargetPositions.Contains(this.currentGridPosition))
            {
                this.RequestOperateSkill_OnMouseClickUpEvent(this.currentGridPosition);
            }

            if (Input.GetMouseButtonUp(1))
            {
                this.RequestSkillRangeTilemapCancelEvent();
            }
        }

        public void ActivateSkillImpactTileMap(int playerUnitID, int skillID, HashSet<Vector2Int> skillTargetPositions)
        {
            this.enabled = true;
            this.isActivated = true;

            this.currentPlayerUniqueID = playerUnitID;
            this.currentSkillID = skillID;
            this.skillTargetPositions = skillTargetPositions;
        }

        public void DisActivateMovementTileMap()
        {
            this.enabled = false;
            this.isActivated = false;


            this.currentPlayerUniqueID = -99;
            this.currentSkillID = -99;
            this.skillTargetPositions = null;

            this.currentGridPosition = new Vector2Int(-99, -99);
        }

        private void RequestActivateSkillImpactRange_OnMousePointerEnter(Vector2Int skillTargetPosition)
        {
            // 특정 지점에 '스킬'이 작동하였을 경우, Impact 범위를 계산 -> Impact Tilemap에 요청. 하는 Notify를 보냄.
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            var PlayerUnitManagerDataDBHandler = HandlerManager.GetDynamicDataHandler<GameSystems.PlayerSystem.PlayerUnitManagerDataDBHandler>();

            // 특정 지점에 Impact 범위 표시.
            if(PlayerUnitManagerDataDBHandler.TryGetPlayerUnitManagerData(this.currentPlayerUniqueID, out var playerUnitManagerData))
            {
                if (playerUnitManagerData.PlayerUnitFeatureInterfaceGroup.PlayerUnitSkillRangeCalculators.TryGetValue(this.currentSkillID, out var playerUnitSkillRangeCalculator))
                {
                    playerUnitSkillRangeCalculator.UpdateSkillImpactRange(skillTargetPosition);
                }
            }
        }

        private void RequestOperateSkill_OnMouseClickUpEvent(Vector2Int skillTargetPosition)
        {
            // UI 필드 멤버 갱신.
            var HandlerManager = LazyReferenceHandlerManager.Instance;
           
            // 특정 지점에 '스킬' 작동.
            var PlayerUnitManagerDataDBHandler = HandlerManager.GetDynamicDataHandler<GameSystems.PlayerSystem.PlayerUnitManagerDataDBHandler>();

            if (PlayerUnitManagerDataDBHandler.TryGetPlayerUnitManagerData(this.currentPlayerUniqueID, out var playerUnitManagerData))
            {
                playerUnitManagerData.PlayerUnitFeatureInterfaceGroup.PlayerUnitManager.OperateSkill(this.currentPlayerUniqueID, this.currentSkillID, skillTargetPosition);
            }
            
            this.SkillRangeTilemapSystem.DisActivateSkillRangeTilemap();
        }

        private void RequestSkillRangeTilemapCancelEvent()
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            var PlayerUnitActionUIUXHandler = HandlerManager.GetDynamicDataHandler<UIUXSystem.UIUXSystemHandler>().PlayerUnitActionUIUXHandler;

            PlayerUnitActionUIUXHandler.IPlayerUnitActionPanelUIMediator.Show_PlayerUnitBehaviourPanel(this.currentPlayerUniqueID);

            this.SkillRangeTilemapSystem.DisActivateSkillRangeTilemap();
        }
    }
}