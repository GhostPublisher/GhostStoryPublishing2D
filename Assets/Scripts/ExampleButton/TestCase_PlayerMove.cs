using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Foundations.Architecture.ReferencesHandler;
using Foundations.Architecture.EventObserver;

using GameSystems.UtilitySystem;

using GameSystems.TerrainSystem;
using GameSystems.StageVisualSystem;
using GameSystems.TilemapSystem;
using GameSystems.TilemapSystem.FogTilemap;
using GameSystems.TilemapSystem.MovementTilemap;

using GameSystems.EnemySystem.EnemyVisibilitySystem;

using GameSystems.PlayerSystem.PlayerSpawnSystem;
using GameSystems.PlayerSystem.PlayerUnitSystem;

using GameSystems.UIUXSystem;

namespace Example00
{
    public class TestCase_PlayerMove : MonoBehaviour
    {
        private IEventObserverNotifier EventObserverNotifier;

        [Header("Stage 크기")]
        [SerializeField] private int Width;
        [SerializeField] private int Height;

        [Header("지형 Tilemap")]
        [SerializeField] private InitialSetStageVisualResourcesData_EventTest GenerateGroundTilemapEvent_Test;

        [Header("Terrain Data 가중치 설정")]
        [SerializeField] private InitialSetGeneratedTerrainDataEvent_Test InitialSetGeneratedTerrainDataEvent_Test;

        [Header("Enemy 위치 설정")]
        [SerializeField] private List<UnitSpawnEvent_Test> EnemySpawnEvent_Tests;
        [Header("Player 위치 설정")]
        [SerializeField] private List<UnitSpawnEvent_Test> PlayerUnitSpawnEvent_Tests;

        [Header("스케너 설정")]
        [SerializeField] private ScannerTestData ScannerTestData;
        [Header("스케너 설정 - Scanner가 움직일 위치지정.")]
        [SerializeField] private List<Vector2Int> TargetMoves;

        [SerializeField] private int testEnemyUniqueID;


        private void Awake()
        {
            this.EventObserverNotifier = new EventObserverNotifier();
        }

        // 지형 Tilemap 생성 / 삭제
        public void Notify_Generate_GroundTilemap()
        {
            this.EventObserverNotifier.NotifyEvent(this.GenerateGroundTilemapEvent_Test);
        }
        public void Notify_Clear_GroundTilemap()
        {
            ClearStageVisualResourcesDataEvent ClearGroundTilemapEvent = new();
            this.EventObserverNotifier.NotifyEvent(ClearGroundTilemapEvent);
        }


        // Terrain Data 할당 / 삭제
        public void Notify_Allocate_GeneratedTerrainData()
        {
            this.EventObserverNotifier.NotifyEvent(this.InitialSetGeneratedTerrainDataEvent_Test);
        }
        public void Notify_Clear_GeneratedTerrainData()
        {
            ClearTrerrainData ClearTrerrainData = new();
            this.EventObserverNotifier.NotifyEvent(ClearTrerrainData);
        }


