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
            this.myPlayerUnitDynamicData.HPCost_Current = Mathf.Max(0, this.myPlayerUnitDynamicData.HPCost_Current - rawDamage);
        }

        public bool IsDead => this.myPlayerUnitDynamicData.HPCost_Current <= 0;
    }
}
