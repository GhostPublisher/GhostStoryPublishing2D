using System.Collections;

using UnityEngine;

using Foundations.Architecture.EventObserver;
using Foundations.Architecture.ReferencesHandler;

namespace GameSystems.EnemySystem.EnemyUnitSystem
{
    public interface IEnemyUnitManager
    {
        public void OperateNewTurnSetting();
        public void OperateEnemyAI();
        public void OperateDie();

        public void OperateEnemyUnitInitialSetting();
        public IEnumerator OperateEnemyUnitInitialSetting_Coroutine();

        public void StopAllCoroutines();
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

        [SerializeField] private GameObject EnemyAIControllerContainer;

        // 판단
        private IEnemyUnitAIBehaviourController EnemyAIBehaviourController;

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

        public void StopAllCoroutines_Refer()
        {
            this.StopAllCoroutines();
        }

        public void OperateEnemyUnitInitialSetting()
        {
            this.InitialSetting();

            var HandlerManager = LazyReferenceHandlerManager.Instance;
            var PlayerUnitManagerDataDBHandler = HandlerManager.GetDynamicDataHandler<PlayerSystem.PlayerUnitManagerDataDBHandler>();

            // 해당 위치 시야 갱신.
            if (PlayerUnitManagerDataDBHandler.TryGetAll(out var playerUnitManagerDatas))
            {
                foreach (var data in playerUnitManagerDatas)
                {
                    data.PlayerUnitFeatureInterfaceGroup.PlayerUnitVisibilityController.UpdateVisibleRange();
                }
            }
        }

        public IEnumerator OperateEnemyUnitInitialSetting_Coroutine()
        {
            this.InitialSetting();

            var HandlerManager = LazyReferenceHandlerManager.Instance;
            var PlayerUnitManagerDataDBHandler = HandlerManager.GetDynamicDataHandler<PlayerSystem.PlayerUnitManagerDataDBHandler>();
            var FogTilemapData = HandlerManager.GetDynamicDataHandler<TilemapSystem.TilemapSystemHandler>().FogTilemapData;

            // 해당 위치 시야 갱신.
            if (PlayerUnitManagerDataDBHandler.TryGetAll(out var playerUnitManagerDatas))
            {
                foreach(var data in playerUnitManagerDatas)
                {
                    data.PlayerUnitFeatureInterfaceGroup.PlayerUnitVisibilityController.UpdateVisibleRange();
                }
            }

            // 플레이어의 시야 안에 존재할 시, 카메라 포커싱 작업 수행.
            if(FogTilemapData.TryGetFogState(this.myEnemyUnitManagerData.EnemyUnitGridPosition(), out var fogStage) && fogStage == TilemapSystem.FogTilemap.FogState.Visible)
            {
                // 카메라 포커싱. 부분

                yield return StartCoroutine(this.EnemyUnitAnimationController.PlayAndWaitAnimation(EnemyUnitAnimationType.Spawn));
            }            
        }



        private void InitialSetting()
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            var EnemyUnitManagerDataDBHandler = HandlerManager.GetDynamicDataHandler<EnemyUnitManagerDataDBHandler>();

            this.UniqueID = this.gameObject.GetInstanceID();

            // PlugIn 조립.
            this.EnemyAIBehaviourController = this.EnemyAIControllerContainer.GetComponent<IEnemyUnitAIBehaviourController>();

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

            this.myEnemyUnitManagerData = new EnemyUnitManagerData(this.UniqueID, this.myEnemyUnitStaticData, this.myEnemyUnitDynamicData, this.myEnemyUnitFeatureInterfaceGroup, this.transform);
            EnemyUnitManagerDataDBHandler.AddEnemyUnitManagerData(this.myEnemyUnitManagerData);

            this.SubObjectInitialSetting();
        }
        private void SubObjectInitialSetting()
        {
            this.EnemyUnitStatusController.InitialSetting(this.myEnemyUnitManagerData);

            this.EnemyUnitSpriteRendererController.InitialSetting(this.myEnemyUnitManagerData);

            this.EnemyHitReactionController.InitialSetting(this.myEnemyUnitManagerData);
            this.EnemyUnitEffectController.InitialSetting(this.myEnemyUnitManagerData);

            this.EnemyAIBehaviourController.InitialSetting(this.myEnemyUnitManagerData);
        }

        public void OperateEnemyAI(int enemyUniqueID)
        {
            // AI 작업 지속.
            if (this.UniqueID != enemyUniqueID) return;

            // 다음 AI 작업 요청.
            if (this.myEnemyUnitDynamicData.IsOperationOver)
            {
                // 다음 EnemyAI 작업을 하도록 Notify.
                this.EventObserverNotifer.NotifyEvent(new EnemyAISequenceSystem.ExecuteEnemyAI());
                return;
            }

            // 행동 판단을 위한 데이터 갱신.
            this.EnemyAIBehaviourController.UpdateSensingAndPerceptionData();
            // 행동 판단 및 실행.
            this.EnemyAIBehaviourController.DecideAIOperation();
        }
        public void OperateEnemyAI()
        {
            // 다음 AI 작업 요청.
            if (this.myEnemyUnitDynamicData.IsOperationOver)
            {
                // 다음 EnemyAI 작업을 하도록 Notify.
                this.EventObserverNotifer.NotifyEvent(new EnemyAISequenceSystem.ExecuteEnemyAI());
                return;
            }

            // 행동 판단을 위한 데이터 갱신.
            this.EnemyAIBehaviourController.UpdateSensingAndPerceptionData();
            // 행동 판단 및 실행.
            this.EnemyAIBehaviourController.DecideAIOperation();
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