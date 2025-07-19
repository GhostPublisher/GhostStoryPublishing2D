using UnityEngine;

using Foundations.Architecture.EventObserver;

namespace GameSystems.EnemySystem.EnemyAISequenceSystem
{
    public class EnemyAISequencerEventListener : MonoBehaviour, IEventObserverListener
    {
        private IEventObserverLinker EventObserverLinker;

        [SerializeField] private EnemyAISequencer EnemyAISequencer;

        private void Awake()
        {
            this.EventObserverLinker = new EventObserverLinker(this);
        }

        private void OnEnable()
        {
            this.EventObserverLinker.RegisterSubscriberListener<AllocateEnemyAISequence>();
            this.EventObserverLinker.RegisterSubscriberListener<ExecuteEnemyAI>();
            this.EventObserverLinker.RegisterSubscriberListener<OperateNewTurnSetting>();
            this.EventObserverLinker.RegisterSubscriberListener<CurrentOperatedEnemy_OperationContinue>();
        }

        private void OnDisable()
        {
            this.EventObserverLinker.Dispose();
        }

        public void UpdateData(IEventData eventData)
        {
            switch (eventData)
            {
                // Enemy Turn의 시작 시, Enemy AI 순서 명시.
                case AllocateEnemyAISequence:
                    this.EnemyAISequencer.AllocateEnemyAISequence();
                    break;
                // 다음 Enemy AI 실행.
                case ExecuteEnemyAI:
                    this.EnemyAISequencer.ExecuteEnemyAI();
                    break;
                // 새로운 Turn이 시작되었을 떄, EnemyAISequenceSystem 초기화
                case OperateNewTurnSetting:
                    this.EnemyAISequencer.OperateNewTurnSettting();
                    break;
                // 불특정 이유로 인해 종료된 EnemyAI 흐름에 재 진입.
                case CurrentOperatedEnemy_OperationContinue:
                    this.EnemyAISequencer.CurrentOperatedEnemy_OperationContinue();
                    break;
                default:
                    break;
            }
        }

    }
}