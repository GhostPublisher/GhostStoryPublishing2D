/*using System;
using System.Collections.Generic;
using UnityEngine;

using GameSystems.PlayerSystem;

namespace GameSystems.UnitSystem
{
    public class DirectDetectionDataProcessorPlugIn_PlayerUnit : MonoBehaviour, IDirectDetectionDataProcessorPlugIn
    {
        private InteractionRangeData_Visible InteractionRangeData_Visible;
        private DirectDetectionData_PlayerUnit directDetectionData_PlayerUnit = new();

        private PlayerUnitManagerDataDBHandler PlayerUnitManagerDataDBHandler;

        public void InitialSetting(IDirectDetectionDataGroup directDetectionDataGroup)
        {
            if (directDetectionDataGroup.InteractionRangeDatas.TryGetValue(typeof(InteractionRangeData_Visible), out var rangeData))
            {
                if (rangeData is InteractionRangeData_Visible visibleData)
                {
                    // �ش� PlugIn�� ����ϴ� �������� ������ �ʿ��� �����͸� ����� ����.
                    this.InteractionRangeData_Visible = visibleData;
                }
            }

            // ���� �����Ϳ� ���.
            directDetectionDataGroup.DirectDetectionDatas.Add(this.directDetectionData_PlayerUnit.GetType(), this.directDetectionData_PlayerUnit);
        }

        public void UpdateDirectDetectionData()
        {
            this.directDetectionData_PlayerUnit.DetectedPlayerDatas.Clear();

            // �þ� ���� ���� Player Unit�� �ִ��� �ݺ�.
            foreach (Vector2Int pos in InteractionRangeData_Visible.AvailableRange)
            {
                // ������ �߰�.
                if (this.PlayerUnitManagerDataDBHandler.TryGetPlayerUnitManagerData(pos, out var generatedPlayerUnitData))
                {
                    this.directDetectionData_PlayerUnit.DetectedPlayerDatas.Add(generatedPlayerUnitData.UniqueID, pos);
                }
            }
        }
    }

    [Serializable]
    public class DirectDetectionData_PlayerUnit : IDirectDetectionData
    {
        public Dictionary<int, Vector2Int> DetectedPlayerDatas { get; }

        public DirectDetectionData_PlayerUnit()
        {
            this.DetectedPlayerDatas = new();
        }
    }
}*/