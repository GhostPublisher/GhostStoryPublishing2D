using System.Collections.Generic;
using UnityEngine;

using Foundations.Architecture.EventObserver;

namespace GameSystems.BattleSceneSystem
{

    public class BattleSceneFlowController : MonoBehaviour
    {
        private IEventObserverNotifier EventObserverNotifier;

        private BattleSceneData myBattleSceneData;

        public void InitialSetting(int stageID)
        {
            this.EventObserverNotifier = new EventObserverNotifier();

            this.myBattleSceneData = new(stageID);

            // StageSetting ����.
            this.OperateStageSetting();

            // Turn Start Setting ����.
            this.OperateTurnStartSetting();
        }

        public void OperateStageSetting()
        {
            TerrainSystem.InitialSetGeneratedTerrainDataEvent initialSetGeneratedTerrainDataEvent = new();
            initialSetGeneratedTerrainDataEvent.StageID = this.myBattleSceneData.StageID;

            StageVisualSystem.InitialSetStageVisualResourcesData initialSetStageVisualResourcesData = new();
            initialSetStageVisualResourcesData.StageID = this.myBattleSceneData.StageID;

            TilemapSystem.FogTilemap.FogTilemapInitialSettingEvent fogTilemapInitialSettingEvent = new();
            fogTilemapInitialSettingEvent.StageID = this.myBattleSceneData.StageID;

            TilemapSystem.MovementTilemap.InitialSetMovementTilemapEvent initialSetMovementTilemapEvent = new();
            initialSetMovementTilemapEvent.StageID = this.myBattleSceneData.StageID;

            TilemapSystem.SkillRangeTilemap.InitialSetSkillRangeTilemapEvent initialSetSkillRangeTilemapEvent = new();
            initialSetSkillRangeTilemapEvent.StageID = this.myBattleSceneData.StageID;

            EnemySystem.EnemySpawnSystem.EnemySpawnInitialSettingEvent enemySpawnInitialSettingEvent = new();
            enemySpawnInitialSettingEvent.StageID = this.myBattleSceneData.StageID;

            this.EventObserverNotifier.NotifyEvent(initialSetGeneratedTerrainDataEvent);
            this.EventObserverNotifier.NotifyEvent(initialSetStageVisualResourcesData);
            this.EventObserverNotifier.NotifyEvent(fogTilemapInitialSettingEvent);
            this.EventObserverNotifier.NotifyEvent(initialSetMovementTilemapEvent);
            this.EventObserverNotifier.NotifyEvent(initialSetSkillRangeTilemapEvent);
            this.EventObserverNotifier.NotifyEvent(enemySpawnInitialSettingEvent);
        }

        public void OperateTurnStartSetting()
        {
            // Unit���� �� �ʱ�ȭ -> Unit ���� ��
        }
    }
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
}