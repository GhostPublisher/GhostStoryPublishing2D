using System;

namespace Foundations.Architecture.EventObserver
{
    public class EventObserverNotifier_Example
    {
        private IEventObserverNotifier EventObserverNotifer;

        public EventObserverNotifier_Example()
        {
            this.EventObserverNotifer = new EventObserverNotifier();
        }

        // Notify 요청.
        public void Notify_Ex()
        {
            EventData_Ex01 eventData_Ex01 = new EventData_Ex01();
            eventData_Ex01.Name = "Ex01";

            this.EventObserverNotifer.NotifyEvent(eventData_Ex01);
        }
    }

    public class EventObserverListener_Example : IEventObserverListener, IDisposable
    {
        private IEventObserverLinker EventObserverLinker;

        public EventObserverListener_Example()
        {
            this.EventObserverLinker = new EventObserverLinker(this);

            this.Register_Ex();
        }
        // 구독할 EventDataType 등록.
        public void Register_Ex()
        {
            this.EventObserverLinker.RegisterSubscriberListener<EventData_Ex01>();
        }

        // 제거 시, 모든 EventDataType 등록 해제 필수.
        public void Dispose()
        {
            this.EventObserverLinker.Dispose();
        }
        // 부분 해제, 거의 사용할 일 없을 듯?
        public void Remove_Ex()
        {
            this.EventObserverLinker.RemoveSubscriberListener<EventData_Ex01>();
        }

        public void UpdateData(IEventData eventData)
        {
            switch (eventData)
            {
                case EventData_Ex01:
                    UnityEngine.Debug.Log($"Name : {((EventData_Ex01)eventData).Name}");
                    break;
                default:
                    break;
            }
        }
    }

    public class EventData_Ex01 : IEventData
    {
        public string Name;
    }
}