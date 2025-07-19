using UnityEngine;
using UnityEngine.UI;

using Foundations.Architecture.ReferencesHandler;

using GameSystems.PlayerSystem.PlayerUnitSystem;

namespace GameSystems.UIUXSystem
{
    public class PlayerUnitIconUIUXContentSetter : MonoBehaviour
    {
        private PlayerUnitDataDBHandler PlayerUnitDataDBHandler;

        [SerializeField] private Image UnitIconImageSlot;

        private int myUnitID;

        public void InitialSetting(int unitID)
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            this.PlayerUnitDataDBHandler = HandlerManager.GetStaticDataHandler<PlayerUnitDataDBHandler>();

            this.myUnitID = unitID;
        }

        public void SetBaseImage()
        {
            if (!this.PlayerUnitDataDBHandler.TryGetPlayerUnitResourceData(this.myUnitID, out var data)) return;

            this.UnitIconImageSlot.sprite = data.UnitSpriteImage;
        }

    }
}