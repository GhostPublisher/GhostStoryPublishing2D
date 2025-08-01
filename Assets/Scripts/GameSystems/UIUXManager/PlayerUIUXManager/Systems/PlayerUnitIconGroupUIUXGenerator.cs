using UnityEngine;

using Foundations.Architecture.ReferencesHandler;
using GameSystems.PlayerSystem;

namespace GameSystems.UIUXSystem
{
    public class PlayerUnitIconGroupUIUXGenerator : MonoBehaviour
    {
        [SerializeField] private Transform PlayerUnitIconUIUXParent;
        [SerializeField] private GameObject PlayerUnitIconUIUXPrefab;

        [SerializeField] private Transform PlayerUnitBehaviourIconUIUXParent;
        [SerializeField] private GameObject PlayerUnitBehaviourIconUIUXPrefab;

        private PlayerUnitActionUIUXHandler myPlayerUnitActionUIUXHandler;

        public void InitialSetting(PlayerUnitActionUIUXHandler playerUnitActionUIUXHandler)
        {
            this.myPlayerUnitActionUIUXHandler = playerUnitActionUIUXHandler;
        }

        public void UpdateGeneratedPlayerUnitIconUIUX()
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            var PlayerUnitManagerDataDBHandler = HandlerManager.GetDynamicDataHandler<PlayerUnitManagerDataDBHandler>();

            // 생성된 PlayerUnit이 없으면 리턴.
            if (!PlayerUnitManagerDataDBHandler.TryGetAll(out var playerUnits)) return;

            foreach(var unitData in playerUnits)
            {
                // 이미 존재하는 Unit이면 넘어감.
                if (this.myPlayerUnitActionUIUXHandler.TryGetPlayerUnitIconGroupUIUXData(unitData.UniqueID, out var _)) continue;

                this.GeneratePlayerUnitIconUIUX(unitData.UniqueID, unitData.PlayerUnitStaticData.UnitID);
            }
        }

        public void GeneratePlayerUnitIconUIUX(int uniqueID, int unitID)
        {
            GameObject unitImageIcon = Instantiate(this.PlayerUnitIconUIUXPrefab, this.PlayerUnitIconUIUXParent);
            GameObject unitBehaviourIcon = Instantiate(this.PlayerUnitBehaviourIconUIUXPrefab, this.PlayerUnitBehaviourIconUIUXParent);

            PlayerUnitIconGroupUIUXData playerUnitIconGroupUIUXData = new(uniqueID, unitID, unitBehaviourIcon);
            this.myPlayerUnitActionUIUXHandler.PlayerUnitIconGroupUIUXDatas.Add(playerUnitIconGroupUIUXData.UniqueID, playerUnitIconGroupUIUXData);

            unitImageIcon.GetComponent<PlayerUnitIconUIUXMediator>().InitialSetting(this.myPlayerUnitActionUIUXHandler, uniqueID, unitID);
            unitBehaviourIcon.GetComponent<PlayerUnitBehaviourIconUIUXGenerator>().InitialSetting(this.myPlayerUnitActionUIUXHandler, uniqueID, unitID);

            playerUnitIconGroupUIUXData.PlayerUnitBehaviourIconGroupUIUXGameObject = unitBehaviourIcon;
        }

        public void ClearPlayerUnitIconGroupUIUXAndData()
        {
            this.myPlayerUnitActionUIUXHandler.PlayerUnitIconGroupUIUXDatas.Clear();

            foreach (Transform child in PlayerUnitIconUIUXParent)
            {
                GameObject.Destroy(child.gameObject);
            }

            foreach (Transform child in PlayerUnitBehaviourIconUIUXParent)
            {
                GameObject.Destroy(child.gameObject);
            }
        }
    }
}