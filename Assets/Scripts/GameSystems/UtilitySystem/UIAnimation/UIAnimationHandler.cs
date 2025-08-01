using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Foundations.Architecture.ReferencesHandler;

namespace GameSystems.UtilitySystem
{
    public class UIAnimationHandler : MonoBehaviour, IUtilityReferenceHandler
    {
        private Dictionary<int, Coroutine> runningAnimations = new();
        private Dictionary<int, Queue<UIAnimationData>> animationQueues = new();

        public void DoAnimation_Ignore(RectTransform target, UIAnimationData animationData)
        {
            int key = target.GetInstanceID();
            if (runningAnimations.ContainsKey(key)) return;

            Coroutine coroutine = StartCoroutine(DoAnimation_Co(target, animationData, key));
            runningAnimations[key] = coroutine;
        }

        public void DoAnimation_Override(RectTransform target, UIAnimationData animationData)
        {
            int key = target.GetInstanceID();

            if (runningAnimations.TryGetValue(key, out Coroutine running))
            {
                StopCoroutine(running);

                this.runningAnimations.Remove(key);
                this.animationQueues.Remove(key);
            }

            Coroutine coroutine = StartCoroutine(DoAnimation_Co(target, animationData, key));
            runningAnimations[key] = coroutine;
        }

        public void DoAnimation_Queue(RectTransform target, UIAnimationData animationData)
        {
            int key = target.GetInstanceID();

            if (!runningAnimations.ContainsKey(key))
            {
                Coroutine coroutine = StartCoroutine(DoAnimation_Co(target, animationData, key));
                runningAnimations[key] = coroutine;
            }
            else
            {
                if (!animationQueues.ContainsKey(key))
                    animationQueues[key] = new Queue<UIAnimationData>();

                animationQueues[key].Enqueue(animationData);
            }
        }

        public IEnumerator DoAnimation_Co(RectTransform target, UIAnimationData data, int key)
        {
            yield return DoAnimation(target, data); // 실제 움직임

            // 끝난 후 처리
            if (animationQueues.TryGetValue(key, out var queue) && queue.Count > 0)
            {
                UIAnimationData next = queue.Dequeue();
                Coroutine coroutine = StartCoroutine(DoAnimation_Co(target, next, key));
                runningAnimations[key] = coroutine;
            }
            else
            {
                runningAnimations.Remove(key);
                animationQueues.Remove(key);
            }
        }

        private IEnumerator DoAnimation(RectTransform UITranform_, UIAnimationData UIAnimationData_)
        {
            // 경과 시간을 초기화합니다.
            float elapsedTime = 0f;

            Vector2 startPosition = UITranform_.anchoredPosition;
            Vector2 endPosition = UIAnimationData_.TargetPosition;

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
                UITranform_.anchoredPosition = currentPosition;

                // 프레임별로 경과 시간을 업데이트합니다.
                elapsedTime += Time.deltaTime;
                // 다음 프레임까지 대기합니다.
                yield return null;
            }

            // 애니메이션이 끝난 후, 정확한 끝 위치로 설정합니다.
            UITranform_.anchoredPosition = UIAnimationData_.TargetPosition;
        }
    }

    [Serializable]
    public class UIAnimationData
    {
        [SerializeField] public Vector2 StartPosition;
        [SerializeField] public Vector2 TargetPosition;
        [SerializeField] public float AnimationTime;
        [SerializeField] public AnimationCurve AnimatinoCurve;
    }
}