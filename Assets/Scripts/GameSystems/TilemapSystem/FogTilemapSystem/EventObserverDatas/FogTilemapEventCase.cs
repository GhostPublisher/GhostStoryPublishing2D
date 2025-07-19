using System;
using System.Collections.Generic;

using UnityEngine;

using Foundations.Architecture.EventObserver;

namespace GameSystems.TilemapSystem.FogTilemap
{
    // Fog 초기 설정.
    [Serializable]
    public class FogTilemapInitialSettingEvent : IEventData
    {
        public int StageID;
    }

    // 플레이어 유닛에 의한 Enemy 보여짐.
    [Serializable]
    public class UpdatePlayerVisibleData_ForFogVisibility : IEventData
    {
        public int PlayerUniqueID;

        public HashSet<Vector2Int> VisibleRange;

        public UpdatePlayerVisibleData_ForFogVisibility()
        {
            this.VisibleRange = new();
        }
    }
    [Serializable]
    public class RemovePlayerVisibleData_ForFogVisibility : IEventData
    {
        public int PlayerUniqueID;
    }

    // Scanner에 의해 보여지는 시야.
    [Serializable]
    public class UpdateScanerVisibleData_ForFogVisibility : IEventData
    {
        public int ScannerUniqueID;

        public HashSet<Vector2Int> VisibleRange;

        public UpdateScanerVisibleData_ForFogVisibility()
        {
            this.VisibleRange = new();
        }
    }
    [Serializable]
    public class RemoveScanerVisibleData_ForFogVisibility : IEventData
    {
        public int ScannerUniqueID;
    }






    // Test를 위한 Event;
    // 일반화 되지 않은 Raw 데이터를 통한 초기 설정 요청.
    [Serializable]
    public class InitialSetFogTilemapEvent_Raw : IEventData
    {
        public int Width;
        public int Height;
    }
    // Fog 설정 초기화.
    [Serializable]
    public class ClearFogTilemapEvent : IEventData { }
    // Fog Tilemap 활성화.
    [Serializable]
    public class ShowAllFogTilemap : IEventData { }
    // Fog Tilemap 비활성화.
    [Serializable]
    public class HideAllFogTilemap : IEventData { }



    // 일반화 되지 않은 Raw 데이터를 통한 Update 요청.
    [Serializable]
    public class UpdateScanerRawVisibleData_ForFogVisibility : IEventData
    {
        public int ScannerUniqueID;
        public Vector2Int TargetPosition;

        public int VisibleSize;
        public int VisibleOvercomeWeight;
    }
}
