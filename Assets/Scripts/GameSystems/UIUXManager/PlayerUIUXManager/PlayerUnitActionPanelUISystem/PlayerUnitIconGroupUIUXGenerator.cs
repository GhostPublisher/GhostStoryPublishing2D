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

        private PlayerUnitIconGroupUIUXDataGroup myPlayerUnitIconGroupUIUXDataGroup;

        public void InitialSetting(PlayerUnitIconGroupUIUXDataGroup playerUnitIconGroupUIUXData)
        {
            this.myPlayerUnitIconGroupUIUXDataGroup = playerUnitIconGroupUIUXData;
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
                if (this.myPlayerUnitIconGroupUIUXDataGroup.TryGetPlayerUnitIconGroupUIUXData(unitData.UniqueID, out var _)) continue;

                this.GeneratePlayerUnitIconUIUX(unitData.UniqueID, unitData.PlayerUnitStaticData.UnitID);
            }
        }

        public void GeneratePlayerUnitIconUIUX(int uniqueID, int unitID)
        {
            PlayerUnitIconGroupUIUXData playerUnitIconGroupUIUXData = new();
            playerUnitIconGroupUIUXData.UniqueID = uniqueID;

            GameObject unitImageIcon = Instantiate(this.PlayerUnitIconUIUXPrefab, this.PlayerUnitIconUIUXParent);
            unitImageIcon.GetComponent<PlayerUnitIconUIUXMediator>().InitialSetting(playerUnitIconGroupUIUXData, unitID);

            GameObject unitBehaviourIcon = Instantiate(this.PlayerUnitBehaviourIconUIUXPrefab, this.PlayerUnitBehaviourIconUIUXParent);
            unitBehaviourIcon.GetComponent<PlayerUnitBehaviourIconUIUXGenerator>().InitialSetting(uniqueID, unitID);
            playerUnitIconGroupUIUXData.PlayerUnitBehaviourIconGroupUIUXGameObject = unitBehaviourIcon;

            this.myPlayerUnitIconGroupUIUXDataGroup.PlayerUnitIconGroupUIUXDatas.Add(uniqueID, playerUnitIconGroupUIUXData);
        }

        public void ClearPlayerUnitIconGroupUIUXAndData()
        {
            this.myPlayerUnitIconGroupUIUXDataGroup.PlayerUnitIconGroupUIUXDatas.Clear();

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