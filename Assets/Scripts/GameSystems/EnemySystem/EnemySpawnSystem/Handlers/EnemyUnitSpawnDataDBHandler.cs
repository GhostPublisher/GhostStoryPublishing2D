using System;

using UnityEngine;

using Foundations.Architecture.ReferencesHandler;

namespace GameSystems.EnemySystem.EnemySpawnSystem
{
    [Serializable]
    public class EnemyUnitSpawnDataDBHandler : IStaticReferenceHandler
    {
        private string EnemyUnitSpawnDataGroupDB_Stage_SOPath = "ScriptableObject/EnemySpawn/EnemyUnitSpawnDataGroupDB_Stage";
        private string EnemyUnitSpawnDataGroupDB_Turn_SOPath = "ScriptableObject/EnemySpawn/EnemyUnitSpawnDataGroupDB_Turn";
        private string EnemyUnitSpawnDataGroupDB_Trigger_SOPath = "ScriptableObject/EnemySpawn/EnemyUnitSpawnDataGroupDB_Trigger";

        // Stage Enemy Spawn DB SO 로드 시도.
        private bool TryLoadScriptableObject_Stage(out EnemyUnitSpawnDataGroupDB_Stage enemyUnitSpawnDataGroupDB_Stage)
        {
            enemyUnitSpawnDataGroupDB_Stage = Resources.Load<EnemyUnitSpawnDataGroupDB_Stage>(this.EnemyUnitSpawnDataGroupDB_Stage_SOPath);

            // 파일 못찾으면 false 리턴.
            if (enemyUnitSpawnDataGroupDB_Stage == null) return false;
            return true;
        }
        // Turn Enemy Spawn DB SO 로드 시도.
        private bool TryLoadScriptableObject_Turn(out EnemyUnitSpawnDataGroupDB_Turn enemyUnitSpawnDataGroupDB_Turn)
        {
            enemyUnitSpawnDataGroupDB_Turn = Resources.Load<EnemyUnitSpawnDataGroupDB_Turn>(this.EnemyUnitSpawnDataGroupDB_Turn_SOPath);

            // 파일 못찾으면 false 리턴.
            if (enemyUnitSpawnDataGroupDB_Turn == null) return false;
            return true;
        }
        // Trigger Enemy Spawn DB SO 로드 시도.
        private bool TryLoadScriptableObject_Trigger(out EnemyUnitSpawnDataGroupDB_Trigger enemyUnitSpawnDataGroupDB_Trigger)
        {
            enemyUnitSpawnDataGroupDB_Trigger = Resources.Load<EnemyUnitSpawnDataGroupDB_Trigger>(this.EnemyUnitSpawnDataGroupDB_Trigger_SOPath);

            // 파일 못찾으면 false 리턴.
            if (enemyUnitSpawnDataGroupDB_Trigger == null) return false;
            return true;
        }

