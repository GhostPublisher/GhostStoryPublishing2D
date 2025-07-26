using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using TMPro;

using Foundations.Architecture.EventObserver;

namespace GameSystems.TestCase
{
    public class BattleFlowTest_Lobby : MonoBehaviour
    {
        private IEventObserverNotifier EventObserverNotifier;

        [Header("UI References")]
        [SerializeField] private TMP_InputField stageInputField;
        [SerializeField] private TMP_Text stageDisplayText;

        [SerializeField] private string BattleSceneName;

        private int currentStageID = 0;

        private void Awake()
        {
            this.EventObserverNotifier = new EventObserverNotifier();
        }

        public void OnLoadStageClicked()
        {
            // 입력값의 공백 제거
            string trimmedInput = stageInputField.text.Replace(" ", "");

            // 공백 제거 후 빈 문자열이면 리턴
            if (string.IsNullOrEmpty(trimmedInput))
            {
                Debug.LogError("스테이지 ID를 입력해주세요.");
                return;
            }

            // 숫자인지 검사
            if (int.TryParse(trimmedInput, out int inputID))
            {
                currentStageID = inputID;
            }
            else
            {
                stageDisplayText.text = "숫자만 입력해주세요.";
            }
        }

        public void OperateSceneConversion()
        {
            this.OnLoadStageClicked();

            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadScene(this.BattleSceneName);
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Debug.Log($"현재 StageID : {this.currentStageID}");

            var data = new BattleSceneSystem.InitialSetBattleSceneFlowControllerEvent();
            data.StageID = this.currentStageID;

            this.EventObserverNotifier.NotifyEvent(data);
        }
    }
}


