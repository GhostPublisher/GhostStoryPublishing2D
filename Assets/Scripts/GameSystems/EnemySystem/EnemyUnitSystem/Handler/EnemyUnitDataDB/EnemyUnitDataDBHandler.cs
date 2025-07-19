using UnityEngine;

using Foundations.Architecture.ReferencesHandler;

namespace GameSystems.EnemySystem.EnemyUnitSystem
{
    public class EnemyUnitDataDBHandler : IStaticReferenceHandler
    {
        private EnemyPrefabResourceDataGroup EnemyPrefabResourceDataGroup;

        public EnemyUnitDataDBHandler()
        {
            this.LoadScriptableObject();
        }
        private void LoadScriptableObject()
        {
            // EnemyPrefabResourceDataGroup
            this.EnemyPrefabResourceDataGroup = Resources.Load<EnemyPrefabResourceDataGroup>("ScriptableObject/Enemy/EnemyPrefabResourceDataGroup");

            if (this.EnemyPrefabResourceDataGroup == null)
            {
                Debug.LogError("[EnemyPrefabResourceDataGroup] EnemyPrefabResourceDataGroup.asset을 찾을 수 없습니다. 경로 또는 파일 확인 필요.");
            }
        }

        public EnemyPrefabResourceData GetEnemyPrefabResourceData(int unitID)
        {
            return this.EnemyPrefabResourceDataGroup.GetEnemyPrefabResourceData(unitID);
        }
    }
}