using UnityEngine;

using Foundations.Architecture.EventObserver;

namespace GameSystems.TerrainSystem
{
    public class TerrainDataControllerEventListener : MonoBehaviour, IEventObserverListener
    {
        private IEventObserverLinker EventObserverLinker;

        [SerializeField] private TerrainDataController TerrainDataController;

        private void Awake()
        {
            this.EventObserverLinker = new EventObserverLinker(this);
        }

        private void OnEnable()
        {
            this.EventObserverLinker.RegisterSubscriberListener<InitialSetGeneratedTerrainDataEvent>();
            this.EventObserverLinker.RegisterSubscriberListener<InitialSetGeneratedTerrainDataEvent_Test>();
            this.EventObserverLinker.RegisterSubscriberListener<ClearTrerrainData>();
        }

        private void OnDisable()
        {
            this.EventObserverLinker.Dispose();
        }

        public void UpdateData(IEventData eventData)
        {
            switch (eventData)
            {
                case InitialSetGeneratedTerrainDataEvent:
                    var data01 = (InitialSetGeneratedTerrainDataEvent)eventData;
                    this.TerrainDataController.InitialSetGeneratedTerrainData(data01.StageID);
                    break;
                case InitialSetGeneratedTerrainDataEvent_Test:
                    var data02 = (InitialSetGeneratedTerrainDataEvent_Test)eventData;
                    this.TerrainDataController.InitialSetGeneratedTerrainData_Test(data02.Width, data02.Height);
                    this.TerrainDataController.OverwriteSetGeneratedTerrainData_Test(data02.TerrainDatas);
                    break;
                case ClearTrerrainData:
                    this.TerrainDataController.ClearTrerrainData();
                    break;
                default:
                    break;
            }
        }
    }
}