        // StageID에 해당되는 StageSetting에서 사용될 EnemySpawn 값을 리턴합니다.
        public bool TryGetEnemySpawnData_Stage(int stageID, out EnemyUnitSpawnData_Stage enemyUnitSpawnData_Stage)
        {
            // SO를 못찾을 경우 false 리턴.
            if (!this.TryLoadScriptableObject_Stage(out var stageEnemySpawnData_StageGroup))
            {
                Debug.LogError($"[EnemyUnitSpawnDataDBHandler] SO 위치 못 찾음.");
                enemyUnitSpawnData_Stage = null;
                return false;
            }

            // SO 안에 StageID에 해당되는 데이터가 없을 경우 false 리턴.
            if (!stageEnemySpawnData_StageGroup.TryGetEnemyUnitSpawnDataGroup_Stage(stageID, out var enemyUnitSpawnDataGroup_Stage))
            {
                Debug.LogError($"[EnemyUnitSpawnDataDBHandler] SO 안에 StageID 해당 데이터가 없음. StageID : {stageID}");
                enemyUnitSpawnData_Stage = null;
                return false;
            }

            // SO 안에 StageID에 해당되는 데이터는 넣었는데, Json 파일을 누락했을 경우.
            if (enemyUnitSpawnDataGroup_Stage.EnemyUnitSpawnData_StageJsonFile == null)
            {
                Debug.LogError($"[EnemyUnitSpawnDataDBHandler] JsonFile이 비어 있음. StageID : {stageID}");
                enemyUnitSpawnData_Stage = null;
                return false;
            }

            // StageID에 해당되는 'Stage Enemy 생성 JsonFile'을 가져와서 Parsing 작업.
            var enemyUnitSpawnData_Stage_Parsed = JsonUtility.FromJson<EnemyUnitSpawnData_Stage>(enemyUnitSpawnDataGroup_Stage.EnemyUnitSpawnData_StageJsonFile.text);
            if (enemyUnitSpawnData_Stage_Parsed == null || enemyUnitSpawnData_Stage_Parsed.UnitSpawnDatas == null)
            {
                Debug.LogError($"[EnemyUnitSpawnDataDBHandler] Json 파싱 실패. StageID: {stageID}");
                enemyUnitSpawnData_Stage = null;
                return false;
            }

            enemyUnitSpawnData_Stage = enemyUnitSpawnData_Stage_Parsed;
            return true;
        }
        // StageID에 해당되는 TurnStartSetting에서 사용될 EnemySpawn 값을 리턴합니다.
        public bool TryGetEnemyUnitSpawnDataGroup_Turn(int stageID, out EnemyUnitSpawnData_Turn[] enemyUnitSpawnData_Turns)
        {
            // SO를 못찾을 경우 false 리턴.
            if (!this.TryLoadScriptableObject_Turn(out var enemyUnitSpawnDataGroupDB_Turn))
            {
                Debug.LogError($"[EnemySpawnDataDBHandler] SO 위치 못 찾음.");
                enemyUnitSpawnData_Turns = null;
                return false;
            }

            // SO 안에 StageID에 해당되는 데이터가 없을 경우 false 리턴.
            if (!enemyUnitSpawnDataGroupDB_Turn.TryGetEnemyUnitSpawnDataGroup_Turn(stageID, out var enemyUnitSpawnDataGroup_Turn))
            {
                Debug.LogWarning($"[EnemySpawnDataDBHandler] SO 안에 StageID 해당 데이터가 없음. StageID : {stageID}");
                enemyUnitSpawnData_Turns = null;
                return false;
            }

            // SO 안에 StageID에 해당되는 데이터는 넣었는데, Json 파일을 누락했을 경우.
            if (enemyUnitSpawnDataGroup_Turn.EnemyUnitSpawnData_TurnJsonFile == null)
            {
                Debug.LogError($"[EnemySpawnDataDBHandler] JsonFile이 비어 있음. StageID : {stageID}");
                enemyUnitSpawnData_Turns = null;
                return false;
            }

            // StageID에 해당되는 'Turn Enemy 생성 JsonFile'을 가져와서 Parsing 작업.
            var enemyUnitSpawnData_TurnArrayWrapper = JsonUtility.FromJson<EnemyUnitSpawnData_TurnArrayWrapper>(enemyUnitSpawnDataGroup_Turn.EnemyUnitSpawnData_TurnJsonFile.text);
            if (enemyUnitSpawnData_TurnArrayWrapper == null || enemyUnitSpawnData_TurnArrayWrapper.EnemyUnitSpawnData_Turns == null)
            {
                Debug.LogError($"[EnemySpawnDataDBHandler] Json 파싱 실패. StageID: {stageID}");
                enemyUnitSpawnData_Turns = null;
                return false;
            }

            enemyUnitSpawnData_Turns = enemyUnitSpawnData_TurnArrayWrapper.EnemyUnitSpawnData_Turns;
            return true;
        }
        // StageID에 해당되는 TurnStartSetting에서 사용될 EnemySpawn 값을 리턴합니다.
        public bool TryGetEnemyUnitSpawnDataGroup_Trigger(int stageID, out EnemyUnitSpawnData_Trigger[] enemyUnitSpawnData_Triggers)
        {
            // SO를 못찾을 경우 false 리턴.
            if (!this.TryLoadScriptableObject_Trigger(out var enemyUnitSpawnDataGroupDB_Trigger))
            {
                Debug.LogError($"[EnemySpawnDataDBHandler] SO 위치 못 찾음.");
                enemyUnitSpawnData_Triggers = null;
                return false;
            }

            // SO 안에 StageID에 해당되는 데이터가 없을 경우 false 리턴.
            if (!enemyUnitSpawnDataGroupDB_Trigger.TryGetEnemyUnitSpawnDataGroup_Trigger(stageID, out var enemyUnitSpawnDataGroup_Trigger))
            {
                Debug.LogWarning($"[EnemySpawnDataDBHandler] SO 안에 StageID 해당 데이터가 없음. StageID : {stageID}");
                enemyUnitSpawnData_Triggers = null;
                return false;
            }

            // SO 안에 StageID에 해당되는 데이터는 넣었는데, Json 파일을 누락했을 경우.
            if (enemyUnitSpawnDataGroup_Trigger.EnemyUnitSpawnData_TriggerJsonFile == null)
            {
                Debug.LogError($"[EnemySpawnDataDBHandler] JsonFile이 비어 있음. StageID : {stageID}");
                enemyUnitSpawnData_Triggers = null;
                return false;
            }

            // StageID에 해당되는 'Trigger Enemy 생성 JsonFile'을 가져와서 Parsing 작업.
            var enemyUnitSpawnData_TriggerArrayWrapper = JsonUtility.FromJson<EnemyUnitSpawnData_TriggerArrayWrapper>(enemyUnitSpawnDataGroup_Trigger.EnemyUnitSpawnData_TriggerJsonFile.text);
            if (enemyUnitSpawnData_TriggerArrayWrapper == null || enemyUnitSpawnData_TriggerArrayWrapper.EnemyUnitSpawnData_Triggers == null)
            {
                Debug.LogError($"[EnemySpawnDataDBHandler] Json 파싱 실패. StageID: {stageID}");
                enemyUnitSpawnData_Triggers = null;
                return false;
            }

            enemyUnitSpawnData_Triggers = enemyUnitSpawnData_TriggerArrayWrapper.EnemyUnitSpawnData_Triggers;
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