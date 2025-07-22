using System.Collections.Generic;

using UnityEngine;

using Foundations.Architecture.EventObserver;
using Foundations.Architecture.ReferencesHandler;

using GameSystems.UtilitySystem;

namespace GameSystems.PlayerSystem.PlayerUnitSystem
{
    public class PlayerUnitMoveRangeCalculator_FourDirection : MonoBehaviour, IPlayerUnitMoveRangeCalculator
    {
        private IEventObserverNotifier EventObserverNotifier;

        private UnitMoveRangeCalculator_BFS_FourDirection UnitMoveRangeCalculator_BFS_FourDirection;

        // Player Unit 데이터 + 인터페이스 + Transform;
        private PlayerUnitManagerData myPlayerUnitManagerData;

        private void Awake()
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            this.UnitMoveRangeCalculator_BFS_FourDirection = HandlerManager.GetUtilityHandler<UnitMoveRangeCalculator_BFS_FourDirection>();

            this.EventObserverNotifier = new EventObserverNotifier();
        }

        public void InitialSetting(PlayerUnitManagerData playerUnitManagerData)
        {
            this.myPlayerUnitManagerData = playerUnitManagerData;
        }

        // 유닛의 '이동 가능 범위' 갱신 및 MovementTilemap에 이벤트 요청.
        public void UpdateMoveableRange()
        {
            if (this.UnitMoveRangeCalculator_BFS_FourDirection == null) return;

            Vector2Int currentPosition = this.myPlayerUnitManagerData.PlayerUnitGridPosition();

            // '현재 위치'에서 '일정 이동 코스트'를 통해 도달 할 수 있는 '이동 가능 범위'를 가져옵니다. ( 이동 (육지)극복치 비교 수행 )
            // 이동 가능 범위가 없을 시 리턴함.
            // 참고로, 요청마다 중복적으로 범위를 요청하는 것이긴 한데, 갱신 블럭에 대한 변수가 너무많아서 그냥 이렇게 하는게 편함.
            HashSet<Vector2Int> moveableRange = this.UnitMoveRangeCalculator_BFS_FourDirection.GetMoveRange(currentPosition,
                this.myPlayerUnitManagerData.PlayerUnitDynamicData.CurrentMoveCost,
                this.myPlayerUnitManagerData.PlayerUnitStaticData.GroundOvercomeWeight);

            // Notify를 통해 Moveable Tilemap에 Notify.
            TilemapSystem.MovementTilemap.ActivateMovementTilemapEvent activatePlayerUnitInteractionTileMap__Move = new();
            activatePlayerUnitInteractionTileMap__Move.PlayerUniqueID = this.myPlayerUnitManagerData.UniqueID;
            activatePlayerUnitInteractionTileMap__Move.CurrentPosition = currentPosition;
            activatePlayerUnitInteractionTileMap__Move.MoveableRange = moveableRange;

            this.EventObserverNotifier.NotifyEvent(activatePlayerUnitInteractionTileMap__Move);
        }
    }
}
