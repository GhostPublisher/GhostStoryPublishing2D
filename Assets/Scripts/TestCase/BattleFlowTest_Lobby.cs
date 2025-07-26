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
            // �Է°��� ���� ����
            string trimmedInput = stageInputField.text.Replace(" ", "");

            // ���� ���� �� �� ���ڿ��̸� ����
            if (string.IsNullOrEmpty(trimmedInput))
            {
                Debug.LogError("�������� ID�� �Է����ּ���.");
                return;
            }

            // �������� �˻�
            if (int.TryParse(trimmedInput, out int inputID))
            {
                currentStageID = inputID;
            }
            else
            {
                stageDisplayText.text = "���ڸ� �Է����ּ���.";
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
            Debug.Log($"���� StageID : {this.currentStageID}");

            var data = new BattleSceneSystem.InitialSetBattleSceneFlowControllerEvent();
            data.StageID = this.currentStageID;

            this.EventObserverNotifier.NotifyEvent(data);
        }
    }
}


