
using UnityEngine;

namespace GameSystems.PlayerSystem.PlayerUnitSystem
{
    public interface IPlayerUnitSpriteRendererController
    {
        public void InitialSetting(PlayerUnitManagerData playerUnitManagerData);

        public void UpdateFlipX(Vector2Int targetedPosition);
        public void OperateHit();

        public void OnPointerEnter();
        public void OnPointerExit();
        public void OnPointerDown();
        public void OnPointerUp();
    }
}