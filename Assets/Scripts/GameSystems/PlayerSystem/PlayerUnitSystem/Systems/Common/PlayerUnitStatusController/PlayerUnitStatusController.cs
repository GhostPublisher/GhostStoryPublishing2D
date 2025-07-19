using UnityEngine;

namespace GameSystems.PlayerSystem.PlayerUnitSystem
{

    public class PlayerUnitStatusController : MonoBehaviour, IPlayerUnitStatusController
    {
        private PlayerUnitManagerData myPlayerUnitManagerData;

        private PlayerUnitDynamicData myPlayerUnitDynamicData;

        public void InitialSetting(PlayerUnitManagerData playerUnitManagerData)
        {
            this.myPlayerUnitManagerData = playerUnitManagerData;
            this.myPlayerUnitDynamicData = this.myPlayerUnitManagerData.PlayerUnitDynamicData;
        }

        public void ApplyDamage(int rawDamage)
        {
            this.myPlayerUnitDynamicData.CurrentHPCost = Mathf.Max(0, this.myPlayerUnitDynamicData.CurrentHPCost - rawDamage);
        }

        public bool IsDead => this.myPlayerUnitDynamicData.CurrentHPCost <= 0;
    }
}
