using System.Collections.Generic;
using System.Collections;

using UnityEngine;

using Foundations.Architecture.ReferencesHandler;
using Foundations.Architecture.EventObserver;

using GameSystems.UtilitySystem;

using GameSystems.PlayerSystem.PlayerUnitSystem;

namespace GameSystems.EnemySystem.EnemyUnitSystem
{

    public class EnemyUnitMoveController_FourDirection : MonoBehaviour, IEnemyUnitMoveController
    {
        private IEventObserverNotifier EventObserverNotifier;

        // 로직 수행을 위한 유틸 클래스.
        private UnitPathFinder_AStar_FourDirection UnitPathFinder_AStar_FourDirection;

        private IEnemyUnitAnimationController EnemyUnitAnimationController;

        private EnemyUnitManagerData myEnemyUnitManagerData;

        [SerializeField] private int EnemyMoveSpeed;

        private void Awake()
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            this.UnitPathFinder_AStar_FourDirection = HandlerManager.GetUtilityHandler<UnitPathFinder_AStar_FourDirection>();

            this.EventObserverNotifier = new EventObserverNotifier();
        }

        public void InitialSetting(EnemyUnitManagerData enemyUnitManagerData)
        {
            this.myEnemyUnitManagerData = enemyUnitManagerData;

            this.EnemyUnitAnimationController = this.myEnemyUnitManagerData.EnemyUnitFeatureInterfaceGroup.EnemyUnitAnimationController;
        }

        public bool TryOperateMoveToTarget_NearestTarget()
        {
            // 모든 포지션 가져옴. ( 못 가져오면 심한 오류이지만, 오류 난다면 앞에서 먼저 남 )
            if (!this.myEnemyUnitManagerData.EnemyUnitDynamicData.TryGetEnemyUnitCurrentDetectedData<EnemyUnitCurrentMemoryData_PlayerUnit>(out var detectedPlayerUnitPositions))
            {
                // 해당 행동을 정상 작동 못했단 것을 리턴.
                return false;
            }         

            // A* 돌려서 가장 가까운 경로 가져옴.
            (Vector2Int, List<Vector2Int>) resultPath = new();
            resultPath.Item2 = new();

            List<Vector2Int> tempPath = new();
            int pathCount = 999;

            foreach (Vector2Int targetPosition in detectedPlayerUnitPositions.DetectedPositions)
            {
                tempPath.Clear();
                tempPath = this.UnitPathFinder_AStar_FourDirection.GetMovePath(
                    this.myEnemyUnitManagerData.EnemyUnitGridPosition, targetPosition, this.myEnemyUnitManagerData.EnemyUnitStaticData.GroundOvercomeWeight);

                if (pathCount > tempPath.Count)
                {
                    resultPath.Item1 = targetPosition;
                    resultPath.Item2 = new(tempPath);
                    pathCount = resultPath.Item2.Count;
                }
            }

            // 이동경로 크기가 1이면, 자기 자신의 위치값만을 말한다. 이동할 필요 없음.
            if (resultPath.Item2.Count <= 1)
            {
                // 해당 행동을 정상 작동할 필요가 없다는 것임.
                // 다음 우선순위 행동으로 넘어가도록 false 리턴.
                return false;
            }

            StopAllCoroutines();
            StartCoroutine(this.OperateMoveBehaviour(resultPath.Item2[1]));
            return true;
        }

        private IEnumerator OperateMoveBehaviour(Vector2Int nextPosition)
        {
            yield return StartCoroutine(this.MovePlayerToTarget(nextPosition));

            // 이곳에서 이동 코스값을 줄여주자.
            --this.myEnemyUnitManagerData.EnemyUnitDynamicData.CurrentMoveCost;

            // 장애물 관련 작업을 수행하라고 호출해야 되는 부분.
            // 장애물 관련 작업을 수행하라는 OperateObstacle_ToEnemy(enemyUniqueID, enemyPosition);
            // 해당 위치에 장애물이 있으면 장애물 OperateObstacle_ToEnemy(enemyUniqueID, enemyPosition);을 호출하고, 해당 장애물의 코루틴이 마무리되고 Enemy의 작업을 수행하도록 만들자.

            // 내 Enemy AI의 다음 작업 수행.
            this.myEnemyUnitManagerData.EnemyUnitFeatureInterfaceGroup.EnemyUnitManager.OperateEnemyAI();
        }

        private IEnumerator MovePlayerToTarget(Vector2Int nextPosition)
        {
            Vector2Int currentGridPosition = this.myEnemyUnitManagerData.EnemyUnitGridPosition;

            // 두 Grid 격자의 중심을 구함.
            Vector2 borderPosition = this.GetBorderPositionOfGridCell(currentGridPosition, nextPosition);

            this.EnemyUnitAnimationController.OperateAnimation(EnemyUnitAnimationType.IsWalk);
            // 두 Grid 격자의 중심 지점까지 이동.
            yield return StartCoroutine(this.MoveToNextPosition(borderPosition));
            // 다음 격자를 기준으로 시야 갱신.
            this.NotifyPlayerVisibility();
            // 나머지 절반 이동.
            yield return StartCoroutine(this.MoveToNextPosition(nextPosition));

            this.EnemyUnitAnimationController.OperateAnimation(EnemyUnitAnimationType.Idle);

            yield return new WaitForSeconds(0.2f);
        }

        // Grid 격자 상에서, '현재 위치'에서 '다음 위치'까지 이동하는 코루틴.
        private IEnumerator MoveToNextPosition(Vector2 nextPosition)
        {
            Transform EnemyUnitTransform = this.myEnemyUnitManagerData.EnemyUnitTransform;
            Vector3 nextWorldPosition = new Vector3(nextPosition.x * 1f, nextPosition.y * 1f, 0);

            while (Vector3.Distance(EnemyUnitTransform.position, nextWorldPosition) > 0.01f) // 목표 지점과의 거리 체크
            {
                EnemyUnitTransform.position = Vector3.MoveTowards(
                    EnemyUnitTransform.position,
                    nextWorldPosition,
                    this.EnemyMoveSpeed * Time.deltaTime);

                yield return null; // 다음 프레임까지 대기
            }

            EnemyUnitTransform.position = nextWorldPosition;
        }

        private Vector2 GetBorderPositionOfGridCell(Vector2 currentPosition, Vector2 nextPosition)
        {
            return currentPosition + (nextPosition - currentPosition) * 0.51f;
        }

        private void NotifyPlayerVisibility()
        {
            UpdatePlayerUnitVisibleRange UpdatePlayerUnitVisibleRange = new();
            this.EventObserverNotifier.NotifyEvent(UpdatePlayerUnitVisibleRange);
        }
    }
}