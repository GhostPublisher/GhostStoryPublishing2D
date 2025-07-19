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
                    // �ش� PlugIn�� ����ϴ� �������� ������ �ʿ��� �����͸� ����� ����.
                    this.DirectDetectionData_PlayerUnit = data;
                }
            }

            // ���� �����Ϳ� ���.
            memorizedDataGroup.MemorizedDatas.Add(this.memorizedDataGroup_DetectedPlayerUnit.GetType(), this.memorizedDataGroup_DetectedPlayerUnit);
        }

        public void UpdateMemorizedData()
        {
            // �þ߿� ���̴� PlayerUnit ������ ������.
            foreach (var uniqueIDAndPosition in DirectDetectionData_PlayerUnit.DetectedPlayerDatas)
            {
                // ��￡ �ִ� ������ ����. ( ������ ������ ��� ��ġ���� ��� �Ⱓ ���� )
                if(this.memorizedDataGroup_DetectedPlayerUnit.MemorizedData_DetectedPlayerUnits.TryGetValue(uniqueIDAndPosition.Key, out var memmoryDetectedData))
                {
                    memmoryDetectedData.UnitPosition = uniqueIDAndPosition.Value;
                    memmoryDetectedData.RemainMemoryCount = this.MemoryCount;
                }
                // ���ο� �������� ��� �߰�.
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
                // ���� Turn�� ��� Ƚ���� 0�̸��� �Ǹ�, ����.
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