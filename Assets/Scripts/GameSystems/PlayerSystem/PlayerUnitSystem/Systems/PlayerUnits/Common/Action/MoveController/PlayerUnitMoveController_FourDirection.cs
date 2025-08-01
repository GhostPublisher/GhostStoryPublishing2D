using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Foundations.Architecture.ReferencesHandler;

using GameSystems.UtilitySystem;

namespace GameSystems.PlayerSystem.PlayerUnitSystem
{

    public class PlayerUnitMoveController_FourDirection : MonoBehaviour, IPlayerUnitMoveController
    {
        // 로직 수행을 위한 유틸 클래스.
        private UnitPathFinder_AStar_FourDirection IUnitPathFinder_AStar_FourDirection;

        // '시야 갱신' 기능 클래스 참조 인터페이스.
        private IPlayerUnitVisibilityController IPlayerUnitVisibilityController;
        private IPlayerUnitSpriteRendererController IPlayerUnitSpriteRendererController;
        private IPlayerUnitAnimationController IPlayerUnitAnimationController;

        // Player Unit 고유 데이터 + 인터페이스 + Transform;
        private PlayerUnitManagerData myPlayerUnitManagerData;

        [SerializeField] private int PlayerMoveSpeed;

        private void Awake()
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            this.IUnitPathFinder_AStar_FourDirection = HandlerManager.GetUtilityHandler<UnitPathFinder_AStar_FourDirection>();
        }

        public void InitialSetting(PlayerUnitManagerData playerUnitManagerData)
        {
            this.myPlayerUnitManagerData = playerUnitManagerData;

            this.IPlayerUnitVisibilityController = this.myPlayerUnitManagerData.PlayerUnitFeatureInterfaceGroup.PlayerUnitVisibilityController;
            this.IPlayerUnitSpriteRendererController = this.myPlayerUnitManagerData.PlayerUnitFeatureInterfaceGroup.PlayerUnitSpriteRendererController;
            this.IPlayerUnitAnimationController = this.myPlayerUnitManagerData.PlayerUnitFeatureInterfaceGroup.PlayerUnitAnimationController;
        }

        // 목표 위치로 이동.
        // 경로 탐색 -> 1칸 이동 -> 이벤트 확인 -> 1칸이동 -> 이동 완료 명시.
        public IEnumerator OperateMove_Coroutine(Vector2Int targetPosition)
        {
            // 이동 경로 못찾음. 또는 이동할 필요 없음 ( 이동 경로가 자기 자신만 있을 경우 )
            if (!this.TryGetMovePath(this.myPlayerUnitManagerData.PlayerUnitGridPosition(), targetPosition, out var movePath))
                yield break;

            Queue<Vector2Int> movePath_Queue = new(movePath);

            // 맨 처음 1개는 현재 내 위치. ( 제외하고 시작 )
            movePath_Queue.Dequeue();
            // 경로를 1칸씩 이동하면서, 코스트 감소 및 이벤트 체크.
            while (movePath_Queue.Count > 0)
            {
                Vector2Int nextPos = movePath_Queue.Dequeue();
//                Debug.Log($"CurrentMoveCost : {this.myPlayerUnitManagerData.PlayerUnitDynamicData.CurrentMoveCost}, NextPos : {nextPos}");

                yield return StartCoroutine(this.MovePlayerToTarget(nextPos));
                // 잠시 대기.
                yield return new WaitForSeconds(0.1f);

                // Move Cost 감소.
                this.myPlayerUnitManagerData.PlayerUnitDynamicData.BehaviourCost_Current -= this.myPlayerUnitManagerData.PlayerUnitStaticData.MoveCost;

                // 장애물 이벤트에 한번 확인.
                // if( try(~~~) ) 이벤트가 있을 경우, movePath_Queue.Clear(); 를 통해 이동 정지. + 관련 이벤트 코루틴 수행 및 yield return을 통해 대기.
            }

            // 다 끝나면, Player Unit Action UIUX 갱신 및 상호작용 가능 명시.
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            var PlayerUnitActionUIUXHandler = HandlerManager.GetDynamicDataHandler<UIUXSystem.UIUXSystemHandler>().PlayerUnitActionUIUXHandler;

            // Player Cost 값에 따른 PlayerUnitActionUIUXHandler 업데이트하라는 코드. 
            PlayerUnitActionUIUXHandler.IPlayerUnitActionPanelUIMediator.Update_PlayerUnit_ActionableState();
            // Player Action UIUX 상호작용 가능 명시.
            PlayerUnitActionUIUXHandler.IsInteractived = true;
        }

