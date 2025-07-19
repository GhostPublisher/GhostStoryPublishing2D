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

        // �ش� UI/UX�� ���� �ʱ� ��ġ�� �����մϴ�.
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
            // ��� �ð��� �ʱ�ȭ�մϴ�.
            float elapsedTime = 0f;

            Vector2 startPosition = this.UITransform.anchoredPosition;
            Vector2 endPosition = UIAnimationData_.EndPosition;

            // �ִϸ��̼��� ���� ������ ������ �����մϴ�.
            while (elapsedTime < UIAnimationData_.AnimationTime)
            {
                // ��� �ð��� ������ ����մϴ�.
                float t = elapsedTime / UIAnimationData_.AnimationTime;
                // �ִϸ��̼� ��� ���� ������ �����մϴ�.
                float curveValue = UIAnimationData_.AnimatinoCurve.Evaluate(t);

                // Lerp�� ����Ͽ� ���� ��ġ�� ����մϴ�.
                Vector2 currentPosition = Vector2.Lerp(startPosition, endPosition, curveValue);
                // UI ����� ��ġ�� ������Ʈ�մϴ�.
                this.UITransform.anchoredPosition = currentPosition;

                // �����Ӻ��� ��� �ð��� ������Ʈ�մϴ�.
                elapsedTime += Time.deltaTime;                
                // ���� �����ӱ��� ����մϴ�.
                yield return null;
            }

            // �ִϸ��̼��� ���� ��, ��Ȯ�� �� ��ġ�� �����մϴ�.
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