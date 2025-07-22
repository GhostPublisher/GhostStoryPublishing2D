using System;
using System.Collections.Generic;

using UnityEngine;

using Foundations.Architecture.ReferencesHandler;

using GameSystems.PlayerSystem.PlayerUnitSystem;

namespace GameSystems.PlayerSystem
{
    public class PlayerUnitManagerDataDBHandler : IDynamicReferenceHandler
    {
        private HashSet<PlayerUnitManagerData> PlayerUnitManagerDatas = new();

        public void AddPlayerUnitManagerData(PlayerUnitManagerData unit)
        {
            this.PlayerUnitManagerDatas.Add(unit);
        }
        public void RemovePlayerUnitManagerData(PlayerUnitManagerData unit)
        {
            this.PlayerUnitManagerDatas.Remove(unit);
        }
        public bool TryGetPlayerUnitManagerData(int uniqueID, out PlayerUnitManagerData playerUnitManagerData)
        {
            foreach (PlayerUnitManagerData data in this.PlayerUnitManagerDatas)
            {
                if (data.UniqueID == uniqueID)
                {
                    playerUnitManagerData = data;
                    return true;
                }
            }

            playerUnitManagerData = null;
            return false;
        }
        public bool TryGetPlayerUnitManagerData(Vector2Int pos, out PlayerUnitManagerData playerUnitManagerData)
        {
            foreach (PlayerUnitManagerData data in this.PlayerUnitManagerDatas)
            {
                if (data.PlayerUnitGridPosition() == pos)
                {
                    playerUnitManagerData = data;
                    return true;
                }
            }

            playerUnitManagerData = null;
            return false;
        }
        public bool TryGetAll(out HashSet<PlayerUnitManagerData> datas)
        {
            datas = this.PlayerUnitManagerDatas;
            return datas.Count > 0;
        }

        public void ClearPlayerUnitManagerDataGroup()
        {
            this.PlayerUnitManagerDatas.Clear();
        }
    }

    public class PlayerUnitManagerData
    {
        public int UniqueID;

        public PlayerUnitManagerData(int uniqueID, PlayerUnitStaticData playerUnitStaticData, PlayerUnitDynamicData playerUnitDynamicData, PlayerUnitFeatureInterfaceGroup playerUnitFeatureInterfaceGroup, Transform playerUnitTransform)
        {
            this.UniqueID = uniqueID;
            this.PlayerUnitStaticData = playerUnitStaticData;
            this.PlayerUnitDynamicData = playerUnitDynamicData;
            this.PlayerUnitFeatureInterfaceGroup = playerUnitFeatureInterfaceGroup;
            this.PlayerUnitTransform = playerUnitTransform;

            this.PlayerUnitStaticData.UniqueID = this.UniqueID;
            this.PlayerUnitDynamicData.UniqueID = this.UniqueID;
            this.PlayerUnitFeatureInterfaceGroup.UniqueID = this.UniqueID;

            this.PlayerUnitDynamicData.CurrentHPCost = this.PlayerUnitStaticData.DefaultHPCost;
            this.PlayerUnitDynamicData.CurrentMoveCost = this.PlayerUnitStaticData.DefaultMoveCost;
            this.PlayerUnitDynamicData.CurrentSkillCost = this.PlayerUnitStaticData.DefaultSkillCost;
        }

        public PlayerUnitStaticData PlayerUnitStaticData { get; }
        public PlayerUnitDynamicData PlayerUnitDynamicData { get; }
        public PlayerUnitFeatureInterfaceGroup PlayerUnitFeatureInterfaceGroup { get; }
        public Transform PlayerUnitTransform { get; }

        public Vector2Int PlayerUnitGridPosition()
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance.GetUtilityHandler <UtilitySystem.IsometricCoordinateConvertor>();
            return HandlerManager.ConvertWorldToGrid(this.PlayerUnitTransform.position);
        }
    }
}