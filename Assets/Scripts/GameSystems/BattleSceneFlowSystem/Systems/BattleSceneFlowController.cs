using System;
using System.Collections;
using UnityEngine;

using Foundations.Architecture.EventObserver;
using Foundations.Architecture.ReferencesHandler;

namespace GameSystems.BattleSceneSystem
{
    public class BattleSceneHandler : IDynamicReferenceHandler
    {
        public IBattleSceneFlowController IBattleSceneFlowController;
    }

    public interface IBattleSceneFlowController
    {
        public void BattleSceneFlowControllEvent_StageSetting(int stageID);

    }

    public class BattleSceneFlowController : MonoBehaviour, IBattleSceneFlowController
    {
        private IEventObserverNotifier EventObserverNotifier;

        private BattleSceneData myBattleSceneData;

        private void Awake()
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            HandlerManager.GetDynamicDataHandler<BattleSceneHandler>().IBattleSceneFlowController = this;
        }

        public void BattleSceneFlowControllEvent_StageSetting(int stageID)
        {
            this.EventObserverNotifier = new EventObserverNotifier();

            this.myBattleSceneData = new(stageID);

            // StageSetting 수행.
            this.OperateStageSetting();

            // Turn Start Setting 수행.
            this.OperateTurnStartSetting();
        }

        public void OperateStageSetting()
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            var FogTilemapController = HandlerManager.GetDynamicDataHandler<TilemapSystem.TilemapSystemHandler>().IFogTilemapController;
            var EnemyUnitSpawnController = HandlerManager.GetDynamicDataHandler<EnemySystem.EnemySystemHandler>().IEnemyUnitSpawnController;
            var PlayerUnitSpawnContorller = HandlerManager.GetDynamicDataHandler<PlayerSystem.PlayerSystemHandler>().IPlayerUnitSpawnController;

            TerrainSystem.InitialSetGeneratedTerrainDataEvent initialSetGeneratedTerrainDataEvent = new();
            initialSetGeneratedTerrainDataEvent.StageID = this.myBattleSceneData.StageID;

            StageVisualSystem.InitialSetStageVisualResourcesData initialSetStageVisualResourcesData = new();
            initialSetStageVisualResourcesData.StageID = this.myBattleSceneData.StageID;

            TilemapSystem.MovementTilemap.InitialSetMovementTilemapEvent initialSetMovementTilemapEvent = new();
            initialSetMovementTilemapEvent.StageID = this.myBattleSceneData.StageID;

            TilemapSystem.SkillRangeTilemap.InitialSetSkillRangeTilemapEvent initialSetSkillRangeTilemapEvent = new();
            initialSetSkillRangeTilemapEvent.StageID = this.myBattleSceneData.StageID;
            
            this.EventObserverNotifier.NotifyEvent(initialSetGeneratedTerrainDataEvent);
            this.EventObserverNotifier.NotifyEvent(initialSetStageVisualResourcesData);

            this.EventObserverNotifier.NotifyEvent(initialSetMovementTilemapEvent);
            this.EventObserverNotifier.NotifyEvent(initialSetSkillRangeTilemapEvent);

            FogTilemapController.InitialSetting(this.myBattleSceneData.StageID);
            FogTilemapController.UpdateFogVisibility();

            EnemyUnitSpawnController.InitialSetting(this.myBattleSceneData.StageID);
            EnemyUnitSpawnController.AllocateEnemyUnitSpawnData_Stage();
            EnemyUnitSpawnController.GenerateEnemyUnit_Queue();

            PlayerUnitSpawnContorller.InitialSetting(this.myBattleSceneData.StageID);
            PlayerUnitSpawnContorller.AllocatePlayerUnitSpawnData_Stage();
            PlayerUnitSpawnContorller.GeneratePlayerUnit_Queue();
        }

        public void OperateTurnStartSetting()
        {
            // Turn 증가
            ++this.myBattleSceneData.TurnID;

            this.StopAllCoroutines();
            this.StartCoroutine(this.OperateTurnStartSetting_Coroutine());
        }

        private IEnumerator OperateTurnStartSetting_Coroutine()
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            var EnemyUnitSpawnController = HandlerManager.GetDynamicDataHandler<EnemySystem.EnemySystemHandler>().IEnemyUnitSpawnController;

            // Turn기반 Enemy 생성 요청 -> 작업 수행 시, 수행완료까지 대기.
            if (EnemyUnitSpawnController.TryAllocateEnemyUnitSpawnData_Turn(this.myBattleSceneData.TurnID))
            {
                EnemyUnitSpawnController.StopAllCoroutines_Refer();
                yield return StartCoroutine(EnemyUnitSpawnController.GenerateEnemyUnit_Queue_Coroutine());
            }

            // DB 하나 더 만들어주자...
        }
    }

    [Serializable]
    public class BattleSceneData
    {
        public int StageID;
        public int TurnID;

        public BattleSceneData(int stageID)
        {
            StageID = stageID;
            TurnID = 0;
        }
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