        // Fog Tilemap 할당 / 삭제 / Hide / Show
        public void Notify_Allocate_FogTilemap()
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            HandlerManager.GetDynamicDataHandler<TilemapSystemHandler>().IFogTilemapController.InitialSetting(this.Width, this.Height);
        }
        public void Notify_Clear_FogTilemap()
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            HandlerManager.GetDynamicDataHandler<TilemapSystemHandler>().IFogTilemapController.ClearFogData();
        }
        public void Notify_Hide_FogTilemap()
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            HandlerManager.GetDynamicDataHandler<TilemapSystemHandler>().IFogTilemapController.HideAllFog_Test();
        }
        public void Notify_Show_FogTilemap()
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            HandlerManager.GetDynamicDataHandler<TilemapSystemHandler>().IFogTilemapController.ShowAllFog_Test();
        }


        // Movement Tilemap 할당.
        public void Notify_Allocate_MovementTilemap()
        {
            InitialSetMovementTilemapEvent_Raw InitialSetMovementTilemapEvent_Raw = new();
            InitialSetMovementTilemapEvent_Raw.Width = this.Width;
            InitialSetMovementTilemapEvent_Raw.Height = this.Height;

            this.EventObserverNotifier.NotifyEvent(InitialSetMovementTilemapEvent_Raw);
        }


        // 아군 생성
        public void Notify_SpawnPlayerDatas()
        {
            StartCoroutine(this.SpawnPlayers());
        }
        private IEnumerator SpawnPlayers()
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            var PlayerUnitSpawnController = HandlerManager.GetDynamicDataHandler<GameSystems.PlayerSystem.PlayerSystemHandler>().IPlayerUnitSpawnController;

            // Raw 데이터를 통한 Enemy 생성.            
            foreach (var data in this.EnemySpawnEvent_Tests)
            {
                PlayerUnitSpawnController.StopAllCoroutines_Refer();
                yield return StartCoroutine(PlayerUnitSpawnController.GeneratePlayerUnit_Coroutine(data.UnitID, data.SpawnPosition));
            }
        }
        // 아군 클리어.
        public void Notify_ClearPlayerData()
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            var PlayerUnitSpawnController = HandlerManager.GetDynamicDataHandler<GameSystems.PlayerSystem.PlayerSystemHandler>().IPlayerUnitSpawnController;

            PlayerUnitSpawnController.ClearPlayerUnitAndDatas();
        }


        // enemy 생성.
        public void Notify_SpawnEnemyDatas()
        {
            this.StopAllCoroutines();
            StartCoroutine(this.SpawnEnemys());
        }
        private IEnumerator SpawnEnemys()
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            var EnemyUnitSpawnController = HandlerManager.GetDynamicDataHandler<GameSystems.EnemySystem.EnemySystemHandler>().IEnemyUnitSpawnController;

            // Raw 데이터를 통한 Enemy 생성.            
            foreach (var data in this.EnemySpawnEvent_Tests)
            {
                yield return StartCoroutine(EnemyUnitSpawnController.GenerateEnemyUnit_Coroutine(data.UnitID, data.SpawnPosition));
            }
        }
        // Enemy Clear
        public void Notify_ClearEnemyData()
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            var EnemyUnitSpawnController = HandlerManager.GetDynamicDataHandler<GameSystems.EnemySystem.EnemySystemHandler>().IEnemyUnitSpawnController;

            EnemyUnitSpawnController.ClearEnemyUnitAndDatas();
        }


        // Scanner를 통한 시야 갱신
        public void Notify_UpdateScanner()
        {
            this.StopAllCoroutines();
            this.StartCoroutine(OperateScannerMove());
        }
        public IEnumerator OperateScannerMove()
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            var FogVisibleRangeCalculator = HandlerManager.GetUtilityHandler<VisibilityRangeCalculator>();
            var IFogTilemapController = HandlerManager.GetDynamicDataHandler<TilemapSystemHandler>().IFogTilemapController;

            foreach (Vector2Int target in this.TargetMoves)
            {
                var newVisibleRange = FogVisibleRangeCalculator.GetFilteredVisibleRange_Player(target, this.ScannerTestData.VisibleSize, this.ScannerTestData.VisibleOvercomeWeight);

                IFogTilemapController.UpdateScannerVisibleData_ForFogVisibility(this.ScannerTestData.ScannerID, newVisibleRange);
                IFogTilemapController.UpdateFogVisibility();

                UpdateScanerVisibleData_ForEnemyVisibility data02 = new();
                data02.ScannerUniqueID = this.ScannerTestData.ScannerID;
                data02.VisibleRange = newVisibleRange;
                data02.PhysicalVisionOvercomeWeight = this.ScannerTestData.PhysicalVisionOvercomeWeight;
                data02.SpiritualVisionOvercomeWeight = this.ScannerTestData.SpiritualVisionOvercomeWeight;

                this.EventObserverNotifier.NotifyEvent(data02);

                yield return new WaitForSeconds(1f);
            }
        }
        public void Notify_ClearScanner()
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            var IFogTilemapController = HandlerManager.GetDynamicDataHandler<TilemapSystemHandler>().IFogTilemapController;

            IFogTilemapController.RemoveScannerVisibleData_ForFogVisibility(this.ScannerTestData.ScannerID);
            IFogTilemapController.UpdateFogVisibility();
        }


        // Player Unit을 통한 시야 갱신
        public void Notify_UpdatePlayerVisible()
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            var PlayerUnitManagerDataDBHandler = HandlerManager.GetDynamicDataHandler<GameSystems.PlayerSystem.PlayerUnitManagerDataDBHandler>();

            if(PlayerUnitManagerDataDBHandler.TryGetAll(out var playerUnitManagerDatas))
            {
                foreach (var data in playerUnitManagerDatas)
                {
                    data.PlayerUnitFeatureInterfaceGroup.PlayerUnitVisibilityController.UpdateVisibleRange();
                }
            }
        }

        // Player Unit Action Panel 활성화 / 갱신 / 초기화
        public void Notify_DisplayPlayerUnitActionPanelUIUX()
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            var PlayerUnitActionUIUXHandler = HandlerManager.GetDynamicDataHandler<GameSystems.UIUXSystem.UIUXSystemHandler>().PlayerUnitActionUIUXHandler;

            StartCoroutine(PlayerUnitActionUIUXHandler.IPlayerUnitActionPanelUIMediator.Show_PlayerUnitActionPanel());
        }
        // Player Unit Action Panel 갱신
        public void Notify_UpdatePlayerUnitActionPanelUIUX()
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            var PlayerUnitActionUIUXHandler = HandlerManager.GetDynamicDataHandler<GameSystems.UIUXSystem.UIUXSystemHandler>().PlayerUnitActionUIUXHandler;

            this.StartCoroutine(PlayerUnitActionUIUXHandler.IPlayerUnitActionPanelUIMediator.Show_PlayerUnitActionPanel());
        }
        // Player Unit Action Panel 초기화
        public void Notify_ClearPlayerUnitAcitionPanelUI()
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            var PlayerUnitActionUIUXHandler = HandlerManager.GetDynamicDataHandler<GameSystems.UIUXSystem.UIUXSystemHandler>().PlayerUnitActionUIUXHandler;

            PlayerUnitActionUIUXHandler.IPlayerUnitActionPanelUIMediator.Clear_PlayerUnitActionPanel();
        }

        // 모든 AI 순차 실행.
        public void Notify_OperateAISequencer()
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            var IEnemyAISequencer = HandlerManager.GetDynamicDataHandler<GameSystems.EnemySystem.EnemySystemHandler>().IEnemyAISequencer;

            // Enemy 순서 명시 -> Enemy 순차 실행 + 대기
            IEnemyAISequencer.AllocateEnemyAISequence();
            StartCoroutine(IEnemyAISequencer.ExecuteEnemyAI_Coroutine());
        }
        public void Notify_OperateNewTurnSetting_Sequencer()
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            var EnemyUnitManagerDataDBHandler = HandlerManager.GetDynamicDataHandler<GameSystems.EnemySystem.EnemyUnitSystem.EnemyUnitManagerDataDBHandler>();

            if(EnemyUnitManagerDataDBHandler.TryGetAll(out var enemyUnitManagerDatas))
            {
                foreach (var enemyUnitManager in enemyUnitManagerDatas)
                {
                    enemyUnitManager.EnemyUnitFeatureInterfaceGroup.EnemyUnitManager.OperateTurnEndSetting();
                }
            }
        }
    }

    [System.Serializable]
    public class ScannerTestData
    {
        [SerializeField] public int ScannerID;
        [SerializeField] public int VisibleSize;
        [SerializeField] public int VisibleOvercomeWeight;
        [SerializeField] public int PhysicalVisionOvercomeWeight;
        [SerializeField] public int SpiritualVisionOvercomeWeight;
    }

    [System.Serializable]
    public class UnitSpawnEvent_Test
    {
        [SerializeField] public int UnitID;
        [SerializeField] public Vector2Int SpawnPosition;
    }


}