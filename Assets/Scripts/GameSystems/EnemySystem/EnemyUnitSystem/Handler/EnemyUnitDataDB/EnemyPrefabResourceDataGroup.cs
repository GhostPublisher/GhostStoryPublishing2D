using System;
using System.Collections.Generic;

using UnityEngine;

namespace GameSystems.EnemySystem.EnemyUnitSystem
{
    [Serializable]
    [CreateAssetMenu(menuName = "ScriptableObject/Enemy/EnemyPrefabResourceDataGroup", fileName = "EnemyPrefabResourceDataGroup")]
    public class EnemyPrefabResourceDataGroup : ScriptableObject
    {
        [SerializeField] private List<EnemyPrefabResourceData> EnemyPrefabResourceDatas;

        public bool TryGetEnemyPrefabResourceData(int unitID, out EnemyPrefabResourceData enemyPrefabResourceData)
        {
            if (this.EnemyPrefabResourceDatas == null)
            {
                enemyPrefabResourceData = null;
                return false;
            }

            foreach (var data in this.EnemyPrefabResourceDatas)
            {
                if(data.UnitID == unitID)
                {
                    enemyPrefabResourceData = data;
                    return true;
                }
            }

            enemyPrefabResourceData = null;
            return false;
        }
    }

    [Serializable]
    public class EnemyPrefabResourceData
    {
        public int UnitID;
        public GameObject EnemyPrefab;
    }
}