        // 현재 위치, 목표 위치 기반 이동 경로 탐색.
        private bool TryGetMovePath(Vector2Int currentPosition, Vector2Int targetPosition, out List<Vector2Int> movePath)
        {
            movePath = this.IUnitPathFinder_AStar_FourDirection.GetMovePath(currentPosition, targetPosition,
                this.myPlayerUnitManagerData.PlayerUnitStaticData.GroundOvercomeWeight);

            if (movePath == null || movePath.Count <= 1)
            {
                movePath = null;
                return false;
            }

            return true;
        }

        // 1칸 이동. 반칸 이동 -> 시야 갱신 -> 반칸 이동.
        private IEnumerator MovePlayerToTarget(Vector2Int nextPosition)
        {
            Vector2Int currentGridPosition = this.myPlayerUnitManagerData.PlayerUnitGridPosition();
            if (nextPosition == currentGridPosition) yield break;

            // 두 Grid 격자의 중심을 구함.
            Vector2 borderPosition = this.GetBorderPositionOfGridCell(currentGridPosition, nextPosition);

            // 이동 방향을 향해 'FilpX' 변경.
            this.IPlayerUnitSpriteRendererController.UpdateFlipX(nextPosition);
            // Walk 애니메이션
            this.IPlayerUnitAnimationController.OperateAnimation(PlayerUnitAnimationType.IsWalk);
            // 두 Grid 격자의 중심 지점까지 이동.
            yield return StartCoroutine(this.MoveToNextPosition(borderPosition));
            // 다음 격자를 기준으로 시야 갱신.
            this.IPlayerUnitVisibilityController.UpdateVisibleRange(nextPosition);
            // 나머지 절반 이동.
            yield return StartCoroutine(this.MoveToNextPosition(nextPosition));

            this.IPlayerUnitAnimationController.OperateAnimation(PlayerUnitAnimationType.Idle);
        }

        // Grid 격자 상에서, '현재 위치'에서 '다음 위치'까지 이동하는 코루틴.
        private IEnumerator MoveToNextPosition(Vector2 nextPosition)
        {
            Transform PlayerUnitTransform = this.myPlayerUnitManagerData.PlayerUnitTransform;
            Vector3 nextWorldPosition = this.ConvertWorldToGrid(nextPosition);

            while (Vector3.Distance(PlayerUnitTransform.localPosition, nextWorldPosition) > 0.01f) // 목표 지점과의 거리 체크
            {
                PlayerUnitTransform.localPosition = Vector3.MoveTowards(
                    PlayerUnitTransform.localPosition,
                    nextWorldPosition,
                    this.PlayerMoveSpeed * Time.deltaTime);

                yield return null; // 다음 프레임까지 대기
            }

            PlayerUnitTransform.localPosition = nextWorldPosition;
        }

        public void MoveForce(Vector2Int forceDirection)
        {

        }

        public Vector3 ConvertWorldToGrid(Vector2 targetPosition)
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance.GetUtilityHandler<UtilitySystem.IsometricCoordinateConvertor>();
            return HandlerManager.ConvertGridToWorld(targetPosition);
        }

        private Vector2 GetBorderPositionOfGridCell(Vector2 currentPosition, Vector2 nextPosition)
        {
            return currentPosition + (nextPosition - currentPosition) / 2f;
        }
    }
}
