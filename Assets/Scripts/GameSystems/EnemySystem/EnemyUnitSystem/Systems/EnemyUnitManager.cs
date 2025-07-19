using System.Collections.Generic;

using UnityEngine;

using Foundations.Architecture.EventObserver;
using Foundations.Architecture.ReferencesHandler;

namespace GameSystems.EnemySystem.EnemyUnitSystem
{
    public interface IEnemyUnitManager
    {
        public bool TryInitialSetting(out EnemyUnitManagerData enemyUnitManagerData);
        public void OperateNewTurnSetting();
        public void OperateEnemyAI();
        public void OperateEnemyUnitMove(Vector2Int targetPosition);
        public void OperateSkill(int skillID, Vector2Int targetedPosition);
        public void OperateDie();
    }

    public class EnemyUnitManager : MonoBehaviour, IEnemyUnitManager
    {
        private IEventObserverNotifier EventObserverNotifer;

        // 공통적으로 사용.
        [SerializeField] private EnemyUnitStatusController EnemyUnitStatusController;

        [SerializeField] private EnemyUnitSpriteRendererController EnemyUnitSpriteRendererController;
        [SerializeField] private EnemyUnitAnimationController EnemyUnitAnimationController;

        [SerializeField] private EnemyHitReactionController EnemyHitReactionController;
        [SerializeField] private EnemyUnitEffectController EnemyUnitEffectController;

        [SerializeField] private GameObject EnemyUnitInterfaceComponentContainer;
        [SerializeField] private GameObject EnemyUnitSkillInterfaceComponentContainer;
        // 가공
        private IEnemyUnitAIDataPreprocessor EnemyUnitAIDataPreprocessor;
        // 가공
        private Dictionary<SkillSlotType, IEnemyUnitSkillRangeDataPreprocessor> EnemyUnitSkillRangeDataPreprocessors;
        // 판단
        private IEnemyUnitAIBehaviourController EnemyAIBehaviourController;
        // 사용
        private IEnemyUnitMoveController EnemyUnitMoveController;
        // 사용
        private Dictionary<SkillSlotType, IEnemyUnitSkillController> EnemyUnitSkillControllers;


        private int UniqueID;
        // Enemy Unit 데이터 + 인터페이스 + Transform;
        private EnemyUnitManagerData myEnemyUnitManagerData;
        [SerializeField] private EnemyUnitStaticData myEnemyUnitStaticData;
        private EnemyUnitDynamicData myEnemyUnitDynamicData;
        private EnemyUnitFeatureInterfaceGroup myEnemyUnitFeatureInterfaceGroup;

        private void Awake()
        {
            this.EventObserverNotifer = new EventObserverNotifier();
        }

        private void OnDestroy()
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            var EnemyUnitManagerDataDBHandler = HandlerManager.GetDynamicDataHandler<EnemyUnitManagerDataDBHandler>();

            EnemyUnitManagerDataDBHandler.RemoveEnemyUnitManagerData(this.myEnemyUnitManagerData);
        }

