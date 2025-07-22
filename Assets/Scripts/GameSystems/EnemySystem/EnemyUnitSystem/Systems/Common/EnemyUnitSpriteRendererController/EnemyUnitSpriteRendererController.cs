using System.Collections;
using UnityEngine;

namespace GameSystems.EnemySystem.EnemyUnitSystem
{

    public class EnemyUnitSpriteRendererController : MonoBehaviour, IEnemyUnitSpriteRendererController
    {
        [SerializeField] private SpriteRenderer SpriteRendererMain;

        [SerializeField] private Color SpriteBaseColor;
        [SerializeField] private Color SpriteHittedColor;

        [SerializeField] private float SpriteBaseAlpha;

        private bool isHideRequested = false;
        private bool isShowRequested = false;
        private bool isActivated = true;
        private float duration = 0.5f;


        private EnemyUnitManagerData myEnemyUnitManagerData;

        public void InitialSetting(EnemyUnitManagerData enemyUnitManagerData)
        {
            this.myEnemyUnitManagerData = enemyUnitManagerData;
        }

        public void UpdateFlipX(Vector2Int targetedPosition)
        {
            if (this.myEnemyUnitManagerData.EnemyUnitGridPosition().x == targetedPosition.x) return;

            if (this.myEnemyUnitManagerData.EnemyUnitGridPosition().x > targetedPosition.x && this.SpriteRendererMain.flipX)
            {
                this.SpriteRendererMain.flipX = false;
            }

            if (this.myEnemyUnitManagerData.EnemyUnitGridPosition().x < targetedPosition.x && !this.SpriteRendererMain.flipX)
            {
                this.SpriteRendererMain.flipX = true;
            }
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


        public void HideRequestFromPlayerView()
        {
            this.isShowRequested = false;
            this.isHideRequested = true;
        }
        public void ShowRequestFromPlayerView()
        {
            this.isShowRequested = true;
            this.isHideRequested = false;
        }
        public void OperateShowAndHide()
        {
            if (this.isHideRequested && this.isActivated)
            {
                StopCoroutine(this.HideOperation());
                StartCoroutine(this.HideOperation());
            }

            if (this.isShowRequested && !this.isActivated)
            {
                StopCoroutine(this.ShowOperation());
                StartCoroutine(this.ShowOperation());
            }

            this.isHideRequested = false;
            this.isShowRequested = false;
        }

        public IEnumerator HideOperation()
        {
            if (this.SpriteRendererMain == null) yield break;

            Color color = this.SpriteRendererMain.color;
            float startAlpha = color.a;
            float time = 0f;

            while (time < this.duration)
            {
                float t = time / duration;
                float alpha = Mathf.Lerp(startAlpha, 0, t);
                color.a = alpha;
                this.SpriteRendererMain.color = color;

                time += Time.deltaTime;
                yield return null;
            }

            // 마지막에 정확하게 targetAlpha 적용
            color.a = 0;
            this.SpriteRendererMain.color = color;

            this.isActivated = false;
        }
        public IEnumerator ShowOperation()
        {
            if (this.SpriteRendererMain == null) yield break;

            Color color = this.SpriteRendererMain.color;
            float startAlpha = color.a;
            float time = 0f;

            while (time < this.duration)
            {
                float t = time / duration;
                float alpha = Mathf.Lerp(startAlpha, this.SpriteBaseAlpha, t);
                color.a = alpha;
                this.SpriteRendererMain.color = color;

                time += Time.deltaTime;
                yield return null;
            }

            // 마지막에 정확하게 targetAlpha 적용
            color.a = this.SpriteBaseAlpha;
            this.SpriteRendererMain.color = color;

            this.isActivated = true;
        }
    }
}