using UnityEngine;

namespace GameSystems.EnemySystem.EnemyUnitSystem
{
    public interface IEnemyUnitSpriteRendererController
    {
        public void InitialSetting(EnemyUnitManagerData enemyUnitManagerData);

        public void UpdateFlipX(Vector2Int targetedPosition);
        public void OperateHit();

        public void ShowRequestFromPlayerView();
        public void HideRequestFromPlayerView();
        public void OperateShowAndHide();
    }
}