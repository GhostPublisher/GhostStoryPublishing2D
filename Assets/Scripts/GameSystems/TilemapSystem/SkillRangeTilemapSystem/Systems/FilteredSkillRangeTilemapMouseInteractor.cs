using System.Collections.Generic;
using UnityEngine.EventSystems;

using UnityEngine;
using UnityEngine.Tilemaps;

using Foundations.Architecture.EventObserver;

using GameSystems.PlayerSystem.PlayerUnitSystem;

namespace GameSystems.TilemapSystem.SkillRangeTilemap
{
    public class FilteredSkillRangeTilemapMouseInteractor : MonoBehaviour
    {
        private IEventObserverNotifier EventObserverNotifier;

        private ISkillRangeTilemapSystem SkillRangeTilemapSystem;

        [SerializeField] private Tilemap FilteredSkillRangeTilemap;
        [SerializeField] private Vector3 mousePositionOffset;

        private int currentPlayerUniqueID;
        private int currentSkillID;
        private HashSet<Vector2Int> skillTargetPositions;

        private bool isActivated = false;
        private Vector2Int currentGridPosition;

        private void Awake()
        {
            this.EventObserverNotifier = new EventObserverNotifier();
        }

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
            UpdateSkillImpactRange_PlayerUnit updateSkillImpactRange_PlayerUnit = new();
            updateSkillImpactRange_PlayerUnit.PlayerUniqueID = this.currentPlayerUniqueID;
            updateSkillImpactRange_PlayerUnit.SkillID = this.currentSkillID;
            updateSkillImpactRange_PlayerUnit.TargetedPosition = skillTargetPosition;

            this.EventObserverNotifier.NotifyEvent(updateSkillImpactRange_PlayerUnit);
        }

        private void RequestOperateSkill_OnMouseClickUpEvent(Vector2Int skillTargetPosition)
        {
            // 특정 지점에 '스킬' 작동.
            OperateSkill_PlayerUnit operateSkill_PlayerUnit = new();
            operateSkill_PlayerUnit.PlayerUniqueID = this.currentPlayerUniqueID;
            operateSkill_PlayerUnit.SkillID = this.currentSkillID;
            operateSkill_PlayerUnit.TargetedPosition = skillTargetPosition;

            UIUXSystem.PlayerSkillRangeTilemap_CancelOrOperated PlayerSkillRangeTilemap_Cancel = new();
            PlayerSkillRangeTilemap_Cancel.PlayerUniqueID = this.currentPlayerUniqueID;

            this.EventObserverNotifier.NotifyEvent(PlayerSkillRangeTilemap_Cancel);
            this.EventObserverNotifier.NotifyEvent(operateSkill_PlayerUnit);

            this.SkillRangeTilemapSystem.DisActivateSkillRangeTilemap();
        }

        private void RequestSkillRangeTilemapCancelEvent()
        {
            UIUXSystem.PlayerSkillRangeTilemap_CancelOrOperated PlayerSkillRangeTilemap_Cancel = new();
            PlayerSkillRangeTilemap_Cancel.PlayerUniqueID = this.currentPlayerUniqueID;

            this.EventObserverNotifier.NotifyEvent(PlayerSkillRangeTilemap_Cancel);

            this.SkillRangeTilemapSystem.DisActivateSkillRangeTilemap();
        }
    }
}