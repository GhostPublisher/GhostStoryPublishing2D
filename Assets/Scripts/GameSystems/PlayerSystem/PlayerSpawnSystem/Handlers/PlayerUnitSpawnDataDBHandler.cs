using System;

using UnityEngine;

using Foundations.Architecture.ReferencesHandler;

namespace GameSystems.PlayerSystem.PlayerSpawnSystem
{
    public class PlayerUnitSpawnDataDBHandler : IStaticReferenceHandler
    {
        private string PlayerUnitSpawnDataGroupDB_Stage_SOPath = "ScriptableObject/PlayerSpawn/PlayerUnitSpawnDataGroupDB_Stage";
        private string PlayerUnitSpawnDataGroupDB_Trigger_SOPath = "ScriptableObject/PlayerSpawn/PlayerUnitSpawnDataGroupDB_Trigger";

        // PlayerUnitSpawnDataGroupDB_Stage SO �ε� �õ�.
        private bool TryLoadScriptableObject_Stage(out PlayerUnitSpawnDataGroupDB_Stage playerUnitSpawnDataGroupDB_Stage)
        {
            playerUnitSpawnDataGroupDB_Stage = Resources.Load<PlayerUnitSpawnDataGroupDB_Stage>(this.PlayerUnitSpawnDataGroupDB_Stage_SOPath);

            // ���� ��ã���� false ����.
            if (playerUnitSpawnDataGroupDB_Stage == null) return false;
            return true;
        }
        // PlayerUnitSpawnDataGroupDB_Trigger SO �ε� �õ�.
        private bool TryLoadScriptableObject_Trigger(out PlayerUnitSpawnDataGroupDB_Trigger playerUnitSpawnDataGroupDB_Trigger)
        {
            playerUnitSpawnDataGroupDB_Trigger = Resources.Load<PlayerUnitSpawnDataGroupDB_Trigger>(this.PlayerUnitSpawnDataGroupDB_Trigger_SOPath);

            // ���� ��ã���� false ����.
            if (playerUnitSpawnDataGroupDB_Trigger == null) return false;
            return true;
        }

        // StageID�� �ش�Ǵ� StageSetting���� ���� PlayerUnitSpawn ���� �����մϴ�.
        public bool TryGetPlayerUnitSpawnData_Stage(int stageID, out PlayerUnitSpawnData_Stage playerUnitSpawnData_Stage)
        {
            // SO�� ��ã�� ��� false ����.
            if (!this.TryLoadScriptableObject_Stage(out var playerUnitSpawnDataGroupDB_Stage))
            {
                Debug.LogError($"[PlayerUnitSpawnDataDBHandler] SO ��ġ �� ã��.");
                playerUnitSpawnData_Stage = null;
                return false;
            }

            // SO �ȿ� StageID�� �ش�Ǵ� �����Ͱ� ���� ��� false ����.
            if (!playerUnitSpawnDataGroupDB_Stage.TryGetPlayerUnitSpawnDataGroup_Stage(stageID, out var playerUnitSpawnDataGroup_Stage))
            {
                Debug.LogError($"[PlayerUnitSpawnDataDBHandler] SO �ȿ� StageID �ش� �����Ͱ� ����. StageID : {stageID}");
                playerUnitSpawnData_Stage = null;
                return false;
            }

            // SO �ȿ� StageID�� �ش�Ǵ� �����ʹ� �־��µ�, Json ������ �������� ���.
            if (playerUnitSpawnDataGroup_Stage.PlayerUnitSpawnData_StageJsonFile == null)
            {
                Debug.LogError($"[PlayerUnitSpawnDataDBHandler] JsonFile�� ��� ����. StageID : {stageID}");
                playerUnitSpawnData_Stage = null;
                return false;
            }

            // StageID�� �ش�Ǵ� 'Stage Enemy ���� JsonFile'�� �����ͼ� Parsing �۾�.
            var playerUnitSpawnData_Stage_Parsed = JsonUtility.FromJson<PlayerUnitSpawnData_Stage>(playerUnitSpawnDataGroup_Stage.PlayerUnitSpawnData_StageJsonFile.text);
            if (playerUnitSpawnData_Stage_Parsed == null || playerUnitSpawnData_Stage_Parsed.UnitSpawnDatas == null)
            {
                Debug.LogError($"[PlayerUnitSpawnDataDBHandler] Json �Ľ� ����. StageID: {stageID}");
                playerUnitSpawnData_Stage = null;
                return false;
            }

            playerUnitSpawnData_Stage = playerUnitSpawnData_Stage_Parsed;
            return true;
        }
        // StageID�� �ش�Ǵ� TurnStartSetting���� ���� PlayerUnitSpawn ���� �����մϴ�.
        public bool TryGetPlayerUnitSpawnDataGroup_Trigger(int stageID, out PlayerUnitSpawnData_Trigger[] playerUnitSpawnData_Triggers)
        {
            // SO�� ��ã�� ��� false ����.
            if (!this.TryLoadScriptableObject_Trigger(out var playerUnitSpawnDataGroupDB_Trigger))
            {
                Debug.LogError($"[PlayerUnitSpawnDataDBHandler] SO ��ġ �� ã��.");
                playerUnitSpawnData_Triggers = null;
                return false;
            }

            // SO �ȿ� StageID�� �ش�Ǵ� �����Ͱ� ���� ��� false ����.
            if (!playerUnitSpawnDataGroupDB_Trigger.TryGetPlayerUnitSpawnDataGroup_Trigger(stageID, out var playerUnitSpawnDataGroup_Trigger))
            {
                Debug.LogWarning($"[PlayerUnitSpawnDataDBHandler] SO �ȿ� StageID �ش� �����Ͱ� ����. StageID : {stageID}");
                playerUnitSpawnData_Triggers = null;
                return false;
            }

            // SO �ȿ� StageID�� �ش�Ǵ� �����ʹ� �־��µ�, Json ������ �������� ���.
            if (playerUnitSpawnDataGroup_Trigger.PlayerUnitSpawnData_TriggerJsonFile == null)
            {
                Debug.LogError($"[PlayerUnitSpawnDataDBHandler] JsonFile�� ��� ����. StageID : {stageID}");
                playerUnitSpawnData_Triggers = null;
                return false;
            }

            // StageID�� �ش�Ǵ� 'Trigger Enemy ���� JsonFile'�� �����ͼ� Parsing �۾�.
            var playerUnitSpawnData_TriggerArrayWrapper = JsonUtility.FromJson<PlayerUnitSpawnData_TriggerArrayWrapper>(playerUnitSpawnDataGroup_Trigger.PlayerUnitSpawnData_TriggerJsonFile.text);
            if (playerUnitSpawnData_TriggerArrayWrapper == null || playerUnitSpawnData_TriggerArrayWrapper.PlayerUnitSpawnData_Triggers == null)
            {
                Debug.LogError($"[PlayerUnitSpawnDataDBHandler] Json �Ľ� ����. StageID: {stageID}");
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
