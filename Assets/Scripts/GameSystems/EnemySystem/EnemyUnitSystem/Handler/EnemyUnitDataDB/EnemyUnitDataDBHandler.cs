using UnityEngine;

using Foundations.Architecture.ReferencesHandler;

namespace GameSystems.EnemySystem.EnemyUnitSystem
{
    public class EnemyUnitDataDBHandler : IStaticReferenceHandler
    {
        private string EnemyPrefabResourceDataGroup_SOPath = "ScriptableObject/Enemy/EnemyPrefabResourceDataGroup";

        // EnemyPrefabResource SO 로드 시도.
        private bool TryLoadScriptableObject(out EnemyPrefabResourceDataGroup enemyPrefabResourceDataGroup)
        {
            enemyPrefabResourceDataGroup = Resources.Load<EnemyPrefabResourceDataGroup>(this.EnemyPrefabResourceDataGroup_SOPath);

            // 파일 못찾으면 false 리턴.
            if (enemyPrefabResourceDataGroup == null) return false;
            return true;
        }

        public bool TryGetEnemyPrefabResourceData(int unitID, out EnemyPrefabResourceData enemyPrefabResourceData)
        {
            // SO를 못찾을 경우 false 리턴.
            if (!this.TryLoadScriptableObject(out var enemyPrefabResourceDataGroup))
            {
                Debug.LogError($"[EnemyUnitDataDBHandler] SO 위치 못 찾음.");
                enemyPrefabResourceData = null;
                return false;
            }

            return enemyPrefabResourceDataGroup.TryGetEnemyPrefabResourceData(unitID, out enemyPrefabResourceData);
        }
    }
}