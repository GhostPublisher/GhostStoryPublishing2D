using UnityEngine;
using UnityEngine.SceneManagement;

using TMPro;

using Foundations.Architecture.ReferencesHandler;

namespace GameSystems.TestCase
{
    public class BattleFlowTest_Lobby : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TMP_InputField stageInputField;
        [SerializeField] private TMP_Text stageDisplayText;

        [SerializeField] private string BattleSceneName;

        private int currentStageID = 0;

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
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            var BattleSceneHandler = HandlerManager.GetDynamicDataHandler<BattleSceneSystem.BattleSceneSystemHandler>();

            Debug.Log($"���� StageID : {this.currentStageID}");
            BattleSceneHandler.IBattleSceneFlowController.OperateBattleSceneFlow_StageSetting(this.currentStageID);
        }
    }
}


