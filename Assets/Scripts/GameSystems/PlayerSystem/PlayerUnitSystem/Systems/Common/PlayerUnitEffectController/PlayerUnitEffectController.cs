
using System.Collections;
using UnityEngine;

namespace GameSystems.PlayerSystem.PlayerUnitSystem
{

    public class PlayerUnitEffectController : MonoBehaviour, IPlayerUnitEffectController
    {
        private PlayerUnitManagerData myPlayerUnitManagerData;

        public void InitialSetting(PlayerUnitManagerData playerUnitManagerData)
        {
            this.myPlayerUnitManagerData = playerUnitManagerData;
        }

        public IEnumerator OperateEffect(int effectID)
        {
            throw new System.NotImplementedException();
        }
    }
}
