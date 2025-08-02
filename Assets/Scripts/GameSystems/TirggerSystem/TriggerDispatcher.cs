using System;
using System.Collections;

using UnityEngine;

using Foundations.Architecture.ReferencesHandler;

namespace GameSystems.TriggerSystem
{
    // 해당 시스템은 코드 최적화 순서에서 수행하자.
    // 이유 : 아직 모든 Trigger 흐름 기반을 Dispatcher에 연결해야한다는 확신이 안선다.
    //      -> 이곳에 연결하는 순간, 해당 Trigger에 대한 이벤트 흐름이 규격화가 되어버린다. --> 이건 확장성을 제한하는 방향임.

    [Serializable]
    public class TriggerSystemHandler : IDynamicReferenceHandler
    {
        public ITriggerDispatcher ITriggerDispatcher;
    }

    public interface ITriggerDispatcher
    {
        public IEnumerator OperateTriggerEvent(TriggerData triggerData);
    }

    // 동일한 조건의 Trigger는 작동 순서가 필요함.
    public class TriggerDispatcher : MonoBehaviour, ITriggerDispatcher
    {
        // 이곳에 각종 Trigger를 연결해줄 System이 존재합니다.
        // or Trigger와 Link된 DB가 존재합니다.

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