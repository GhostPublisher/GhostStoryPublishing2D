/*using System;
using System.Collections.Generic;

using UnityEngine;

using Foundations.Architecture.EventObserver;

namespace GameSystems.UnitSystem
{
    public class MemorizedDataPlugIn_DetectedPlayerUnit : MonoBehaviour, IMemorizedDataProcessorPlugIn
    {
        [SerializeField] private int MemoryCount;

        private DirectDetectionData_PlayerUnit DirectDetectionData_PlayerUnit;
        private MemorizedDataGroup_DetectedPlayerUnit memorizedDataGroup_DetectedPlayerUnit = new();

        public void InitialSetting(IMemorizedDataGroup memorizedDataGroup)
        {
            if (memorizedDataGroup.DirectDetectionDatas.TryGetValue(typeof(DirectDetectionData_PlayerUnit), out var detectedData))
            {
                if (detectedData is DirectDetectionData_PlayerUnit data)
                {
                    // 해당 PlugIn이 사용하는 데이터의 가공에 필요한 데이터를 등록해 놓음.
                    this.DirectDetectionData_PlayerUnit = data;
                }
            }

            // 공통 데이터에 등록.
            memorizedDataGroup.MemorizedDatas.Add(this.memorizedDataGroup_DetectedPlayerUnit.GetType(), this.memorizedDataGroup_DetectedPlayerUnit);
        }

        public void UpdateMemorizedData()
        {
            // 시야에 보이는 PlayerUnit 값들을 가져옴.
            foreach (var uniqueIDAndPosition in DirectDetectionData_PlayerUnit.DetectedPlayerDatas)
            {
                // 기억에 있는 데이터 갱신. ( 동일한 유닛의 경우 위치값과 기억 기간 갱신 )
                if(this.memorizedDataGroup_DetectedPlayerUnit.MemorizedData_DetectedPlayerUnits.TryGetValue(uniqueIDAndPosition.Key, out var memmoryDetectedData))
                {
                    memmoryDetectedData.UnitPosition = uniqueIDAndPosition.Value;
                    memmoryDetectedData.RemainMemoryCount = this.MemoryCount;
                }
                // 새로운 데이터의 경우 추가.
                else
                {
                    MemorizedData_DetectedPlayerUnit newMemoryData = new();
                    newMemoryData.UniqueID = uniqueIDAndPosition.Key;
                    newMemoryData.UnitPosition = uniqueIDAndPosition.Value;
                    newMemoryData.RemainMemoryCount = this.MemoryCount;

                    this.memorizedDataGroup_DetectedPlayerUnit.MemorizedData_DetectedPlayerUnits.Add(uniqueIDAndPosition.Key, newMemoryData);
                }
            }
        }
        public void UpdateMemorizedData_TurnOver()
        {
            HashSet<int> removeIDs = new();

            foreach (var momorizedData in this.memorizedDataGroup_DetectedPlayerUnit.MemorizedData_DetectedPlayerUnits.Values)
            {
                // 다음 Turn에 기억 횟수가 0미만이 되면, 삭제.
                if (0 > momorizedData.RemainMemoryCount - 1)
                {
                    removeIDs.Add( momorizedData.UniqueID);
                }
            }

            foreach (int id in removeIDs)
            {
                this.memorizedDataGroup_DetectedPlayerUnit.MemorizedData_DetectedPlayerUnits.Remove(id);
            }
        }
        public void UpdateMemorizedData(IEventData eventData)
        {
            throw new NotImplementedException();
        }
    }

    public class MemorizedDataGroup_DetectedPlayerUnit : IMemorizedData
    {
       public Dictionary<int, MemorizedData_DetectedPlayerUnit> MemorizedData_DetectedPlayerUnits { get; }

       public MemorizedDataGroup_DetectedPlayerUnit()
        {
            this.MemorizedData_DetectedPlayerUnits = new();
        }

    }

    [Serializable]
    public class MemorizedData_DetectedPlayerUnit
    {
        public int UniqueID;

        public Vector2Int UnitPosition;

        public int RemainMemoryCount;
    }
}*/