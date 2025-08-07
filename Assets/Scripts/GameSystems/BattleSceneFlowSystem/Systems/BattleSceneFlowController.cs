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

            // 다음 Flow 흐름 수행 요청.
            this.OperateBattleSceneFlow_TurnStartSetting();
        }

        // Turn 시작 흐름.
        public void OperateBattleSceneFlow_TurnStartSetting()
        {
            // Turn 증가
            ++this.myBattleSceneData.TurnID;

            this.StopAllCoroutines();
            this.StartCoroutine(this.OperateBattleSceneFlow_TurnStartSetting_Coroutine());
        }
        private IEnumerator OperateBattleSceneFlow_TurnStartSetting_Coroutine()
        {
            // StageSetting 안전하게 끝나는 것 대기.
            yield return new WaitForSeconds(0.2f);

            var HandlerManager = LazyReferenceHandlerManager.Instance;
            var EnemyUnitSpawnController = HandlerManager.GetDynamicDataHandler<EnemySystem.EnemySystemHandler>().IEnemyUnitSpawnController;

            // Turn기반 Enemy 생성 요청 -> 작업 수행 시, 수행완료까지 대기.
            if (EnemyUnitSpawnController.TryAllocateEnemyUnitSpawnData_Turn(this.myBattleSceneData.TurnID))
            {
                yield return StartCoroutine(EnemyUnitSpawnController.GenerateEnemyUnit_Queue_Coroutine());
            }

            // Player 용 DB 하나 더 만들어주자...

            // 안전하게 1프레임 대기.
            yield return null;

            // 다음 Flow 흐름 수행 요청.
            this.OperateBattleSceneFlow_PlayerTurnStartSetting();
        }

        // Player Turn 시작 흐름.
        public void OperateBattleSceneFlow_PlayerTurnStartSetting()
        {
            this.StopAllCoroutines();
            this.StartCoroutine(this.OperateBattleSceneFlow_PlayerTurnSetting_Coroutine());
        }
        private IEnumerator OperateBattleSceneFlow_PlayerTurnSetting_Coroutine()
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            var PlayerUnitActionUIUXHandler = HandlerManager.GetDynamicDataHandler<UIUXSystem.UIUXSystemHandler>().PlayerUnitActionUIUXHandler;

            // PlayerTurnSetting에 관한 해당 Panel 활성화 UIUX 작업 수행.
            PlayerUnitActionUIUXHandler.IPlayerUnitActionPanelUIMediator.Update_PlayerUnitIconUIUX();
            PlayerUnitActionUIUXHandler.IPlayerUnitActionPanelUIMediator.Update_PlayerUnit_ActionableState();
            yield return this.StartCoroutine(PlayerUnitActionUIUXHandler.IPlayerUnitActionPanelUIMediator.Show_PlayerUnitActionPanel());

            yield return null;
        }

        // Player Turn 종료 흐름.
        public void OperateBattleSceneFlow_PlayerTurnEndSetting()
        {
            this.StopAllCoroutines();
            this.StartCoroutine(this.OperateBattleSceneFlow_PlayerTurnEndSetting_Coroutine());
        }
        private IEnumerator OperateBattleSceneFlow_PlayerTurnEndSetting_Coroutine()
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            var PlayerUnitActionUIUXHandler = HandlerManager.GetDynamicDataHandler<UIUXSystem.UIUXSystemHandler>().PlayerUnitActionUIUXHandler;

            // PlayerTurnSetting에 관한 해당 Panel UIUX 비활성화 작업 수행.
            yield return this.StartCoroutine(PlayerUnitActionUIUXHandler.IPlayerUnitActionPanelUIMediator.Hide_PlayerUnitActionPanel());

            // 다음 Flow 흐름 수행 요청.
            this.OperateBattleSceneFlow_EnemyTurnStartSetting();
        }

        // Enmey Turn 시작 호출.
        public void OperateBattleSceneFlow_EnemyTurnStartSetting()
        {
            this.StopAllCoroutines();
            this.StartCoroutine(this.OperateBattleSceneFlow_EnemyTurnStartSetting_Coroutine());
        }
        private IEnumerator OperateBattleSceneFlow_EnemyTurnStartSetting_Coroutine()
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            var IEnemyAISequencer = HandlerManager.GetDynamicDataHandler<EnemySystem.EnemySystemHandler>().IEnemyAISequencer;

            // Enemy UIUX 출력.

            // Enemy 순서 명시 -> Enemy 순차 실행 + 대기
            IEnemyAISequencer.AllocateEnemyAISequence();
            yield return StartCoroutine(IEnemyAISequencer.ExecuteEnemyAI_Coroutine());

            // Enemy UIUX 종료.

            // Turn End Setting으로 넘기기.
            this.OperateBattleSceneFlow_TurnEndSetting();
        }

        // Turn 종료 흐름.
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


            // Enemy 값 초기화.
            if (EnemyUnitManagerDataDBHandler.TryGetAll(out var enemyUnitManagerDatas))
            {
                foreach (var enemyUnitManager in enemyUnitManagerDatas)
                {
                    enemyUnitManager.EnemyUnitFeatureInterfaceGroup.EnemyUnitManager.OperateTurnEndSetting();
                }
            }

            // Player 값 초기화.
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