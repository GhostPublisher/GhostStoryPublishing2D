using System;
using System.Collections.Generic;

namespace Foundations.Architecture.EventObserver
{
    public interface IEventObserverLinker
    {
        public void RegisterSubscriberListener<T>() where T : IEventData;
        public void RemoveSubscriberListener<T>() where T : IEventData;
        public void Dispose();
    }

    public class EventObserverLinker : IEventObserverLinker
    {
        private IEventObserverListener eventObserverListener;

        private HashSet<Type> eventObserverKeys;

        public EventObserverLinker(IEventObserverListener eventObserverListener)
        {
            this.eventObserverListener = eventObserverListener;

            this.eventObserverKeys = new();
        }

        public void RegisterSubscriberListener<T>() where T : IEventData
        {
            Type eventType = typeof(T);

            this.eventObserverKeys.Add(eventType);
            EventObserverDispatcher.Instance.RegisterSubscriberListener<T>(this.eventObserverListener);
        }

        public void RemoveSubscriberListener<T>() where T : IEventData
        {
            Type eventType = typeof(T);

            this.eventObserverKeys.Remove(eventType); // 중복 허용된다면 문제가 됨
            EventObserverDispatcher.Instance.RemoveSubscriberListener(eventType, this.eventObserverListener);
        }

        public void Dispose()
        {
            foreach (Type eventType in this.eventObserverKeys)
            {
                // Type 기반 Remove 호출 (Type 직접 전달)
                EventObserverDispatcher.Instance.RemoveSubscriberListener(eventType, this.eventObserverListener);
            }

            this.eventObserverKeys.Clear();
        }
    }
}