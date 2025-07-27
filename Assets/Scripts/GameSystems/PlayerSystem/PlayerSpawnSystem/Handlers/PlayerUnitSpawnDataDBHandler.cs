using System;

using UnityEngine;

using Foundations.Architecture.ReferencesHandler;

namespace GameSystems.PlayerSystem.PlayerSpawnSystem
{
    public class PlayerUnitSpawnDataDBHandler : IStaticReferenceHandler
    {
        private string PlayerUnitSpawnDataGroupDB_Stage_SOPath = "ScriptableObject/PlayerSpawn/PlayerUnitSpawnDataGroupDB_Stage";
        private string PlayerUnitSpawnDataGroupDB_Trigger_SOPath = "ScriptableObject/PlayerSpawn/PlayerUnitSpawnDataGroupDB_Trigger";

        // PlayerUnitSpawnDataGroupDB_Stage SO 로드 시도.
        private bool TryLoadScriptableObject_Stage(out PlayerUnitSpawnDataGroupDB_Stage playerUnitSpawnDataGroupDB_Stage)
        {
            playerUnitSpawnDataGroupDB_Stage = Resources.Load<PlayerUnitSpawnDataGroupDB_Stage>(this.PlayerUnitSpawnDataGroupDB_Stage_SOPath);

            // 파일 못찾으면 false 리턴.
            if (playerUnitSpawnDataGroupDB_Stage == null) return false;
            return true;
        }
        // PlayerUnitSpawnDataGroupDB_Trigger SO 로드 시도.
        private bool TryLoadScriptableObject_Trigger(out PlayerUnitSpawnDataGroupDB_Trigger playerUnitSpawnDataGroupDB_Trigger)
        {
            playerUnitSpawnDataGroupDB_Trigger = Resources.Load<PlayerUnitSpawnDataGroupDB_Trigger>(this.PlayerUnitSpawnDataGroupDB_Trigger_SOPath);

            // 파일 못찾으면 false 리턴.
            if (playerUnitSpawnDataGroupDB_Trigger == null) return false;
            return true;
        }

        // StageID에 해당되는 StageSetting에서 사용될 PlayerUnitSpawn 값을 리턴합니다.
        public bool TryGetPlayerUnitSpawnData_Stage(int stageID, out PlayerUnitSpawnData_Stage playerUnitSpawnData_Stage)
        {
            // SO를 못찾을 경우 false 리턴.
            if (!this.TryLoadScriptableObject_Stage(out var playerUnitSpawnDataGroupDB_Stage))
            {
                Debug.LogError($"[PlayerUnitSpawnDataDBHandler] SO 위치 못 찾음.");
                playerUnitSpawnData_Stage = null;
                return false;
            }

            // SO 안에 StageID에 해당되는 데이터가 없을 경우 false 리턴.
            if (!playerUnitSpawnDataGroupDB_Stage.TryGetPlayerUnitSpawnDataGroup_Stage(stageID, out var playerUnitSpawnDataGroup_Stage))
            {
                Debug.LogError($"[PlayerUnitSpawnDataDBHandler] SO 안에 StageID 해당 데이터가 없음. StageID : {stageID}");
                playerUnitSpawnData_Stage = null;
                return false;
            }

            // SO 안에 StageID에 해당되는 데이터는 넣었는데, Json 파일을 누락했을 경우.
            if (playerUnitSpawnDataGroup_Stage.PlayerUnitSpawnData_StageJsonFile == null)
            {
                Debug.LogError($"[PlayerUnitSpawnDataDBHandler] JsonFile이 비어 있음. StageID : {stageID}");
                playerUnitSpawnData_Stage = null;
                return false;
            }

            // StageID에 해당되는 'Stage Enemy 생성 JsonFile'을 가져와서 Parsing 작업.
            var playerUnitSpawnData_Stage_Parsed = JsonUtility.FromJson<PlayerUnitSpawnData_Stage>(playerUnitSpawnDataGroup_Stage.PlayerUnitSpawnData_StageJsonFile.text);
            if (playerUnitSpawnData_Stage_Parsed == null || playerUnitSpawnData_Stage_Parsed.UnitSpawnDatas == null)
            {
                Debug.LogError($"[PlayerUnitSpawnDataDBHandler] Json 파싱 실패. StageID: {stageID}");
                playerUnitSpawnData_Stage = null;
                return false;
            }

            playerUnitSpawnData_Stage = playerUnitSpawnData_Stage_Parsed;
            return true;
        }
        // StageID에 해당되는 TurnStartSetting에서 사용될 PlayerUnitSpawn 값을 리턴합니다.
        public bool TryGetPlayerUnitSpawnDataGroup_Trigger(int stageID, out PlayerUnitSpawnData_Trigger[] playerUnitSpawnData_Triggers)
        {
            // SO를 못찾을 경우 false 리턴.
            if (!this.TryLoadScriptableObject_Trigger(out var playerUnitSpawnDataGroupDB_Trigger))
            {
                Debug.LogError($"[PlayerUnitSpawnDataDBHandler] SO 위치 못 찾음.");
                playerUnitSpawnData_Triggers = null;
                return false;
            }

            // SO 안에 StageID에 해당되는 데이터가 없을 경우 false 리턴.
            if (!playerUnitSpawnDataGroupDB_Trigger.TryGetPlayerUnitSpawnDataGroup_Trigger(stageID, out var playerUnitSpawnDataGroup_Trigger))
            {
                Debug.LogWarning($"[PlayerUnitSpawnDataDBHandler] SO 안에 StageID 해당 데이터가 없음. StageID : {stageID}");
                playerUnitSpawnData_Triggers = null;
                return false;
            }

            // SO 안에 StageID에 해당되는 데이터는 넣었는데, Json 파일을 누락했을 경우.
            if (playerUnitSpawnDataGroup_Trigger.PlayerUnitSpawnData_TriggerJsonFile == null)
            {
                Debug.LogError($"[PlayerUnitSpawnDataDBHandler] JsonFile이 비어 있음. StageID : {stageID}");
                playerUnitSpawnData_Triggers = null;
                return false;
            }

            // StageID에 해당되는 'Trigger Enemy 생성 JsonFile'을 가져와서 Parsing 작업.
            var playerUnitSpawnData_TriggerArrayWrapper = JsonUtility.FromJson<PlayerUnitSpawnData_TriggerArrayWrapper>(playerUnitSpawnDataGroup_Trigger.PlayerUnitSpawnData_TriggerJsonFile.text);
            if (playerUnitSpawnData_TriggerArrayWrapper == null || playerUnitSpawnData_TriggerArrayWrapper.PlayerUnitSpawnData_Triggers == null)
            {
                Debug.LogError($"[PlayerUnitSpawnDataDBHandler] Json 파싱 실패. StageID: {stageID}");
                playerUnitSpawnData_Triggers = null;
                return false;
            }

            playerUnitSpawnData_Triggers = playerUnitSpawnData_TriggerArrayWrapper.PlayerUnitSpawnData_Triggers;
            return true;
        }
    }

    [Serializable]
    public class UnitSpawnData
    {
        public int UnitID;

        public int SpawnPositionX;
        public int SpawnPositionY;
    }
}