        public bool TryInitialSetting(out EnemyUnitManagerData enemyUnitManagerData)
        {
            this.UniqueID = this.gameObject.GetInstanceID();

            // PlugIn 조립.
            this.EnemyUnitAIDataPreprocessor = this.EnemyUnitInterfaceComponentContainer.GetComponent<IEnemyUnitAIDataPreprocessor>();
            this.EnemyAIBehaviourController = this.EnemyUnitInterfaceComponentContainer.GetComponent<IEnemyUnitAIBehaviourController>();
            this.EnemyUnitMoveController = this.EnemyUnitInterfaceComponentContainer.GetComponent<IEnemyUnitMoveController>();

            this.EnemyUnitSkillRangeDataPreprocessors = new();
            this.EnemyUnitSkillControllers = new();

            foreach (var component in this.EnemyUnitSkillInterfaceComponentContainer.GetComponents<MonoBehaviour>())
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

            // EnemyUnitDynamicData
            this.myEnemyUnitDynamicData = new();

            // EnemyUnitFeatureInterfaceGroup
            this.myEnemyUnitFeatureInterfaceGroup = new();
            this.myEnemyUnitFeatureInterfaceGroup.EnemyUnitManager = this;
            this.myEnemyUnitFeatureInterfaceGroup.EnemyUnitStatusController = this.EnemyUnitStatusController;

            this.myEnemyUnitFeatureInterfaceGroup.EnemyUnitSpriteRendererController = this.EnemyUnitSpriteRendererController;
            this.myEnemyUnitFeatureInterfaceGroup.EnemyUnitAnimationController = this.EnemyUnitAnimationController;

            this.myEnemyUnitFeatureInterfaceGroup.EnemyHitReactionController = this.EnemyHitReactionController;
            this.myEnemyUnitFeatureInterfaceGroup.EnemyUnitEffectController = this.EnemyUnitEffectController;

            this.myEnemyUnitFeatureInterfaceGroup.EnemyUnitMoveController = this.EnemyUnitMoveController;

            foreach (var interfaceMember in this.EnemyUnitSkillRangeDataPreprocessors)
            {
                this.myEnemyUnitFeatureInterfaceGroup.EnemyUnitSkillRangeDataPreprocessors.Add(interfaceMember.Key, interfaceMember.Value);
            }
            foreach (var interfaceMember in this.EnemyUnitSkillControllers)
            {
                this.myEnemyUnitFeatureInterfaceGroup.EnemyUnitSkillControllers.Add(interfaceMember.Key, interfaceMember.Value);
            }

            enemyUnitManagerData = new EnemyUnitManagerData(this.UniqueID, this.myEnemyUnitStaticData, this.myEnemyUnitDynamicData, this.myEnemyUnitFeatureInterfaceGroup, this.transform);

            // 뭔가 오류날게 없나? 일단 이걸로 명시.
            if (enemyUnitManagerData == null) return false;

            this.myEnemyUnitManagerData = enemyUnitManagerData;
            this.AdditionalSetting();
            return true;
        }
        private void AdditionalSetting()
        {
            this.EnemyUnitStatusController.InitialSetting(this.myEnemyUnitManagerData);

            this.EnemyUnitSpriteRendererController.InitialSetting(this.myEnemyUnitManagerData);

            this.EnemyHitReactionController.InitialSetting(this.myEnemyUnitManagerData);
            this.EnemyUnitEffectController.InitialSetting(this.myEnemyUnitManagerData);

            this.EnemyUnitAIDataPreprocessor.InitialSetting(this.myEnemyUnitManagerData);
            this.EnemyAIBehaviourController.InitialSetting(this.myEnemyUnitManagerData);
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

        public void OperateEnemyAI(int enemyUniqueID)
        {
            // AI 작업 지속.
            if (this.UniqueID != enemyUniqueID) return;

            Debug.Log($"EnemyUniqueID : {this.UniqueID}, CurrentMoveCost: {this.myEnemyUnitDynamicData.CurrentMoveCost}, IsOperationOver {this.myEnemyUnitDynamicData.IsOperationOver}");

            // 다음 AI 작업 요청.
            if (this.myEnemyUnitDynamicData.IsOperationOver)
            {
                // 다음 EnemyAI 작업을 하도록 Notify.
                this.EventObserverNotifer.NotifyEvent(new EnemyAISequenceSystem.ExecuteEnemyAI());
                return;
            }

            // 범위 값 갱신.
            this.EnemyUnitAIDataPreprocessor.UpdateDataPreprocessor();
            foreach(var dataProcessor in this.EnemyUnitSkillRangeDataPreprocessors.Values)
            {
                dataProcessor.UpdateSkillDataPreprocessor();
            }
            // 행동 판단 및 실행.
            this.EnemyAIBehaviourController.DecideAIOperation();
        }
        public void OperateEnemyAI()
        {
//            Debug.Log($"EnemyUniqueID : {this.UniqueID}, CurrentMoveCost: {this.myEnemyUnitDynamicData.CurrentMoveCost}, IsOperationOver : {this.myEnemyUnitDynamicData.IsOperationOver}");

            // 다음 AI 작업 요청.
            if (this.myEnemyUnitDynamicData.IsOperationOver)
            {
                // 다음 EnemyAI 작업을 하도록 Notify.
                this.EventObserverNotifer.NotifyEvent(new EnemyAISequenceSystem.ExecuteEnemyAI());
                return;
            }

            // 범위 값 갱신.
            this.EnemyUnitAIDataPreprocessor.UpdateDataPreprocessor();
            foreach (var dataProcessor in this.EnemyUnitSkillRangeDataPreprocessors.Values)
            {
                dataProcessor.UpdateSkillDataPreprocessor();
            }
            // 행동 판단 및 실행.
            this.EnemyAIBehaviourController.DecideAIOperation();
        }

        public void OperateEnemyUnitMove(Vector2Int targetPosition)
        {

        }

        public void OperateSkill(int skillID, Vector2Int targetedPosition)
        {

        }

        public void OperateDie()
        {
            Destroy(this.gameObject);
        }

        public void OperateNewTurnSetting()
        {
            // 이곳에서 이동 코스트 및 스킬 코스트를 초기화.
            this.myEnemyUnitDynamicData.CurrentSkillCost = this.myEnemyUnitStaticData.DefaultSkillCost;
            this.myEnemyUnitDynamicData.CurrentMoveCost = this.myEnemyUnitStaticData.DefaultMoveCost;

            this.myEnemyUnitDynamicData.IsOperationOver = false;
        }
    }
}