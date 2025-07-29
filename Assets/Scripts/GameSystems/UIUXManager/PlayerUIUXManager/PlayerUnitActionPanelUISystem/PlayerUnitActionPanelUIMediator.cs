using System.Collections.Generic;
using UnityEngine;

namespace GameSystems.UIUXSystem
{
    public class PlayerUnitActionPanelUIMediator : MonoBehaviour
    {
        [SerializeField] private PlayerUnitIconGroupUIUXGenerator PlayerUnitIconGroupUIUXGenerator;
        [SerializeField] private PlayerUnitIconGroupUIUXViewController PlayerUnitIconGroupUIUXViewController;
        [SerializeField] private PlayerUnitActionPanelMouseInteractor PlayerUnitActionPanelMouseInteractor;

        private PlayerUnitIconGroupUIUXDataGroup myPlayerUnitIconGroupUIUXDataGroup;

        private void Awake()
        {
            this.myPlayerUnitIconGroupUIUXDataGroup = new();

            this.PlayerUnitIconGroupUIUXGenerator.InitialSetting(this.myPlayerUnitIconGroupUIUXDataGroup);
            this.PlayerUnitIconGroupUIUXViewController.InitialSetting(this.myPlayerUnitIconGroupUIUXDataGroup);
            this.PlayerUnitActionPanelMouseInteractor.InitialSetting(this.HidePlayerUnitActionPanel_Half_NotTilemapOperation);
        }

        // 생성된 Player Units에 해당되는 PlayerUnitIcon 갱신
        public void UpdateGeneratedPlayerUnitIconUIUX()
        {
//            Debug.Log($"asaA");
            this.PlayerUnitIconGroupUIUXGenerator.UpdateGeneratedPlayerUnitIconUIUX();
        }
        public void ClearGeneratedPlayerUnitIconUIUX()
        {
            this.PlayerUnitIconGroupUIUXGenerator.ClearPlayerUnitIconGroupUIUXAndData();
        }

        public void ShowPlayerUnitActionPanel_All(int playerUniqueID)
        {
            if (this.myPlayerUnitIconGroupUIUXDataGroup.IsTilemapOperated) return;

            this.PlayerUnitIconGroupUIUXViewController.TogglePlayerBehaviourUIUX(playerUniqueID);
            this.PlayerUnitIconGroupUIUXViewController.ShowUIAnimationData_All();
            this.PlayerUnitActionPanelMouseInteractor.enabled = true;
        }
        public void ShowPlayerUnitActionPanel_Half()
        {
            this.myPlayerUnitIconGroupUIUXDataGroup.IsTilemapOperated = false;
            this.PlayerUnitIconGroupUIUXViewController.ShowUIAnimationData_Half();
            this.PlayerUnitActionPanelMouseInteractor.enabled = false;
        }
        public void HidePlayerUnitActionPanel_All()
        {
            this.PlayerUnitIconGroupUIUXViewController.HideUIAnimationData_All();
        }
        public void HidePlayerUnitActionPanel_Half()
        {
            this.myPlayerUnitIconGroupUIUXDataGroup.IsTilemapOperated = true;
            this.PlayerUnitIconGroupUIUXViewController.HideUIAnimationData_Half();
            this.PlayerUnitActionPanelMouseInteractor.enabled = false;
        }
        public void HidePlayerUnitActionPanel_Half_NotTilemapOperation()
        {
            this.PlayerUnitIconGroupUIUXViewController.HideUIAnimationData_Half();
            this.PlayerUnitActionPanelMouseInteractor.enabled = false;
        }


        public void IsOverTilemapOperation()
        {
            this.myPlayerUnitIconGroupUIUXDataGroup.IsTilemapOperated = false;
        }
    }

    public class PlayerUnitIconGroupUIUXDataGroup
    {
        public Dictionary<int, PlayerUnitIconGroupUIUXData> PlayerUnitIconGroupUIUXDatas;

        public bool IsTilemapOperated = false;

        public PlayerUnitIconGroupUIUXDataGroup()
        {
            this.PlayerUnitIconGroupUIUXDatas = new();
        }

        public bool TryGetPlayerUnitIconGroupUIUXData(int uniqueID, out PlayerUnitIconGroupUIUXData playerUnitIconGroupUIUXData)
        {
            playerUnitIconGroupUIUXData = null;
            if (this.PlayerUnitIconGroupUIUXDatas == null) return false;

            return this.PlayerUnitIconGroupUIUXDatas.TryGetValue(uniqueID, out playerUnitIconGroupUIUXData);
        }
    }

    public class PlayerUnitIconGroupUIUXData
    {
        public int UniqueID;
        public IPlayerUnitIconUIUXViewController IPlayerUnitIconUIUXViewController;

        public GameObject PlayerUnitBehaviourIconGroupUIUXGameObject;
    }
}