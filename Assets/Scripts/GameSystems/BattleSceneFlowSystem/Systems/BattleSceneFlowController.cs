using System;
using System.Collections;
using UnityEngine;

using Foundations.Architecture.EventObserver;
using Foundations.Architecture.ReferencesHandler;

namespace GameSystems.BattleSceneSystem
{
    public interface IBattleSceneFlowController
    {
        public void OperateBattleSceneFlow_StageSetting(int stageID);
        public void OperateBattleSceneFlow_TurnStartSetting();
        public void OperateBattleSceneFlow_PlayerTurnStartSetting();
        public void OperateBattleSceneFlow_PlayerTurnEndSetting();
        public void OperateBattleSceneFlow_EnemyTurnStartSetting();
        public void OperateBattleSceneFlow_TurnEndSetting();
    }

    public class BattleSceneFlowController : MonoBehaviour, IBattleSceneFlowController
    {
        private IEventObserverNotifier EventObserverNotifier;

        private BattleSceneData myBattleSceneData = new();

        private void Awake()
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            var BattleSceneSystemHandler = HandlerManager.GetDynamicDataHandler<BattleSceneSystemHandler>();

            BattleSceneSystemHandler.IBattleSceneFlowController = this;
            BattleSceneSystemHandler.BattleSceneData = this.myBattleSceneData;

            this.EventObserverNotifier = new EventObserverNotifier();
        }

        public void OperateBattleSceneFlow_StageSetting(int stageID)
        {
            this.myBattleSceneData.StageID = stageID;

            this.StopAllCoroutines();
            this.StartCoroutine(this.OperateBattleSceneFlow_StageSetting_Coroutine());
        }
        public IEnumerator OperateBattleSceneFlow_StageSetting_Coroutine()
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            var IMapVisibilityTilemapController = HandlerManager.GetDynamicDataHandler<TilemapSystem.TilemapSystemHandler>().IMapVisibilityTilemapController;
            var SkillRangeTilemapController = HandlerManager.GetDynamicDataHandler<TilemapSystem.TilemapSystemHandler>().ISkillRangeTilemapSystem;
            var EnemyUnitSpawnController = HandlerManager.GetDynamicDataHandler<EnemySystem.EnemySystemHandler>().IEnemyUnitSpawnController;
            var PlayerUnitSpawnContorller = HandlerManager.GetDynamicDataHandler<PlayerSystem.PlayerSystemHandler>().IPlayerUnitSpawnController;

            TerrainSystem.InitialSetGeneratedTerrainDataEvent initialSetGeneratedTerrainDataEvent = new();
            initialSetGeneratedTerrainDataEvent.StageID = this.myBattleSceneData.StageID;

            StageVisualSystem.InitialSetStageVisualResourcesData initialSetStageVisualResourcesData = new();
            initialSetStageVisualResourcesData.StageID = this.myBattleSceneData.StageID;

            TilemapSystem.MovementTilemap.InitialSetMovementTilemapEvent initialSetMovementTilemapEvent = new();
            initialSetMovementTilemapEvent.StageID = this.myBattleSceneData.StageID;

            IMapVisibilityTilemapController.InitialSetting(this.myBattleSceneData.StageID);
            IMapVisibilityTilemapController.UpdateFogVisibility();

            this.EventObserverNotifier.NotifyEvent(initialSetGeneratedTerrainDataEvent);
            this.EventObserverNotifier.NotifyEvent(initialSetStageVisualResourcesData);

            this.EventObserverNotifier.NotifyEvent(initialSetMovementTilemapEvent);

            SkillRangeTilemapController.InitialSetting();

            EnemyUnitSpawnController.InitialSetting(this.myBattleSceneData.StageID);
            EnemyUnitSpawnController.AllocateEnemyUnitSpawnData_Stage();
            yield return EnemyUnitSpawnController.GenerateEnemyUnit_Queue_Coroutine();

            PlayerUnitSpawnContorller.InitialSetting(this.myBattleSceneData.StageID);
            PlayerUnitSpawnContorller.AllocatePlayerUnitSpawnData_Stage();
            yield return PlayerUnitSpawnContorller.GeneratePlayerUnit_Queue_Coroutine();

            yield return null;

