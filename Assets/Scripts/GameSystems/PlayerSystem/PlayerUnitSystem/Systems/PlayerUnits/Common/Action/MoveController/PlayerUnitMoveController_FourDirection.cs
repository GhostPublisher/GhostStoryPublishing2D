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


        public void OperatePlayerUnitMove(Vector2Int targetPosition)
        {
            // 이동 경로 못찾음. 또는 이동할 필요 없음.(근처?)
            if (!this.TryGetMovePath(this.myPlayerUnitManagerData.PlayerUnitGridPosition, targetPosition, out var movePath))
                return;

            this.IPlayerUnitSpriteRendererController.UpdateFlipX(targetPosition);
            StartCoroutine(this.MovePlayerToTarget(movePath));
        }
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


        private IEnumerator MovePlayerToTarget(List<Vector2Int> movePath)
        {
            // 경로까지 이동.
            foreach (Vector2Int nextPosition in movePath)
            {
                Vector2Int currentGridPosition = this.myPlayerUnitManagerData.PlayerUnitGridPosition;

                if (nextPosition == currentGridPosition) continue;
                // 두 Grid 격자의 중심을 구함.
                Vector2 borderPosition = this.GetBorderPositionOfGridCell(currentGridPosition, nextPosition);

                this.IPlayerUnitAnimationController.OperateAnimation(PlayerUnitAnimationType.IsWalk);
                // 두 Grid 격자의 중심 지점까지 이동.
                yield return StartCoroutine(this.MoveToNextPosition(borderPosition));
                // 다음 격자를 기준으로 시야 갱신.
                this.IPlayerUnitVisibilityController.UpdateVisibleRange(nextPosition);
                // 나머지 절반 이동.
                yield return StartCoroutine(this.MoveToNextPosition(nextPosition));

                this.IPlayerUnitAnimationController.OperateAnimation(PlayerUnitAnimationType.Idle);
                yield return new WaitForSeconds(0.2f);
            }
        }
        // Grid 격자 상에서, '현재 위치'에서 '다음 위치'까지 이동하는 코루틴.
        private IEnumerator MoveToNextPosition(Vector2 nextPosition)
        {
            Transform PlayerUnitTransform = this.myPlayerUnitManagerData.PlayerUnitTransform;
            Vector3 nextWorldPosition = new Vector3(nextPosition.x * 1f, nextPosition.y * 1f, 0);

            while (Vector3.Distance(PlayerUnitTransform.position, nextWorldPosition) > 0.01f) // 목표 지점과의 거리 체크
            {
                PlayerUnitTransform.position = Vector3.MoveTowards(
                    PlayerUnitTransform.position,
                    nextWorldPosition,
                    this.PlayerMoveSpeed * Time.deltaTime);

                yield return null; // 다음 프레임까지 대기
            }

            PlayerUnitTransform.position = nextWorldPosition;
        }

        public void MoveForce(Vector2Int forceDirection)
        {

        }

        private Vector2 GetBorderPositionOfGridCell(Vector2 currentPosition, Vector2 nextPosition)
        {
            return currentPosition + (nextPosition - currentPosition) / 2f;
        }
    }
}
