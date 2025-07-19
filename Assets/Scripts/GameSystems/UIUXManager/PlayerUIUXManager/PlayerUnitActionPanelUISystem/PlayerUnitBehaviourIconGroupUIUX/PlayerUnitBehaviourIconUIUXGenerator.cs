using UnityEngine;

using Foundations.Architecture.ReferencesHandler;

using GameSystems.UnitSystem;

namespace GameSystems.UIUXSystem
{
    public class PlayerUnitBehaviourIconUIUXGenerator : MonoBehaviour
    {
        [SerializeField] private Transform PlayerUnitMoveIconUIUXParent;
        [SerializeField] private GameObject PlayerUnitMoveIconUIUXPrefab;

        [SerializeField] private Transform PlayerUnitSkillIconUIUXParent;
        [SerializeField] private GameObject PlayerUnitSkillIconUIUXPrefab;

        public void InitialSetting(int uniqueID, int unitID)
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            var UnitSkillLinkDataDBHandler = HandlerManager.GetStaticDataHandler<UnitSkillLinkDataDBHandler>();

            GameObject createMoveIconObject = Instantiate(PlayerUnitMoveIconUIUXPrefab, PlayerUnitMoveIconUIUXParent);
            createMoveIconObject.GetComponent<PlayerUnitMoveIconUIUXMediator>().InitialSetting(uniqueID);

            if (!UnitSkillLinkDataDBHandler.TryGetUnitSkillLinkData(unitID, out var data)) return;


            foreach (int skillID in data.SkillIDs)
            {
                Debug.Log($"uniqueID : {uniqueID}, unitID : {unitID}, skillID : {skillID}");
                GameObject createSkillIconObject = Instantiate(PlayerUnitSkillIconUIUXPrefab, PlayerUnitSkillIconUIUXParent);
                createSkillIconObject.GetComponent<PlayerUnitSkillIconUIUXMediator>().InitialSetting(uniqueID, skillID);
            }
        }
    }
}