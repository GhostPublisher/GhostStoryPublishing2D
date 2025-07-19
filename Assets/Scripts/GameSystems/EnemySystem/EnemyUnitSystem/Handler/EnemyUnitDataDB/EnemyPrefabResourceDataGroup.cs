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

        public EnemyPrefabResourceData GetEnemyPrefabResourceData(int UnitID)
        {
            return this.EnemyPrefabResourceDatas.Find(data => data.UnitID == UnitID);
        }
    }

    [Serializable]
    public class EnemyPrefabResourceData
    {
        public int UnitID;
        public GameObject EnemyPrefab;
    }
}