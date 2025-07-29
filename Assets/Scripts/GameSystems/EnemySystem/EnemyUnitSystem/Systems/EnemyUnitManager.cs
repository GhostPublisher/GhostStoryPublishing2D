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

        // ���������� ���.
        [SerializeField] private EnemyUnitStatusController EnemyUnitStatusController;

        [SerializeField] private EnemyUnitSpriteRendererController EnemyUnitSpriteRendererController;
        [SerializeField] private EnemyUnitAnimationController EnemyUnitAnimationController;

        [SerializeField] private EnemyHitReactionController EnemyHitReactionController;
        [SerializeField] private EnemyUnitEffectController EnemyUnitEffectController;

        [SerializeField] private GameObject EnemyAIControllerContainer;

        // �Ǵ�
        private IEnemyUnitAIBehaviourController EnemyAIBehaviourController;

        private int UniqueID;
        // Enemy Unit ������ + �������̽� + Transform;
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

            // �ش� ��ġ �þ� ����.
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

            // �ش� ��ġ �þ� ����.
            if (PlayerUnitManagerDataDBHandler.TryGetAll(out var playerUnitManagerDatas))
            {
                foreach(var data in playerUnitManagerDatas)
                {
                    data.PlayerUnitFeatureInterfaceGroup.PlayerUnitVisibilityController.UpdateVisibleRange();
                }
            }

            // �÷��̾��� �þ� �ȿ� ������ ��, ī�޶� ��Ŀ�� �۾� ����.
            if(FogTilemapData.TryGetFogState(this.myEnemyUnitManagerData.EnemyUnitGridPosition(), out var fogStage) && fogStage == TilemapSystem.FogTilemap.FogState.Visible)
            {
                // ī�޶� ��Ŀ��. �κ�

                yield return StartCoroutine(this.EnemyUnitAnimationController.PlayAndWaitAnimation(EnemyUnitAnimationType.Spawn));
            }            
        }



        private void InitialSetting()
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            var EnemyUnitManagerDataDBHandler = HandlerManager.GetDynamicDataHandler<EnemyUnitManagerDataDBHandler>();

            this.UniqueID = this.gameObject.GetInstanceID();

            // PlugIn ����.
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
            // AI �۾� ����.
            if (this.UniqueID != enemyUniqueID) return;

            // ���� AI �۾� ��û.
            if (this.myEnemyUnitDynamicData.IsOperationOver)
            {
                // ���� EnemyAI �۾��� �ϵ��� Notify.
                this.EventObserverNotifer.NotifyEvent(new EnemyAISequenceSystem.ExecuteEnemyAI());
                return;
            }

            // �ൿ �Ǵ��� ���� ������ ����.
            this.EnemyAIBehaviourController.UpdateSensingAndPerceptionData();
            // �ൿ �Ǵ� �� ����.
            this.EnemyAIBehaviourController.DecideAIOperation();
        }
        public void OperateEnemyAI()
        {
            // ���� AI �۾� ��û.
            if (this.myEnemyUnitDynamicData.IsOperationOver)
            {
                // ���� EnemyAI �۾��� �ϵ��� Notify.
                this.EventObserverNotifer.NotifyEvent(new EnemyAISequenceSystem.ExecuteEnemyAI());
                return;
            }

            // �ൿ �Ǵ��� ���� ������ ����.
            this.EnemyAIBehaviourController.UpdateSensingAndPerceptionData();
            // �ൿ �Ǵ� �� ����.
            this.EnemyAIBehaviourController.DecideAIOperation();
        }

        public void OperateDie()
        {
            Destroy(this.gameObject);
        }

        public void OperateNewTurnSetting()
        {
            // �̰����� �̵� �ڽ�Ʈ �� ��ų �ڽ�Ʈ�� �ʱ�ȭ.
            this.myEnemyUnitDynamicData.CurrentSkillCost = this.myEnemyUnitStaticData.DefaultSkillCost;
            this.myEnemyUnitDynamicData.CurrentMoveCost = this.myEnemyUnitStaticData.DefaultMoveCost;

            this.myEnemyUnitDynamicData.IsOperationOver = false;
        }
    }
}