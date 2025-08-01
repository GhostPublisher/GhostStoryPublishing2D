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
            yield return DoAnimation(target, data); // ���� ������

            // ���� �� ó��
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
            // ��� �ð��� �ʱ�ȭ�մϴ�.
            float elapsedTime = 0f;

            Vector2 startPosition = UITranform_.anchoredPosition;
            Vector2 endPosition = UIAnimationData_.TargetPosition;

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
                UITranform_.anchoredPosition = currentPosition;

                // �����Ӻ��� ��� �ð��� ������Ʈ�մϴ�.
                elapsedTime += Time.deltaTime;
                // ���� �����ӱ��� ����մϴ�.
                yield return null;
            }

            // �ִϸ��̼��� ���� ��, ��Ȯ�� �� ��ġ�� �����մϴ�.
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