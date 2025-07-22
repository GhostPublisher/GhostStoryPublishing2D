using System;
using System.Collections.Generic;

using UnityEngine;

using Foundations.Architecture.ReferencesHandler;

namespace GameSystems.EnemySystem.EnemyUnitSystem
{
    public class EnemyUnitManagerDataDBHandler : IDynamicReferenceHandler
    {
        private HashSet<EnemyUnitManagerData> GeneratedEnemyUnitDatas = new();

        public void AddEnemyUnitManagerData(EnemyUnitManagerData unit)
        {
            this.GeneratedEnemyUnitDatas.Add(unit);
        }
        public void RemoveEnemyUnitManagerData(EnemyUnitManagerData unit)
        {
            this.GeneratedEnemyUnitDatas.Remove(unit);
        }
        public bool TryGetEnemyUnitManagerData(int uniqueID, out EnemyUnitManagerData enemyUnitManagerData)
        {
            foreach (EnemyUnitManagerData data in this.GeneratedEnemyUnitDatas)
            {
                if (data.UniqueID == uniqueID)
                {
                    enemyUnitManagerData = data;
                    return true;
                }
            }

            enemyUnitManagerData = null;
            return false;
        }
        public bool TryGetEnemyUnitManagerData(Vector2Int pos, out EnemyUnitManagerData enemyUnitManagerData)
        {
            foreach (EnemyUnitManagerData data in this.GeneratedEnemyUnitDatas)
            {
                if(data.EnemyUnitGridPosition() == pos)
                {
                    enemyUnitManagerData = data;
                    return true;
                }
            }

            enemyUnitManagerData = null;
            return false;
        }
        public bool TryGetAll(out HashSet<EnemyUnitManagerData> enemyUnitManagerDatas)
        {
            enemyUnitManagerDatas = this.GeneratedEnemyUnitDatas;
            return enemyUnitManagerDatas.Count > 0;
        }

        public void ClearEnemyUnitManagerDataGroup()
        {
            this.GeneratedEnemyUnitDatas.Clear();
        }
    }

    [Serializable]
    public class EnemyUnitManagerData
    {
        public EnemyUnitManagerData(int uniqueID, EnemyUnitStaticData enemyUnitStaticData, EnemyUnitDynamicData enemyUnitDynamicData, EnemyUnitFeatureInterfaceGroup enemyUnitFeatureInterfaceGroup, Transform enemyUnitTransform)
        {
            this.UniqueID = uniqueID;
            this.EnemyUnitStaticData = enemyUnitStaticData;
            this.EnemyUnitDynamicData = enemyUnitDynamicData;
            this.EnemyUnitFeatureInterfaceGroup = enemyUnitFeatureInterfaceGroup;
            this.EnemyUnitTransform = enemyUnitTransform;

            this.EnemyUnitStaticData.UniqueID = this.UniqueID;
            this.EnemyUnitDynamicData.UniqueID = this.UniqueID;

            this.EnemyUnitDynamicData.CurrentHPCost = this.EnemyUnitStaticData.DefaultHPCost;
            this.EnemyUnitDynamicData.CurrentMoveCost = this.EnemyUnitStaticData.DefaultMoveCost;
            this.EnemyUnitDynamicData.CurrentSkillCost = this.EnemyUnitStaticData.DefaultSkillCost;
        }

        public int UniqueID { get; }
        public EnemyUnitStaticData EnemyUnitStaticData { get; }
        public EnemyUnitDynamicData EnemyUnitDynamicData { get; }
        public EnemyUnitFeatureInterfaceGroup EnemyUnitFeatureInterfaceGroup { get; }
        public Transform EnemyUnitTransform { get; }
        public Vector2Int EnemyUnitGridPosition()
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance.GetUtilityHandler<UtilitySystem.IsometricCoordinateConvertor>();
            return HandlerManager.ConvertWorldToGrid(this.EnemyUnitTransform.position);
        }
    }
}