using System.Collections;

using UnityEngine;

using Foundations.Architecture.ReferencesHandler;
using GameSystems.EnemySystem.EnemyUnitSystem;

namespace GameSystems.UnitSystem
{
    public interface IMoveOperation
    {
        public IEnumerator MoveOperation(Transform unitTransform, Vector2Int targetPosition, float moveSpeed = 3);
    }

    public class MoveOperateion_Walk : MonoBehaviour, IMoveOperation, IUtilityReferenceHandler
    {
        public IEnumerator MoveOperation(Transform unitTransform, Vector2Int targetPosition, float moveSpeed = 3)
        {
            Vector3 targetWorldPosition = this.ConvertGridToWorld(targetPosition);

            while (Vector3.Distance(unitTransform.position, targetWorldPosition) > 0.01f) // 목표 지점과의 거리 체크
            {
                unitTransform.position = Vector3.MoveTowards(
                    unitTransform.position,
                    targetWorldPosition,
                    moveSpeed * Time.deltaTime);

                yield return null; // 다음 프레임까지 대기
            }

            unitTransform.position = targetWorldPosition;
        }

        private Vector3 ConvertGridToWorld(Vector2Int targetPosition)
        {
            return new Vector3(targetPosition.x * 2.5f, targetPosition.y * 2.5f);
        }
    }
}