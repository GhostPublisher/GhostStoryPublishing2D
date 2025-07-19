using System;
using System.Collections.Generic;

using UnityEngine;

using Foundations.Architecture.EventObserver;

namespace GameSystems.EnemySystem.EnemyVisibilitySystem
{
    // 초기 설정
    [Serializable]
    public class EnemyVisibilitySystemInitialSetting : IEventData
    {

    }

    // 플레이어 유닛에 의한 Enemy 보여지는 상태
    [Serializable]
    public class UpdatePlayerVisibleData_ForEnemyVisibility : IEventData
    {
        public int PlayerUniqueID;

        public HashSet<Vector2Int> VisibleRange;

        public int PhysicalVisionOvercomeWeight;
        public int SpiritualVisionOvercomeWeight;

        public UpdatePlayerVisibleData_ForEnemyVisibility()
        {
            this.VisibleRange = new();
        }
    }
    [Serializable]
    public class RemovePlayerVisibleData_ForEnemyVisibility : IEventData
    {
        public int PlayerUniqueID;
    }

    // Scanner에 의한 Enemy 보여지는 상태
    [Serializable]
    public class UpdateScanerVisibleData_ForEnemyVisibility : IEventData
    {
        public int ScannerUniqueID;

        public HashSet<Vector2Int> VisibleRange;

        public int PhysicalVisionOvercomeWeight;
        public int SpiritualVisionOvercomeWeight;

        public UpdateScanerVisibleData_ForEnemyVisibility()
        {
            this.VisibleRange = new();
        }
    }
    [Serializable]
    public class RemoveScanerVisibleData_ForEnemyVisibility : IEventData
    {
        public int ScannerUniqueID;
    }


    // Test를 위한 Event
    [Serializable]
    public class ShowAllEnemyVisibility : IEventData { }
    [Serializable]
    public class HideAllEnemyVisibility : IEventData { }
    [Serializable]
    public class ClearEnemyVisibilityData : IEventData { }


    // 이건 좀 더 생각해 봐야 할 듯.
    // 이유 : Enemy가 이동의 경우, 시야 밖에서 이동할 때, 먼저 활성화 되고 들어와야 됨.
    //      반면 시야 밖으로 나갈때는 나간다음, 비활성화 되어야 됨.
    //      뭔가 좀 걸림. 아마도 새로운 메소드들이 필요할 듯. ( 양방향으로 )

    // Enemy 유닛 행동에 의한 Enemy 보여짐.
    // Enemy 생성.
    [Serializable]
    public class UpdateEnemyVisibility_ForEnemySpawn : IEventData
    {
        public int EnemyUniqueID;

        public Vector2Int SpawnPosition;

        public int PhysicalVisionBlockWeight;
        public int SpiritualVisionBlockWeight;
    }
    // Enemy 이동.
    [Serializable]
    public class UpdateEnemyVisibility_ForEnemyMove : IEventData
    {
        public int EnemyUniqueID;

        public Vector2Int CurrentPosition;
        public Vector2Int NextPosition;

        public int PhysicalVisionBlockWeight;
        public int SpiritualVisionBlockWeight;
    }
    // Enemy 가중치 변화.
    [Serializable]
    public class UpdateEnemyVisibility_ForEnemyBlockWeightUpdate : IEventData
    {
        public int EnemyUniqueID;

        public Vector2Int CurrentPosition;

        public int PhysicalVisionBlockWeight;
        public int SpiritualVisionBlockWeight;
    }
}