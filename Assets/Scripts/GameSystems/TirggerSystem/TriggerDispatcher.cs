using System;
using System.Collections;

using UnityEngine;

using Foundations.Architecture.ReferencesHandler;

namespace GameSystems.TriggerSystem
{
    // �ش� �ý����� �ڵ� ����ȭ �������� ��������.
    // ���� : ���� ��� Trigger �帧 ����� Dispatcher�� �����ؾ��Ѵٴ� Ȯ���� �ȼ���.
    //      -> �̰��� �����ϴ� ����, �ش� Trigger�� ���� �̺�Ʈ �帧�� �԰�ȭ�� �Ǿ������. --> �̰� Ȯ�强�� �����ϴ� ������.

    [Serializable]
    public class TriggerSystemHandler : IDynamicReferenceHandler
    {
        public ITriggerDispatcher ITriggerDispatcher;
    }

    public interface ITriggerDispatcher
    {
        public IEnumerator OperateTriggerEvent(TriggerData triggerData);
    }

    // ������ ������ Trigger�� �۵� ������ �ʿ���.
    public class TriggerDispatcher : MonoBehaviour, ITriggerDispatcher
    {
        // �̰��� ���� Trigger�� �������� System�� �����մϴ�.
        // or Trigger�� Link�� DB�� �����մϴ�.

        private void Awake()
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            var TriggerSystemHandler = HandlerManager.GetDynamicDataHandler<TriggerSystemHandler>();

            TriggerSystemHandler.ITriggerDispatcher = this;
        }

        public IEnumerator OperateTriggerEvent(TriggerData triggerData)
        {
            switch (triggerData)
            {
                case TriggerData_StageID:
                    var triggerData_StageID = (TriggerData_StageID)triggerData;
                    yield return StartCoroutine(this.OperateTriggerEvent_StageID(triggerData_StageID.StageID));
                    break;
                case TriggerData_TurnID:
                    var triggerData_TurnID = (TriggerData_TurnID)triggerData;
                    yield return StartCoroutine(this.OperateTriggerEvent_TurnID(triggerData_TurnID.TurnID));
                    break;
                case TriggerData_UnitPosition:
                    var triggerData_UnitPosition = (TriggerData_UnitPosition)triggerData;
                    yield return StartCoroutine(this.OperateTriggerEvent_UnitPosition(triggerData_UnitPosition.UnitPosition, triggerData_UnitPosition.UnitID));
                    break;
                default:
                    break;
            }

            yield return null;
        }

        public IEnumerator OperateTriggerEvent_StageID(int stageID)
        {


            yield return null;
        }

        public IEnumerator OperateTriggerEvent_TurnID(int stageID)
        {


            yield return null;
        }

        public IEnumerator OperateTriggerEvent_UnitPosition(Vector2Int position, int unitID)
        {


            yield return null;
        }
    }

    public interface TriggerData { }

    public class TriggerData_StageID : TriggerData
    {
        public int StageID;
    }

    public class TriggerData_TurnID : TriggerData
    {
        public int TurnID;
    }

    public class TriggerData_UnitPosition : TriggerData
    {
        public Vector2Int UnitPosition;
        public int UnitID;
    }
}