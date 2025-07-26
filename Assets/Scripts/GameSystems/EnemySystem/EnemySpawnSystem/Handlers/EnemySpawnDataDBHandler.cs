using System;

using UnityEngine;

using Foundations.Architecture.ReferencesHandler;

namespace GameSystems.EnemySystem.EnemySpawnSystem
{
    [Serializable]
    public class EnemySpawnDataDBHandler : IStaticReferenceHandler
    {
        private string StageEnemySpawnData_StageGroupScriptableObjectPath = "ScriptableObject/EnemySpawn/StageEnemySpawnData_StageGroup";
        private string StageEnemySpawnData_TurnGroupScriptableObjectPath = "ScriptableObject/EnemySpawn/StageEnemySpawnData_TurnGroup";
        private string StageEnemySpawnData_TriggerGroupScriptableObjectPath = "ScriptableObject/EnemySpawn/StageEnemySpawnData_TriggerGroup";

        // Stage Enemy Spawn DB SO 로드 시도.
        private bool TryLoadScriptableObject_Stage(out StageEnemySpawnData_StageGroup stageEnemySpawnData_Stage)
        {
            stageEnemySpawnData_Stage = Resources.Load<StageEnemySpawnData_StageGroup>(this.StageEnemySpawnData_StageGroupScriptableObjectPath);

            // 파일 못찾으면 false 리턴.
            if (stageEnemySpawnData_Stage == null) return false;
            return true;
        }
        // Turn Enemy Spawn DB SO 로드 시도.
        private bool TryLoadScriptableObject_Turn(out StageEnemySpawnData_TurnGroup stageEnemySpawnData_TurnGroup)
        {
            stageEnemySpawnData_TurnGroup = Resources.Load<StageEnemySpawnData_TurnGroup>(this.StageEnemySpawnData_TurnGroupScriptableObjectPath);

            // 파일 못찾으면 false 리턴.
            if (stageEnemySpawnData_TurnGroup == null) return false;
            return true;
        }
        // Trigger Enemy Spawn DB SO 로드 시도.
        private bool TryLoadScriptableObject_Trigger(out StageEnemySpawnData_TriggerGroup stageEnemySpawnData_Trigger)
        {
            stageEnemySpawnData_Trigger = Resources.Load<StageEnemySpawnData_TriggerGroup>(this.StageEnemySpawnData_TriggerGroupScriptableObjectPath);

            // 파일 못찾으면 false 리턴.
            if (stageEnemySpawnData_Trigger == null) return false;
            return true;
        }