            // ���� Flow �帧 ���� ��û.
            this.OperateBattleSceneFlow_TurnStartSetting();
        }

        // Turn ���� �帧.
        public void OperateBattleSceneFlow_TurnStartSetting()
        {
            // Turn ����
            ++this.myBattleSceneData.TurnID;

            this.StopAllCoroutines();
            this.StartCoroutine(this.OperateBattleSceneFlow_TurnStartSetting_Coroutine());
        }
        private IEnumerator OperateBattleSceneFlow_TurnStartSetting_Coroutine()
        {
            // StageSetting �����ϰ� ������ �� ���.
            yield return new WaitForSeconds(0.2f);

            var HandlerManager = LazyReferenceHandlerManager.Instance;
            var EnemyUnitSpawnController = HandlerManager.GetDynamicDataHandler<EnemySystem.EnemySystemHandler>().IEnemyUnitSpawnController;

            // Turn��� Enemy ���� ��û -> �۾� ���� ��, ����Ϸ���� ���.
            if (EnemyUnitSpawnController.TryAllocateEnemyUnitSpawnData_Turn(this.myBattleSceneData.TurnID))
            {
                yield return StartCoroutine(EnemyUnitSpawnController.GenerateEnemyUnit_Queue_Coroutine());
            }

            // Player �� DB �ϳ� �� ���������...

            // �����ϰ� 1������ ���.
            yield return null;

            // ���� Flow �帧 ���� ��û.
            this.OperateBattleSceneFlow_PlayerTurnStartSetting();
        }

        // Player Turn ���� �帧.
        public void OperateBattleSceneFlow_PlayerTurnStartSetting()
        {
            this.StopAllCoroutines();
            this.StartCoroutine(this.OperateBattleSceneFlow_PlayerTurnSetting_Coroutine());
        }
        private IEnumerator OperateBattleSceneFlow_PlayerTurnSetting_Coroutine()
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            var PlayerUnitActionUIUXHandler = HandlerManager.GetDynamicDataHandler<UIUXSystem.UIUXSystemHandler>().PlayerUnitActionUIUXHandler;

            // PlayerTurnSetting�� ���� �ش� Panel Ȱ��ȭ UIUX �۾� ����.
            PlayerUnitActionUIUXHandler.IPlayerUnitActionPanelUIMediator.Update_PlayerUnitIconUIUX();
            PlayerUnitActionUIUXHandler.IPlayerUnitActionPanelUIMediator.Update_PlayerUnit_ActionableState();
            yield return this.StartCoroutine(PlayerUnitActionUIUXHandler.IPlayerUnitActionPanelUIMediator.Show_PlayerUnitActionPanel());

            yield return null;
        }

        // Player Turn ���� �帧.
        public void OperateBattleSceneFlow_PlayerTurnEndSetting()
        {
            this.StopAllCoroutines();
            this.StartCoroutine(this.OperateBattleSceneFlow_PlayerTurnEndSetting_Coroutine());
        }
        private IEnumerator OperateBattleSceneFlow_PlayerTurnEndSetting_Coroutine()
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            var PlayerUnitActionUIUXHandler = HandlerManager.GetDynamicDataHandler<UIUXSystem.UIUXSystemHandler>().PlayerUnitActionUIUXHandler;

            // PlayerTurnSetting�� ���� �ش� Panel UIUX ��Ȱ��ȭ �۾� ����.
            yield return this.StartCoroutine(PlayerUnitActionUIUXHandler.IPlayerUnitActionPanelUIMediator.Hide_PlayerUnitActionPanel());

            // ���� Flow �帧 ���� ��û.
            this.OperateBattleSceneFlow_EnemyTurnStartSetting();
        }

        // Enmey Turn ���� ȣ��.
        public void OperateBattleSceneFlow_EnemyTurnStartSetting()
        {
            this.StopAllCoroutines();
            this.StartCoroutine(this.OperateBattleSceneFlow_EnemyTurnStartSetting_Coroutine());
        }
        private IEnumerator OperateBattleSceneFlow_EnemyTurnStartSetting_Coroutine()
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            var IEnemyAISequencer = HandlerManager.GetDynamicDataHandler<EnemySystem.EnemySystemHandler>().IEnemyAISequencer;

            // Enemy UIUX ���.

            // Enemy ���� ��� -> Enemy ���� ���� + ���
            IEnemyAISequencer.AllocateEnemyAISequence();
            yield return StartCoroutine(IEnemyAISequencer.ExecuteEnemyAI_Coroutine());

            // Enemy UIUX ����.

            // Turn End Setting���� �ѱ��.
            this.OperateBattleSceneFlow_TurnEndSetting();
        }

        // Turn ���� �帧.
        public void OperateBattleSceneFlow_TurnEndSetting()
        {
            this.StopAllCoroutines();
            this.StartCoroutine(this.OperateBattleSceneFlow_TurnEndSetting_Coroutine());
        }
        private IEnumerator OperateBattleSceneFlow_TurnEndSetting_Coroutine()
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            var EnemyUnitManagerDataDBHandler = HandlerManager.GetDynamicDataHandler<GameSystems.EnemySystem.EnemyUnitSystem.EnemyUnitManagerDataDBHandler>();
            var PlayerUnitManagerDataDBHandler = HandlerManager.GetDynamicDataHandler<GameSystems.PlayerSystem.PlayerUnitManagerDataDBHandler>();


            // Enemy �� �ʱ�ȭ.
            if (EnemyUnitManagerDataDBHandler.TryGetAll(out var enemyUnitManagerDatas))
            {
                foreach (var enemyUnitManager in enemyUnitManagerDatas)
                {
                    enemyUnitManager.EnemyUnitFeatureInterfaceGroup.EnemyUnitManager.OperateTurnEndSetting();
                }
            }

            // Player �� �ʱ�ȭ.
            if (PlayerUnitManagerDataDBHandler.TryGetAll(out var playerUnitManagerDatas))
            {
                foreach (var playerUnitManager in playerUnitManagerDatas)
                {
                    playerUnitManager.PlayerUnitFeatureInterfaceGroup.PlayerUnitManager.OperateTurnEndSetting();
                }
            }

            yield return null;

            this.OperateBattleSceneFlow_TurnStartSetting();
        }
    }

    [Serializable]
    public class BattleSceneData
    {
        public BattleSceneData()
        {
            this.StageID = 0;
            this.TurnID = 0;
        }

        public int StageID { get; set; }
        public int TurnID { get; set; }
    }

    [Serializable]
    public enum BattleSceneFlowType
    {
        StageSetting,
        TurnStartOperation,
        PlayerTurn,
        EnemeyTurn,
        TurnEndOperation,
    }
}