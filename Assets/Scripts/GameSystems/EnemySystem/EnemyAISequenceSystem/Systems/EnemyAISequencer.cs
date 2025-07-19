using System.Collections.Generic;

using UnityEngine;

using Foundations.Architecture.ReferencesHandler;

using GameSystems.EnemySystem.EnemyUnitSystem;

namespace GameSystems.EnemySystem.EnemyAISequenceSystem
{
    public interface IEnemyAISequencer
    {

    }

    public class EnemyAISequencer : MonoBehaviour, IEnemyAISequencer
    {
        private EnemyUnitManagerDataDBHandler EnemyUnitManagerDataDBHandler;

        private Queue<IEnemyUnitManager> enemyUnitManagers;

        private IEnemyUnitManager CurrentOperatedEnemyUnitManager;

        private bool isAllocated = false;

        private void Awake()
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            this.EnemyUnitManagerDataDBHandler = HandlerManager.GetDynamicDataHandler<EnemyUnitManagerDataDBHandler>();

            this.enemyUnitManagers = new();
        }

        public void AllocateEnemyAISequence()
        {
            // 이미 할당했을 경우 리턴.
            if (this.isAllocated) return;

            if (this.EnemyUnitManagerDataDBHandler.TryGetAll(out var datas))
            {
                // 일단은 순서대로.
                foreach (var data in datas)
                {
                    this.enemyUnitManagers.Enqueue(data.EnemyUnitFeatureInterfaceGroup.EnemyUnitManager);
                }
            }

            this.isAllocated = true;
//            Debug.Log($"할당된 Queue 개수 : {this.enemyUnitManagers.Count}");
        }

        public void ExecuteEnemyAI()
        {
            if (this.enemyUnitManagers.Count > 0)
            {
                this.CurrentOperatedEnemyUnitManager = this.enemyUnitManagers.Dequeue();

//                Debug.Log($"남은 Queue 개수 : {this.enemyUnitManagers.Count}");
                this.CurrentOperatedEnemyUnitManager.OperateEnemyAI();
            }
            else
            {
                // 생성되어 있는 EnemyAI의 모든 작업이 끝났을 때, 상위 시스템한테 다음에 수행할 작업을 수행하라. 라는 notify or interface를 통한 호출이 수행되는 부분이다.
                // 이전과 다르게, 작업 -> 생성 루틴이 아님. Enemy Turn에 수행되어야 할 작업들의 순차 실행을 관리하는 상위 시스템이 존재하고, 해당 상위 시스템에 요청하는 작업이 들어가는 부분이다. ( Like 단일 AI 연속 작업 -> 다음 AI 작업 요청 )
            }

        }

        // 특정 이유에 의해서, Enemy 루틴 작업에서 벗어난 경우, 다시 호출되는 지점. ( Like 이벤트에 의한 스토리 출력 이후 연결 작업 )
        public void CurrentOperatedEnemy_OperationContinue()
        {
            // Enemy AI 순서 할당. -> 해당 메소드 안에서 이미 Enemy AI 순서를 할당한 경우 바로 리턴해줌.
            this.AllocateEnemyAISequence();

            // 현재 진행하던 EnemyUnit이 없었으면 새로운 EnemyAI 진행.
            if (this.CurrentOperatedEnemyUnitManager == null)
            {
                this.ExecuteEnemyAI();
                return;
            }
            else
            {
                this.CurrentOperatedEnemyUnitManager.OperateEnemyAI();
            }
        }

        public void OperateNewTurnSettting()
        {
            if (this.EnemyUnitManagerDataDBHandler.TryGetAll(out var datas))
            {
                foreach(var data in datas)
                {
                    data.EnemyUnitFeatureInterfaceGroup.EnemyUnitManager.OperateNewTurnSetting();
                }
            }


            this.enemyUnitManagers.Clear();
            this.CurrentOperatedEnemyUnitManager = null;
            this.isAllocated = false;
        }
    }
}