        // StageID에 해당되는 StageSetting에서 사용될 EnemySpawn 값을 리턴합니다.
        public bool TryGetEnemySpawnData_Stage(int stageID, out EnemySpawnData_Stage enemySpawnData_Stage)
        {
            // SO를 못찾을 경우 false 리턴.
            if (!this.TryLoadScriptableObject_Stage(out var stageEnemySpawnData_StageGroup))
            {
                Debug.LogError($"[EnemySpawnDataDBHandler] SO 위치 못 찾음.");
                enemySpawnData_Stage = null;
                return false;
            }

            // SO 안에 StageID에 해당되는 데이터가 없을 경우 false 리턴.
            if (!stageEnemySpawnData_StageGroup.TryGetStageEnemySpawnData_Stage(stageID, out var stageEnemySpawnData_Stage))
            {
                Debug.LogError($"[EnemySpawnDataDBHandler] SO 안에 StageID 해당 데이터가 없음. StageID : {stageID}");
                enemySpawnData_Stage = null;
                return false;
            }

            // SO 안에 StageID에 해당되는 데이터는 넣었는데, Json 파일을 누락했을 경우.
            if (stageEnemySpawnData_Stage.EnemySpawnData_StageJsonFile == null)
            {
                Debug.LogError($"[EnemySpawnDataDBHandler] JsonFile이 비어 있음. StageID : {stageID}");
                enemySpawnData_Stage = null;
                return false;
            }

            // StageID에 해당되는 'Stage Enemy 생성 JsonFile'을 가져와서 Parsing 작업.
            var enemySpawnData_Stage_Parsed = JsonUtility.FromJson<EnemySpawnData_Stage>(stageEnemySpawnData_Stage.EnemySpawnData_StageJsonFile.text);
            if (enemySpawnData_Stage_Parsed == null || enemySpawnData_Stage_Parsed.EnemySpawnDatas == null)
            {
                Debug.LogError($"[EnemySpawnDataDBHandler] Json 파싱 실패. StageID: {stageID}");
                enemySpawnData_Stage = null;
                return false;
            }

            enemySpawnData_Stage = enemySpawnData_Stage_Parsed;
            return true;
        }
        // StageID에 해당되는 TurnStartSetting에서 사용될 EnemySpawn 값을 리턴합니다.
        public bool TryGetEnemySpawnData_Turn(int stageID, out EnemySpawnData_Turn[] enemySpawnData_Turns)
        {
            // SO를 못찾을 경우 false 리턴.
            if (!this.TryLoadScriptableObject_Turn(out var stageEnemySpawnData_TurnGroup))
            {
                Debug.LogError($"[EnemySpawnDataDBHandler] SO 위치 못 찾음.");
                enemySpawnData_Turns = null;
                return false;
            }

            // SO 안에 StageID에 해당되는 데이터가 없을 경우 false 리턴.
            if (!stageEnemySpawnData_TurnGroup.TryGetStageEnemySpawnData_Turn(stageID, out var stageEnemySpawnData_Turn))
            {
                Debug.LogError($"[EnemySpawnDataDBHandler] SO 안에 StageID 해당 데이터가 없음. StageID : {stageID}");
                enemySpawnData_Turns = null;
                return false;
            }

            // SO 안에 StageID에 해당되는 데이터는 넣었는데, Json 파일을 누락했을 경우.
            if (stageEnemySpawnData_Turn.EnemySpawnData_TurnJsonFile == null)
            {
                Debug.LogError($"[EnemySpawnDataDBHandler] JsonFile이 비어 있음. StageID : {stageID}");
                enemySpawnData_Turns = null;
                return false;
            }

            // StageID에 해당되는 'Turn Enemy 생성 JsonFile'을 가져와서 Parsing 작업.
            var enemySpawnData_TurnArrayWrapper = JsonUtility.FromJson<EnemySpawnData_TurnArrayWrapper>(stageEnemySpawnData_Turn.EnemySpawnData_TurnJsonFile.text);
            if (enemySpawnData_TurnArrayWrapper == null || enemySpawnData_TurnArrayWrapper.EnemySpawnData_Turns == null)
            {
                Debug.LogError($"[EnemySpawnDataDBHandler] Json 파싱 실패. StageID: {stageID}");
                enemySpawnData_Turns = null;
                return false;
            }

            enemySpawnData_Turns = enemySpawnData_TurnArrayWrapper.EnemySpawnData_Turns;
            return true;
        }
        // StageID에 해당되는 TurnStartSetting에서 사용될 EnemySpawn 값을 리턴합니다.
        public bool TryGetEnemySpawnData_Trigger(int stageID, out EnemySpawnData_Trigger[] EnemySpawnData_Triggers)
        {
            // SO를 못찾을 경우 false 리턴.
            if (!this.TryLoadScriptableObject_Trigger(out var stageEnemySpawnData_TriggerGroup))
            {
                Debug.LogError($"[EnemySpawnDataDBHandler] SO 위치 못 찾음.");
                EnemySpawnData_Triggers = null;
                return false;
            }

            // SO 안에 StageID에 해당되는 데이터가 없을 경우 false 리턴.
            if (!stageEnemySpawnData_TriggerGroup.TryGetStageEnemySpawnData_Turn(stageID, out var stageEnemySpawnData_Trigger))
            {
                Debug.LogError($"[EnemySpawnDataDBHandler] SO 안에 StageID 해당 데이터가 없음. StageID : {stageID}");
                EnemySpawnData_Triggers = null;
                return false;
            }

            // SO 안에 StageID에 해당되는 데이터는 넣었는데, Json 파일을 누락했을 경우.
            if (stageEnemySpawnData_Trigger.EnemySpawnData_TriggerJsonFile == null)
            {
                Debug.LogError($"[EnemySpawnDataDBHandler] JsonFile이 비어 있음. StageID : {stageID}");
                EnemySpawnData_Triggers = null;
                return false;
            }

            // StageID에 해당되는 'Trigger Enemy 생성 JsonFile'을 가져와서 Parsing 작업.
            var enemySpawnData_TriggerArrayWrapper = JsonUtility.FromJson<EnemySpawnData_TriggerArrayWrapper>(stageEnemySpawnData_Trigger.EnemySpawnData_TriggerJsonFile.text);
            if (enemySpawnData_TriggerArrayWrapper == null || enemySpawnData_TriggerArrayWrapper.EnemySpawnData_Triggers == null)
            {
                Debug.LogError($"[EnemySpawnDataDBHandler] Json 파싱 실패. StageID: {stageID}");
                EnemySpawnData_Triggers = null;
                return false;
            }

            EnemySpawnData_Triggers = enemySpawnData_TriggerArrayWrapper.EnemySpawnData_Triggers;
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