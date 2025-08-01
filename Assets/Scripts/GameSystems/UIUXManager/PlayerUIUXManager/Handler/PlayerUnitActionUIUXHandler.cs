using System;
using System.Collections.Generic;

using UnityEngine;


namespace GameSystems.UIUXSystem
{
    public class PlayerUnitActionUIUXHandler
    {
        // 인터페이스
        public IPlayerUnitActionPanelUIMediator IPlayerUnitActionPanelUIMediator { get; set; }
        public IPlayerUnitIconGroupUIUXViewController IPlayerUnitIconGroupUIUXViewController { get; set; }
        public IPlayerUnitActionPanelMouseInteractor IPlayerUnitActionPanelMouseInteractor { get; set; }
        // 
        public Dictionary<int, PlayerUnitIconGroupUIUXData> PlayerUnitIconGroupUIUXDatas = new();

        public bool IsInteractived { get; set; }

        public PlayerUnitActionUIUXHandler()
        {
            this.IsInteractived = false;
        }

        public bool TryGetPlayerUnitIconGroupUIUXData(int uniqueID, out PlayerUnitIconGroupUIUXData playerUnitIconGroupUIUXData)
        {
            playerUnitIconGroupUIUXData = null;
            if (this.PlayerUnitIconGroupUIUXDatas == null) return false;

            return this.PlayerUnitIconGroupUIUXDatas.TryGetValue(uniqueID, out playerUnitIconGroupUIUXData);
        }
    }

    [Serializable]
    public class PlayerUnitIconGroupUIUXData
    {
        public Dictionary<int, IPlayerUnitSkillIconUIUXViewController> IPlayerUnitSkillIconUIUXViewControllers = new();

        public PlayerUnitIconGroupUIUXData(int uniqueID, int unitID, GameObject behaviourIconGroup)
        {
            this.UniqueID = uniqueID;
            this.UnitID = unitID;
            this.PlayerUnitBehaviourIconGroupUIUXGameObject = behaviourIconGroup;
        }

        public int UniqueID { get; set; }
        public int UnitID { get; set; }
        public GameObject PlayerUnitBehaviourIconGroupUIUXGameObject { get; set; }

        public IPlayerUnitIconUIUXViewController IPlayerUnitIconUIUXViewController { get; set; }

        public IPlayerUnitMoveIconUIUXViewController IPlayerUnitMoveIconUIUXViewController { get; set; }
        public bool TryGetIPlayerUnitSkillIconUIUXViewController(int skillID, out IPlayerUnitSkillIconUIUXViewController playerUnitSkillIconUIUXViewController)
        {
            return this.IPlayerUnitSkillIconUIUXViewControllers.TryGetValue(skillID, out playerUnitSkillIconUIUXViewController);
        }
    }
}