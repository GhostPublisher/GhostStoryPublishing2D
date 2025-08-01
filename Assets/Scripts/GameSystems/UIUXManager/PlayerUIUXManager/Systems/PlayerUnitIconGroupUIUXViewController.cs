using System;
using System.Collections;

using UnityEngine;

namespace GameSystems.UIUXSystem
{
    public interface IPlayerUnitIconGroupUIUXViewController
    {
        public void InitialSetting(PlayerUnitActionUIUXHandler playerUnitActionUIUXHandler);

        public void TogglePlayerBehaviourUIUX(int uniqueID);

        public void Show_UIUX();
        public void HIde_UIUX();
        public void Show_BehaviourUIUX();
        public void Hide_BehaviourUIUX();

        public IEnumerator Show_UIUX_Coroutine();
        public IEnumerator HIde_UIUX_Coroutine();
        public IEnumerator Show_BehaviourUIUX_Coroutine();
        public IEnumerator Hide_BehaviourUIUX_Coroutine();
    }

    public class PlayerUnitIconGroupUIUXViewController : MonoBehaviour, IPlayerUnitIconGroupUIUXViewController
    {
        [SerializeField] private RectTransform PlayerUnitIconGroupUIUXRectTransform;

        [SerializeField] private UIAnimationData ShowData_UIUX;
        [SerializeField] private UIAnimationData HIdeData_UIUX;

        [SerializeField] private UIAnimationData ShowBehaviourData_UIUX;
        [SerializeField] private UIAnimationData HideBehaviourData_UIUX;

        private PlayerUnitActionUIUXHandler myPlayerUnitActionUIUXHandler;

        private Coroutine runningCoroutine;

        public void InitialSetting(PlayerUnitActionUIUXHandler playerUnitActionUIUXHandler)
        {
            this.myPlayerUnitActionUIUXHandler = playerUnitActionUIUXHandler;

            this.myPlayerUnitActionUIUXHandler.IPlayerUnitIconGroupUIUXViewController = this;
        }

        public void TogglePlayerBehaviourUIUX(int uniqueID)
        {
            if (!this.myPlayerUnitActionUIUXHandler.PlayerUnitIconGroupUIUXDatas.TryGetValue(uniqueID, out var data01)) return;

            foreach (var data02 in this.myPlayerUnitActionUIUXHandler.PlayerUnitIconGroupUIUXDatas.Values)
            {
                data02.IPlayerUnitIconUIUXViewController.ApplyInactiveColor();
                data02.PlayerUnitBehaviourIconGroupUIUXGameObject.SetActive(false);
            }

            data01.IPlayerUnitIconUIUXViewController.ApplyActiveColor();
            data01.PlayerUnitBehaviourIconGroupUIUXGameObject.SetActive(true);
        }


        public void Show_UIUX()
        {
            this.StopAllCoroutines();
            StartCoroutine(this.Show_UIUX_Coroutine());
        }
        public void HIde_UIUX()
        {
            this.StopAllCoroutines();
            StartCoroutine(this.HIde_UIUX_Coroutine());
        }
        public void Show_BehaviourUIUX()
        {
            this.StopAllCoroutines();
            StartCoroutine(this.Show_BehaviourUIUX_Coroutine());
        }
        public void Hide_BehaviourUIUX()
        {
            this.StopAllCoroutines();
            StartCoroutine(this.Hide_BehaviourUIUX_Coroutine());
        }

        public IEnumerator Show_UIUX_Coroutine()
        {
            if (this.runningCoroutine != null)
                StopCoroutine(this.runningCoroutine);

            IEnumerator animCoroutine = this.DoAnimation(this.PlayerUnitIconGroupUIUXRectTransform, this.ShowData_UIUX);
            this.runningCoroutine = this.StartCoroutine(animCoroutine);
            yield return animCoroutine;
        }
        public IEnumerator HIde_UIUX_Coroutine()
        {
            if (this.runningCoroutine != null)
                StopCoroutine(this.runningCoroutine);

            IEnumerator animCoroutine = this.DoAnimation(this.PlayerUnitIconGroupUIUXRectTransform, this.HIdeData_UIUX);
            this.runningCoroutine = this.StartCoroutine(animCoroutine);
            yield return animCoroutine;
        }
        public IEnumerator Show_BehaviourUIUX_Coroutine()
        {
            if (this.runningCoroutine != null)
                StopCoroutine(this.runningCoroutine);

            IEnumerator animCoroutine = this.DoAnimation(this.PlayerUnitIconGroupUIUXRectTransform, this.ShowBehaviourData_UIUX);
            this.runningCoroutine = this.StartCoroutine(animCoroutine);
            yield return animCoroutine;
        }
        public IEnumerator Hide_BehaviourUIUX_Coroutine()
        {
            if (this.runningCoroutine != null)
                StopCoroutine(this.runningCoroutine);

            IEnumerator animCoroutine = this.DoAnimation(this.PlayerUnitIconGroupUIUXRectTransform, this.HideBehaviourData_UIUX);
            this.runningCoroutine = this.StartCoroutine(animCoroutine);
            yield return animCoroutine;
        }

        public IEnumerator DoAnimation(RectTransform UITranform_, UIAnimationData UIAnimationData_)
        {
            // 경과 시간을 초기화합니다.
            float elapsedTime = 0f;

            Vector2 startPosition = UITranform_.anchoredPosition;
            Vector2 endPosition = UIAnimationData_.TargetPosition;

            if (UIAnimationData_.KeepX) endPosition.x = startPosition.x;
            if (UIAnimationData_.KeepY) endPosition.y = startPosition.y;

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
        [SerializeField] public bool KeepX;
        [SerializeField] public bool KeepY;

        [SerializeField] public Vector2 TargetPosition;
        [SerializeField] public float AnimationTime;
        [SerializeField] public AnimationCurve AnimatinoCurve;
    }
}