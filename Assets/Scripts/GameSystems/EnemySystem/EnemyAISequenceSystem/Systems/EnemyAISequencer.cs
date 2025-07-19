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
            // �̹� �Ҵ����� ��� ����.
            if (this.isAllocated) return;

            if (this.EnemyUnitManagerDataDBHandler.TryGetAll(out var datas))
            {
                // �ϴ��� �������.
                foreach (var data in datas)
                {
                    this.enemyUnitManagers.Enqueue(data.EnemyUnitFeatureInterfaceGroup.EnemyUnitManager);
                }
            }

            this.isAllocated = true;
//            Debug.Log($"�Ҵ�� Queue ���� : {this.enemyUnitManagers.Count}");
        }

        public void ExecuteEnemyAI()
        {
            if (this.enemyUnitManagers.Count > 0)
            {
                this.CurrentOperatedEnemyUnitManager = this.enemyUnitManagers.Dequeue();

//                Debug.Log($"���� Queue ���� : {this.enemyUnitManagers.Count}");
                this.CurrentOperatedEnemyUnitManager.OperateEnemyAI();
            }
            else
            {
                // �����Ǿ� �ִ� EnemyAI�� ��� �۾��� ������ ��, ���� �ý������� ������ ������ �۾��� �����϶�. ��� notify or interface�� ���� ȣ���� ����Ǵ� �κ��̴�.
                // ������ �ٸ���, �۾� -> ���� ��ƾ�� �ƴ�. Enemy Turn�� ����Ǿ�� �� �۾����� ���� ������ �����ϴ� ���� �ý����� �����ϰ�, �ش� ���� �ý��ۿ� ��û�ϴ� �۾��� ���� �κ��̴�. ( Like ���� AI ���� �۾� -> ���� AI �۾� ��û )
            }

        }

        // Ư�� ������ ���ؼ�, Enemy ��ƾ �۾����� ��� ���, �ٽ� ȣ��Ǵ� ����. ( Like �̺�Ʈ�� ���� ���丮 ��� ���� ���� �۾� )
        public void CurrentOperatedEnemy_OperationContinue()
        {
            // Enemy AI ���� �Ҵ�. -> �ش� �޼ҵ� �ȿ��� �̹� Enemy AI ������ �Ҵ��� ��� �ٷ� ��������.
            this.AllocateEnemyAISequence();

            // ���� �����ϴ� EnemyUnit�� �������� ���ο� EnemyAI ����.
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