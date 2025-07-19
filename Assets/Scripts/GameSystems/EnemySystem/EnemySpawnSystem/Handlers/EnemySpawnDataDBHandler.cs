using System;

using UnityEngine;

using Foundations.Architecture.ReferencesHandler;

namespace GameSystems.EnemySystem.EnemySpawnSystem
{
    [Serializable]
    public class EnemySpawnDataDBHandler : IStaticReferenceHandler
    {
        private StageEnemySpawnData_TurnGroup StageEnemySpawnData_TurnGroup;
        private StageEnemySpawnData_TriggerGroup StageEnemySpawnData_TriggerGroup;

        private bool isInitialized = false;

        // Json 파일 1개 - Stage 1개 들어가 있음.
        private void LoadScriptableObject()
        {
            this.StageEnemySpawnData_TurnGroup = Resources.Load<StageEnemySpawnData_TurnGroup>("ScriptableObject/EnemySpawn/StageEnemySpawnData_TurnGroup");
            this.StageEnemySpawnData_TriggerGroup = Resources.Load<StageEnemySpawnData_TriggerGroup>("ScriptableObject/EnemySpawn/StageEnemySpawnData_TriggerGroup");

            this.isInitialized = true;
        }

        public bool TryGetEnemySpawnDatas_TurnID(int stageID, out EnemySpawnData_Turn[] enemySpawnDatas_Turn)
        {
            if (!this.isInitialized) this.LoadScriptableObject();

            StageEnemySpawnData_Turn stageEnemySpawnData_Turn = this.StageEnemySpawnData_TurnGroup.GetStageEnemySpawnData_Turn(stageID);
            TextAsset enemySpawnDatasJsonFile = stageEnemySpawnData_Turn.EnemySpawnData_TurnJsonFile;

            EnemySpawnData_TurnArrayWrapper enemySpawnDataArrayWrapper = JsonUtility.FromJson<EnemySpawnData_TurnArrayWrapper>(enemySpawnDatasJsonFile.text);
            if (enemySpawnDataArrayWrapper == null || enemySpawnDataArrayWrapper.EnemySpawnData_Turns == null)
            {
                Debug.LogError($"[EnemySpawnDataDBHandler] Json 파싱 실패 - StageID: {stageID}");

                enemySpawnDatas_Turn = default;
                return false;
            }

            enemySpawnDatas_Turn = enemySpawnDataArrayWrapper.EnemySpawnData_Turns;
            return true;
        }

        public bool TryGetEnemySpawnDatas_TriggerID(int stageID, out EnemySpawnData_Trigger[] enemySpawnDatas_Trigger)
        {
            if (!this.isInitialized) this.LoadScriptableObject();

            StageEnemySpawnData_Trigger stageEnemySpawnData_Trigger = this.StageEnemySpawnData_TriggerGroup.GetStageEnemySpawnData_Trigger(stageID);
            TextAsset enemySpawnDatasJsonFile = stageEnemySpawnData_Trigger.EnemySpawnData_TriggerJsonFile;

            EnemySpawnData_TriggerArrayWrapper enemySpawnDataArrayWrapper = JsonUtility.FromJson<EnemySpawnData_TriggerArrayWrapper>(enemySpawnDatasJsonFile.text);
            if (enemySpawnDataArrayWrapper == null || enemySpawnDataArrayWrapper.EnemySpawnData_Triggers == null)
            {
                Debug.LogError($"[EnemySpawnDataDBHandler] Json 파싱 실패 - StageID: {stageID}");

                enemySpawnDatas_Trigger = default;
                return false;
            }

            enemySpawnDatas_Trigger = enemySpawnDataArrayWrapper.EnemySpawnData_Triggers;
            return true;
        }
    }

    [Serializable]
    public class EnemySpawnData
    {
        public int UnitID;

        public int EnemySpawnPositionX;
        public int EnemySpawnPositionY;
    }
}