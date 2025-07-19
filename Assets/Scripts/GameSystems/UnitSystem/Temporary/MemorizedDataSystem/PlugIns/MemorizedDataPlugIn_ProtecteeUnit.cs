/*using System;
using System.Collections.Generic;

using UnityEngine;

using Foundations.Architecture.EventObserver;
using Foundations.Architecture.ReferencesHandler;

using GameSystems.EnemySystem.EnemyUnitSystem;

namespace GameSystems.UnitSystem
{
    public class MemorizedDataPlugIn_ProtecteeUnit : MonoBehaviour, IMemorizedDataProcessorPlugIn
    {
        [SerializeField] private int ProtecteeUnitID;
        [SerializeField] private int MemoryCount;

        private EnemyUnitManagerDataDBHandler GeneratedEnemyUnitDataGroupHandler;
        private MemorizedDataGroup_ProtecteeUnit MemorizedDataGroup_ProtecteeUnit = new();

        public void InitialSetting(IMemorizedDataGroup memorizedDataGroup)
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            this.GeneratedEnemyUnitDataGroupHandler = HandlerManager.GetDynamicDataHandler<EnemyUnitManagerDataDBHandler>();

            // 공통 데이터에 등록.
            memorizedDataGroup.MemorizedDatas.Add(this.MemorizedDataGroup_ProtecteeUnit.GetType(), this.MemorizedDataGroup_ProtecteeUnit);
        }

        public void UpdateMemorizedData()
        {
            if (this.GeneratedEnemyUnitDataGroupHandler.TryGetAll(out var enemyDatas))
            {
                foreach (var data in enemyDatas)
                {
                    if (data.UnitData.UnitID == ProtecteeUnitID)
                    {
                        MemorizedData_ProtecteeUnit newMemoryData = new();
                        newMemoryData.UniqueID = data.UniqueID;
                        newMemoryData.UnitPosition = data.UnitGridPosition;
                        newMemoryData.RemainMemoryCount = this.MemoryCount;

                        this.MemorizedDataGroup_ProtecteeUnit.MemorizedData_ProtecteeUnits.Add(newMemoryData.UniqueID, newMemoryData);
                    }
                }
            }
        }

        public void UpdateMemorizedData(IEventData eventData)
        {
            throw new NotImplementedException();
        }

        public void UpdateMemorizedData_TurnOver()
        {
            throw new NotImplementedException();
        }
    }

    public class MemorizedDataGroup_ProtecteeUnit : IMemorizedData
    {
        public Dictionary<int, MemorizedData_ProtecteeUnit> MemorizedData_ProtecteeUnits { get; }

        public MemorizedDataGroup_ProtecteeUnit()
        {
            this.MemorizedData_ProtecteeUnits = new();
        }
    }

    public class MemorizedData_ProtecteeUnit
    {
        public int UniqueID;

        public Vector2Int UnitPosition;

        public int RemainMemoryCount;
    }
}*/