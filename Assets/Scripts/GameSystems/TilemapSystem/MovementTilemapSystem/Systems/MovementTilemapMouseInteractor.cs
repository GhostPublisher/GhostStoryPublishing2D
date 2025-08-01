using System.Collections.Generic;
using UnityEngine.EventSystems;

using UnityEngine;
using UnityEngine.Tilemaps;

using Foundations.Architecture.ReferencesHandler;
using Foundations.Architecture.EventObserver;

using GameSystems.PlayerSystem.PlayerUnitSystem;

namespace GameSystems.TilemapSystem.MovementTilemap
{
    public class MovementTilemapMouseInteractor : MonoBehaviour
    {
        private IEventObserverNotifier EventObserverNotifier;

        private IMovementTilemapSystem MovementTilemapSystem;

        [SerializeField] private Tilemap MovementTilemap;
        [SerializeField] private Vector3 mousePositionOffset;

        private HashSet<Vector2Int> moveableRange;
        private bool isActivated = false;

        private int currentPlayerUniqueID;
        private Vector2Int currentGridPosition;

        private void Awake()
        {
            this.EventObserverNotifier = new EventObserverNotifier();
        }

        public void InitialSetting(IMovementTilemapSystem MovementTilemapSystem)
        {
            this.MovementTilemapSystem = MovementTilemapSystem;
        }

        private void Update()
        {
            // 활성화 요청이 없었거나, 이동가능범위 값이 정상적이지 않을 경우 리턴.
            if (!isActivated || this.moveableRange == null || this.moveableRange.Count == 0) return;

            if (EventSystem.current.IsPointerOverGameObject()) return;

            // 마우스 현재 좌표를 Grid 좌표로 변환.
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cellPos = this.MovementTilemap.WorldToCell(worldPos + mousePositionOffset);
            Vector2Int gridPos = new Vector2Int(cellPos.x, cellPos.y);

            // 좌표가 동일하면 넘어가기.
            if (this.currentGridPosition != gridPos)
            {
//                Debug.Log($"MousePos : {cellPos}, GirdPos : {gridPos}");

                // 좌표가 변경되었을 경우.
                // 이전 좌표 값이 활성화된 상태였을 경우, PointerExit.
                if (this.moveableRange.Contains(currentGridPosition))
                {
                    this.MovementTilemapSystem.OnPointerExit(this.currentGridPosition);
                }

                // 새로운 좌표 값이 활성화된 상태였을 경우, PointerExit.
                if (this.moveableRange.Contains(gridPos))
                {
                    this.MovementTilemapSystem.OnPointerEnter(gridPos);
                }

                this.currentGridPosition = gridPos;
            }

            if (Input.GetMouseButtonUp(0) && this.moveableRange.Contains(this.currentGridPosition))
            {
                this.RequestMoveToTargetPosition_PlayerUnit_OnMouseClickUpEvent(this.currentGridPosition);
            }

            if (Input.GetMouseButtonUp(1))
            {
                this.RequestMovementTilemapCancelEvent();
            }
        }

        private void RequestMoveToTargetPosition_PlayerUnit_OnMouseClickUpEvent(Vector2Int targetPosition)
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;

            // Player Move 요청.
            var PlayerUnitManagerDataDBHandler = HandlerManager.GetDynamicDataHandler<PlayerSystem.PlayerUnitManagerDataDBHandler>();
            if(PlayerUnitManagerDataDBHandler.TryGetPlayerUnitManagerData(this.currentPlayerUniqueID, out var playerUnitManagerData))
            {
                playerUnitManagerData.PlayerUnitFeatureInterfaceGroup.PlayerUnitManager.OperateMove(this.currentPlayerUniqueID, targetPosition);
            }

            this.MovementTilemapSystem.DisActivateMovementTilemap();
        }

        private void RequestMovementTilemapCancelEvent()
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            var PlayerUnitActionUIUXHandler = HandlerManager.GetDynamicDataHandler<GameSystems.UIUXSystem.UIUXSystemHandler>().PlayerUnitActionUIUXHandler;

            PlayerUnitActionUIUXHandler.IPlayerUnitActionPanelUIMediator.Show_PlayerUnitBehaviourPanel(this.currentPlayerUniqueID);

            this.MovementTilemapSystem.DisActivateMovementTilemap();
        }

        public void ActivateMovementTileMap(int playerUniqueID, HashSet<Vector2Int> moveableRange)
        {
            this.enabled = true;
            this.isActivated = true;

            this.currentPlayerUniqueID = playerUniqueID;
            this.moveableRange = moveableRange;
        }

        public void DisActivateMovementTileMap()
        {
            this.enabled = false;

            this.moveableRange = null;
            this.isActivated = false;

            this.currentGridPosition = new Vector2Int(-99, -99);
        }
    }
}