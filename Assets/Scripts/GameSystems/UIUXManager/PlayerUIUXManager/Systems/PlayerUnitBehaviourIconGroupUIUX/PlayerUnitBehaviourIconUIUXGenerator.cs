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

        public void InitialSetting(PlayerUnitActionUIUXHandler playerUnitActionUIUXHandler, int uniqueID, int unitID)
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            var UnitSkillLinkDataDBHandler = HandlerManager.GetStaticDataHandler<UnitSkillLinkDataDBHandler>();

            GameObject createMoveIconObject = Instantiate(PlayerUnitMoveIconUIUXPrefab, PlayerUnitMoveIconUIUXParent);
            createMoveIconObject.GetComponent<PlayerUnitMoveIconUIUXMediator>().InitialSetting(playerUnitActionUIUXHandler, uniqueID);

            // UnitDB과 SkillDB를 연결하는 LinkTable에서 값을 가져옴.
            if (!UnitSkillLinkDataDBHandler.TryGetUnitSkillLinkData(unitID, out var unitSkillLinkData)) return;


            foreach (int skillID in unitSkillLinkData.SkillIDs)
            {
                GameObject createSkillIconObject = Instantiate(PlayerUnitSkillIconUIUXPrefab, PlayerUnitSkillIconUIUXParent);
                createSkillIconObject.GetComponent<PlayerUnitSkillIconUIUXMediator>().InitialSetting(playerUnitActionUIUXHandler, uniqueID, skillID);
            }
        }
    }
}