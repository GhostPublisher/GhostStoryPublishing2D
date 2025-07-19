using UnityEngine;

namespace Foundations.Architecture.EventObserver
{
    public class EventData_Ex02 : IEventData
    {
        public string Name;
    }

    public class EventObserverNotifier_MonoBehaviourExample : MonoBehaviour
    {
        private IEventObserverNotifier EventObserverNotifer;

        private void Awake()
        {
            this.EventObserverNotifer = new EventObserverNotifier();
        }

        // Notify 요청.
        public void Notify_Ex()
        {
            EventData_Ex02 eventData_Ex02 = new EventData_Ex02();
            eventData_Ex02.Name = "Ex01";

            this.EventObserverNotifer.NotifyEvent(eventData_Ex02);
        }
    }

    public class EventObserverListener_MonoBehaviourExample : MonoBehaviour, IEventObserverListener
    {
        private IEventObserverLinker EventObserverLinker;

        private void Awake()
        {
            this.EventObserverLinker = new EventObserverLinker(this);
        }

        private void OnEnable()
        {
            this.EventObserverLinker.RegisterSubscriberListener<EventData_Ex02>();
        }

        private void OnDisable()
        {
            this.EventObserverLinker.Dispose();
        }

        public void UpdateData(IEventData eventData)
        {
            switch (eventData)
            {
                case EventData_Ex02:
                    UnityEngine.Debug.Log($"Name : {((EventData_Ex02)eventData).Name}");
                    break;
                default:
                    break;
            }
        }
    }
}