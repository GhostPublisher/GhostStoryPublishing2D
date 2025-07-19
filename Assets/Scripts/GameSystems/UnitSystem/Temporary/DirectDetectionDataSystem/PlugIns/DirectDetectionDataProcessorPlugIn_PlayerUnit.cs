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
                    // 해당 PlugIn이 사용하는 데이터의 가공에 필요한 데이터를 등록해 놓음.
                    this.InteractionRangeData_Visible = visibleData;
                }
            }

            // 공통 데이터에 등록.
            directDetectionDataGroup.DirectDetectionDatas.Add(this.directDetectionData_PlayerUnit.GetType(), this.directDetectionData_PlayerUnit);
        }

        public void UpdateDirectDetectionData()
        {
            this.directDetectionData_PlayerUnit.DetectedPlayerDatas.Clear();

            // 시야 범위 내에 Player Unit이 있는지 반복.
            foreach (Vector2Int pos in InteractionRangeData_Visible.AvailableRange)
            {
                // 있으면 추가.
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