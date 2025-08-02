using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace GameSystems.EnemySystem.EnemyUnitSystem
{
    // Enemy 행동 순서 관리자.
    public class EnemyUnitAIBehaviourController_Can : MonoBehaviour, IEnemyUnitAIBehaviourController
    {
        [SerializeField] private GameObject EnemyAIControllerContainer;

        // 가공
        private IEnemyUnitAIDataPreprocessor EnemyUnitAIDataPreprocessor;
        // 가공
        private Dictionary<SkillSlotType, IEnemyUnitSkillRangeDataPreprocessor> EnemyUnitSkillRangeDataPreprocessors;

        // 사용
        private IEnemyUnitMoveController EnemyUnitMoveController;
        // 사용
        private Dictionary<SkillSlotType, IEnemyUnitSkillController> EnemyUnitSkillControllers;


        private EnemyUnitManagerData myEnemyUnitManagerData;
        
        public void InitialSetting(EnemyUnitManagerData enemyUnitManagerData)
        {
            this.myEnemyUnitManagerData = enemyUnitManagerData;

            this.EnemyUnitAIDataPreprocessor = this.EnemyAIControllerContainer.GetComponent<IEnemyUnitAIDataPreprocessor>();
            this.EnemyUnitMoveController = this.EnemyAIControllerContainer.GetComponent<IEnemyUnitMoveController>();

            this.EnemyUnitSkillRangeDataPreprocessors = new();
            this.EnemyUnitSkillControllers = new();

            // 하이어러키 Component 가져오기.
            foreach (var component in this.EnemyAIControllerContainer.GetComponents<MonoBehaviour>())
            {
                if (component is IEnemyUnitSkillRangeDataPreprocessor enemyUnitSkillRangeDataPreprocessor)
                {
                    this.EnemyUnitSkillRangeDataPreprocessors.Add(enemyUnitSkillRangeDataPreprocessor.SkillSlotType, enemyUnitSkillRangeDataPreprocessor);
                }

                if (component is IEnemyUnitSkillController enemyUnitSkillController)
                {
                    this.EnemyUnitSkillControllers.Add(enemyUnitSkillController.SkillSlotType, enemyUnitSkillController);
                }
            }

            // 인터페이스 등록.
            var myEnemyUnitFeatureInterfaceGroup = this.myEnemyUnitManagerData.EnemyUnitFeatureInterfaceGroup;

            myEnemyUnitFeatureInterfaceGroup.EnemyUnitAIDataPreprocessor = this.EnemyUnitAIDataPreprocessor;
            myEnemyUnitFeatureInterfaceGroup.EnemyUnitMoveController = this.EnemyUnitMoveController;

            foreach (var interfaceMember in this.EnemyUnitSkillRangeDataPreprocessors)
            {
                myEnemyUnitFeatureInterfaceGroup.EnemyUnitSkillRangeDataPreprocessors.Add(interfaceMember.Key, interfaceMember.Value);
            }
            foreach (var interfaceMember in this.EnemyUnitSkillControllers)
            {
                myEnemyUnitFeatureInterfaceGroup.EnemyUnitSkillControllers.Add(interfaceMember.Key, interfaceMember.Value);
            }


            this.EnemyUnitAIDataPreprocessor.InitialSetting(this.myEnemyUnitManagerData);
            this.EnemyUnitMoveController.InitialSetting(this.myEnemyUnitManagerData);

            foreach (var interfaceMember in this.EnemyUnitSkillRangeDataPreprocessors.Values)
            {
                interfaceMember.InitialSetting(this.myEnemyUnitManagerData);
            }
            foreach (var interfaceMember in this.EnemyUnitSkillControllers.Values)
            {
                interfaceMember.InitialSetting(this.myEnemyUnitManagerData);
            }
        }

        public IEnumerator OperateEnemyAI_Coroutine()
        {
            while (!this.myEnemyUnitManagerData.EnemyUnitDynamicData.IsOperationOver)
            {
                Debug.Log($"UniqueID : {this.myEnemyUnitManagerData.UniqueID}, MoveCost :{this.myEnemyUnitManagerData.EnemyUnitDynamicData.CurrentMoveCost}, SkillCost : {this.myEnemyUnitManagerData.EnemyUnitDynamicData.CurrentSkillCost}");
                // 행동 판단을 위한 데이터 갱신.
                this.UpdateSensingAndPerceptionData();

                // 공격 가능한 범위 안에 존재하면 공격해라.
                if (this.IsOperationAvailable_Skill01Operation())
                {
                    // 정상적으로 작업이 수행되었을 경우, 해당 행위를 수행하도록 함. <---> 반대로 정삭적으로 수행되지 않았을 경우, 다음 행위를 수행하도록 넘김.
                    if (this.myEnemyUnitManagerData.EnemyUnitFeatureInterfaceGroup.EnemyUnitSkillControllers[SkillSlotType.Skill01] is Enemy_Can_Skill01 skillController)
                    {
                        // 공격 수행.
                        Debug.Log($"탐지된 적을 공격하는 작업 수행.");
                        yield return skillController.OperateSkill_Coroutine();
                        continue;
                    }
                }

                // 시야에 들어온 대상으로 이동해라.
                if (this.IsOperationAvailable_MoveToTarget())
                {
                    var EnemyUnitMoveController = this.myEnemyUnitManagerData.EnemyUnitFeatureInterfaceGroup.EnemyUnitMoveController;
                    // 정상적으로 작업이 수행되었을 경우, 해당 행위를 수행하도록 함. <---> 반대로 정삭적으로 수행되지 않았을 경우, 다음 행위를 수행하도록 넘김.
                    if (EnemyUnitMoveController.TryGetNextPosition_NearestTarget(out var nextPosition))
                    {
                        Debug.Log($"탐지된 적 위치로 이동하는 작업 수행");
                        yield return EnemyUnitMoveController.OperateMoveBehaviour(nextPosition);
                        continue;
                    }
                }

                this.myEnemyUnitManagerData.EnemyUnitDynamicData.IsOperationOver = true;
            }

            yield return null;
        }

        // 행동 판단을 위한 데이터 갱신.
        public void UpdateSensingAndPerceptionData()
        {
            // 감각 범위 및 인지 데이터 갱신.
            this.EnemyUnitAIDataPreprocessor.UpdateDataPreprocessor();
            // 스킬 범위 및 인지 데이터 갱신.
            foreach (var dataProcessor in this.EnemyUnitSkillRangeDataPreprocessors.Values)
            {
                dataProcessor.UpdateSkillDataPreprocessor();
            }
        }

        // 탐지된 적(현재 시야 내 보이는 객체)이 있는지 확인하고, 해당 로직 수행을 요청.
        private bool IsOperationAvailable_MoveToTarget()
        {
            // 이동 가능한 Cost가 없으면 false
            if (this.myEnemyUnitManagerData.EnemyUnitDynamicData.CurrentMoveCost < this.myEnemyUnitManagerData.EnemyUnitStaticData.MoveActionCost) return false;
            // 탐지된 적 데이터가 없거나, 정상적이지 못하면 false.
            if (!this.myEnemyUnitManagerData.EnemyUnitDynamicData.TryGetEnemyUnitCurrentDetectedData<EnemyUnitCurrentMemoryData_PlayerUnit>(out var detectedData)) return false;
            if (detectedData == null || detectedData.DetectedPositions == null || detectedData.DetectedPositions.Count <= 0) return false;

            return true;
        }

        // 스킬 범위 내에 존재하는 PlayerUnit 공격.
        private bool IsOperationAvailable_Skill01Operation()
        {
            // 사용 가능한 Skill Cost가 없으면 false
            if (this.myEnemyUnitManagerData.EnemyUnitDynamicData.CurrentSkillCost < this.myEnemyUnitManagerData.EnemyUnitStaticData.Skill01ActionCost) return false;
            // 공격 가능한 대상이 없거나, 정상적이지 못하면 false
            if (!this.myEnemyUnitManagerData.EnemyUnitDynamicData.TryGetSkillValidTargetRangeData(SkillSlotType.Skill01, out var validTargetRange)) return false;
            if (validTargetRange == null || validTargetRange.ValidTargetPositions == null || validTargetRange.ValidTargetPositions.Count <= 0) return false;

            return true;
        }
    }
}