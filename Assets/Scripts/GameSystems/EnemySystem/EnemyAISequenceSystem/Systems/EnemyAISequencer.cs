using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Foundations.Architecture.ReferencesHandler;

using GameSystems.EnemySystem.EnemyUnitSystem;

namespace GameSystems.EnemySystem.EnemyAISequenceSystem
{
    public interface IEnemyAISequencer
    {
        public void AllocateEnemyAISequence();
        public IEnumerator ExecuteEnemyAI_Coroutine();
    }

    public class EnemyAISequencer : MonoBehaviour, IEnemyAISequencer
    {
        private Queue<IEnemyUnitManager> enemyUnitManagers = new();

        private void Awake()
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            var EnemySystemHandler = HandlerManager.GetDynamicDataHandler<EnemySystemHandler>();
            EnemySystemHandler.IEnemyAISequencer = this;
        }

        public void AllocateEnemyAISequence()
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            var EnemyUnitManagerDataDBHandler = HandlerManager.GetDynamicDataHandler<EnemyUnitManagerDataDBHandler>();

            if (EnemyUnitManagerDataDBHandler.TryGetAll(out var datas))
            {
                // 일단은 순서대로.
                foreach (var data in datas)
                {
                    Debug.Log($"Enqueued : {data.UniqueID}");
                    this.enemyUnitManagers.Enqueue(data.EnemyUnitFeatureInterfaceGroup.EnemyUnitManager);
                }
            }
        }

        public IEnumerator ExecuteEnemyAI_Coroutine()
        {
            Debug.Log($"QueueCount : {this.enemyUnitManagers.Count}");

            while (this.enemyUnitManagers.Count > 0)
            {
                var enemyUnitManager = this.enemyUnitManagers.Dequeue();

                // Enemy 작업 수행 요청
                yield return StartCoroutine(enemyUnitManager.OperateEnemyAI_Coroutine());

                yield return null;
            }

            this.enemyUnitManagers.Clear();
        }
    }
}