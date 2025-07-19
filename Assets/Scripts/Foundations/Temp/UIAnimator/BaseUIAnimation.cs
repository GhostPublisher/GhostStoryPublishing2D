using System.Collections;
using UnityEngine;

namespace EventObserver
{
    public interface IBaseUIAnimation
    {
        public void StartAnimation(UIAnimationData UIAnimationData_);
        public IEnumerator DoAnimation(UIAnimationData UIAnimationData_);
    }

    public class BaseUIAnimation : MonoBehaviour, IBaseUIAnimation
    {
        [SerializeField] protected RectTransform UITransform;

        [SerializeField] private UIAnimationData showAnimation;
        [SerializeField] private UIAnimationData hideAnimation;

        // 해당 UI/UX의 가장 초기 위치를 지정합니다.
        public void PlaceToInitialPosition()
        {
            this.UITransform.anchoredPosition = showAnimation.StartPosition;
        }

        public void ShowAnimation()
        {
            this.StartAnimation(showAnimation);
        }

        public void HideAnimation()
        {
            this.StartAnimation(hideAnimation);
        }

        public void StartAnimation(UIAnimationData UIAnimationData_)
        {
            this.StopAllCoroutines();
            StartCoroutine(DoAnimation(UIAnimationData_));
        }
        public IEnumerator DoAnimation(UIAnimationData UIAnimationData_)
        {
            // 경과 시간을 초기화합니다.
            float elapsedTime = 0f;

            Vector2 startPosition = this.UITransform.anchoredPosition;
            Vector2 endPosition = UIAnimationData_.EndPosition;

            // 애니메이션이 끝날 때까지 루프를 실행합니다.
            while (elapsedTime < UIAnimationData_.AnimationTime)
            {
                // 경과 시간의 비율을 계산합니다.
                float t = elapsedTime / UIAnimationData_.AnimationTime;
                // 애니메이션 곡선을 따라 비율을 조정합니다.
                float curveValue = UIAnimationData_.AnimatinoCurve.Evaluate(t);

                // Lerp를 사용하여 현재 위치를 계산합니다.
                Vector2 currentPosition = Vector2.Lerp(startPosition, endPosition, curveValue);
                // UI 요소의 위치를 업데이트합니다.
                this.UITransform.anchoredPosition = currentPosition;

                // 프레임별로 경과 시간을 업데이트합니다.
                elapsedTime += Time.deltaTime;                
                // 다음 프레임까지 대기합니다.
                yield return null;
            }

            // 애니메이션이 끝난 후, 정확한 끝 위치로 설정합니다.
            this.UITransform.anchoredPosition = UIAnimationData_.EndPosition;
        }
    }

    [System.Serializable]
    public class UIAnimationData
    {
        [SerializeField] private Vector2 startPosition;
        [SerializeField] private Vector2 endPosition;
        [SerializeField] private float animationTime;
        [SerializeField] private AnimationCurve animatinoCurve;

        public Vector2 StartPosition { get => startPosition; set => startPosition = value; }
        public Vector2 EndPosition { get => endPosition; }
        public float AnimationTime { get => animationTime; }
        public AnimationCurve AnimatinoCurve { get => animatinoCurve; }
    }
}