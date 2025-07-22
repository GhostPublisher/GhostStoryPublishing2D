using System.Collections;

using UnityEngine;

namespace GameSystems.PlayerSystem.PlayerUnitSystem
{

    public class PlayerUnitSpriteRendererController : MonoBehaviour, IPlayerUnitSpriteRendererController
    {
        [SerializeField] private SpriteRenderer SpriteRendererFrameObject;
        [SerializeField] private SpriteRenderer SpriteRendererMain;

        [SerializeField] private Color SpriteBaseColor;
        [SerializeField] private Color SpritePointerDownColor;

        [SerializeField] private Color SpriteHittedColor;

        private PlayerUnitManagerData myPlayerUnitManagerData;

        private void OnEnable()
        {
            this.SpriteRendererFrameObject.enabled = false;
        }

        public void InitialSetting(PlayerUnitManagerData playerUnitManagerData)
        {
            this.myPlayerUnitManagerData = playerUnitManagerData;
        }

        // 유닛 2002의 경우이다. 이 유닛의 sprite는 flipX = false일 때 왼쪽을 바로 보고 있다.
        // Sprite의 기본 방향을 통일하면 이곳은 그냥 두면 될듯.
        public void UpdateFlipX(Vector2Int targetedPosition)
        {
            Vector2Int direction = targetedPosition - this.myPlayerUnitManagerData.PlayerUnitGridPosition();

            if (direction.x == direction.y) return;

            if (direction.x > direction.y)
            {
                this.SpriteRendererFrameObject.flipX = true;
                this.SpriteRendererMain.flipX = true;
            }
            else if (direction.x < direction.y)
            {
                this.SpriteRendererFrameObject.flipX = false;
                this.SpriteRendererMain.flipX = false;
            }
        }

        public void OnPointerEnter()
        {
            this.SpriteRendererFrameObject.enabled = true;
        }
        public void OnPointerExit()
        {
            this.SpriteRendererFrameObject.enabled = false;
        }
        public void OnPointerDown()
        {
            this.SpriteRendererMain.color = this.SpritePointerDownColor;
        }
        public void OnPointerUp()
        {
            this.SpriteRendererMain.color = this.SpriteBaseColor;
        }

        public void OperateHit()
        {
            StopCoroutine(this.OperateHit_Cor());
            StartCoroutine(this.OperateHit_Cor());
        }
        private IEnumerator OperateHit_Cor()
        {
            this.SpriteRendererMain.color = this.SpriteHittedColor;

            yield return new WaitForSeconds(0.3f);

            this.SpriteRendererMain.color = this.SpriteBaseColor;
        }
    }
}