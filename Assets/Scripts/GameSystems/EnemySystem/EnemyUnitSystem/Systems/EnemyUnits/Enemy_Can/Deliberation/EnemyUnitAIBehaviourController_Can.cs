using System.Collections;

using UnityEngine;

namespace GameSystems.EnemySystem.EnemyUnitSystem
{

    public class EnemyUnitAIBehaviourController_Can : MonoBehaviour, IEnemyUnitAIBehaviourController
    {
        //
        private EnemyUnitManagerData myEnemyUnitManagerData;
        
        public void InitialSetting(EnemyUnitManagerData enemyUnitManagerData)
        {
            this.myEnemyUnitManagerData = enemyUnitManagerData;
        }

        // true : 행동을 수행한 것, false : 수행할 행동이 없는 것.
        public void DecideAIOperation()
        {
            // 공격 가능한 범위 안에 존재하면 공격해라.
            if (this.IsOperationAvailable_Skill01Operation())
            {
                // 정상적으로 작업이 수행되었을 경우, 해당 행위를 수행하도록 함. <---> 반대로 정삭적으로 수행되지 않았을 경우, 다음 행위를 수행하도록 넘김.
                if (this.myEnemyUnitManagerData.EnemyUnitFeatureInterfaceGroup.EnemyUnitSkillControllers[SkillSlotType.Skill01] is Enemy_Can_Skill01 skillController)
                {
                    // 공격 수행.
                    skillController.TryOperateSkillToTarget_NearestTarget();
                    Debug.Log($"탐지된 적을 공격하는 작업 수행.");
                    return;
                }
            }

            // 시야에 들어온 대상으로 이동해라.
            if (this.IsOperationAvailable_MoveToTarget())
            {
                // 정상적으로 작업이 수행되었을 경우, 해당 행위를 수행하도록 함. <---> 반대로 정삭적으로 수행되지 않았을 경우, 다음 행위를 수행하도록 넘김.
                if (this.myEnemyUnitManagerData.EnemyUnitFeatureInterfaceGroup.EnemyUnitMoveController.TryOperateMoveToTarget_NearestTarget())
                {
                    Debug.Log($"탐지된 적 위치로 이동하는 작업 수행");
                    return;
                }
            }


            this.StartCoroutine(this.WaitNotifyEnd());
        }

        // 탐지된 적(현재 시야 내 보이는 객체)이 있는지 확인하고, 해당 로직 수행을 요청.
        private bool IsOperationAvailable_MoveToTarget()
        {
            // 이동 가능한 Cost가 없으면 false
            if (this.myEnemyUnitManagerData.EnemyUnitDynamicData.CurrentMoveCost <= 0) return false;
            // 탐지된 적 데이터가 없거나, 정상적이지 못하면 false.
            if (!this.myEnemyUnitManagerData.EnemyUnitDynamicData.TryGetEnemyUnitCurrentDetectedData<EnemyUnitCurrentMemoryData_PlayerUnit>(out var detectedData)) return false;
            if (detectedData == null || detectedData.DetectedPositions == null || detectedData.DetectedPositions.Count <= 0) return false;

            return true;
        }

        // 스킬 범위 내에 존재하는 PlayerUnit 공격.
        private bool IsOperationAvailable_Skill01Operation()
        {
            // 사용 가능한 Skill Cost가 없으면 false
            if (this.myEnemyUnitManagerData.EnemyUnitDynamicData.CurrentSkillCost <= 0) return false;
            // 공격 가능한 대상이 없거나, 정상적이지 못하면 false
            if (!this.myEnemyUnitManagerData.EnemyUnitDynamicData.TryGetSkillValidTargetRangeData(SkillSlotType.Skill01, out var validTargetRange)) return false;
            if (validTargetRange == null || validTargetRange.ValidTargetPositions == null || validTargetRange.ValidTargetPositions.Count <= 0) return false;

            return true;
        }

        private IEnumerator WaitNotifyEnd()
        {
            yield return null;

            this.myEnemyUnitManagerData.EnemyUnitDynamicData.IsOperationOver = true;
            this.myEnemyUnitManagerData.EnemyUnitFeatureInterfaceGroup.EnemyUnitManager.OperateEnemyAI();
        }
    }
}