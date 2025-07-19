using System;
using System.Collections.Generic;

using Foundations.Patterns.Singleton;

namespace Foundations.Architecture.EventObserver
{
    public interface IEventObserverListener
    {
        // 케스팅하여 사용. ( 어차피 IEventData.EventObserverType와 'EventData'는 1 대 1 대응 )
        public void UpdateData(IEventData eventData);
    }

    public interface IEventData
    {

    }

    public class EventObserverDispatcher : Singleton<EventObserverDispatcher>
    {
        private Dictionary<Type, List<IEventObserverListener>> eventObserverListenerContainer;
        private HashSet<Type> notifyBlock;

        protected EventObserverDispatcher() { }

        protected override void OnInitialize()
        {
            this.eventObserverListenerContainer = new();
            this.notifyBlock = new();
        }

        /// <summary>
        /// 특정 IEventData 타입에 대한 Subscriber 등록
        /// </summary>
        public void RegisterSubscriberListener<T>(IEventObserverListener eventObserverListener) where T : IEventData
        {
            Type eventType = typeof(T);

            if(!this.eventObserverListenerContainer.TryGetValue(eventType, out var list))
            {
                list = new List<IEventObserverListener>();
                this.eventObserverListenerContainer[eventType] = list;
            }

            if (!list.Contains(eventObserverListener))
            {
                this.eventObserverListenerContainer[eventType].Add(eventObserverListener);
            }
        }

        /// <summary>
        /// 특정 IEventData 타입에 대한 Subscriber 제거
        /// </summary>
        /// <param name="eventType">구독 해제할 IEventData 타입 (Type 기반)</param>
        /// <param name="subscriber">해제할 구독자</param>
        public void RemoveSubscriberListener(Type eventType, IEventObserverListener eventObserverListener)
        {
            // 해당 타입에 대한 Subscriber 리스트가 존재하는지 확인
            if (this.eventObserverListenerContainer.TryGetValue(eventType, out var list))
            {
                // Subscriber 리스트에서 해당 Subscriber 제거
                list.Remove(eventObserverListener);

                // 리스트가 비었으면:
                // - 해당 타입 Key 자체 제거 (불필요한 빈 Key 제거)
                // - 해당 타입의 NotifyBlock 정보도 함께 제거 (깨끗한 상태 유지)
                if (list.Count == 0)
                {
                    this.eventObserverListenerContainer.Remove(eventType);
                    this.notifyBlock.Remove(eventType);
                }
            }
        }

        /// <summary>
        /// 이미 전달 중인 Notify가 있으면 무시하고, 없으면 즉시 동기 호출
        /// </summary>
        public void NotifyEvent(IEventData eventData)
        {
            Type eventType = eventData.GetType();

            // 구독자가 없으면 무시
            if (!this.eventObserverListenerContainer.TryGetValue(eventType, out var listenerList))
                return;

            // Block 중이면 무시
            if (this.notifyBlock.Contains(eventType))
                return;

            // Block 설정 (중복 방지)
            this.notifyBlock.Add(eventType);

            // listener.UpdateData(eventData) 중 하나의 예외 발생 방지.
            try
            {
                // 등록된 Subscriber들에게 사건 전달 (동기)
                foreach (var listener in listenerList)
                {
                    listener.UpdateData(eventData);
                }
            }
            finally
            {
                // Block 해제
                this.notifyBlock.Remove(eventType);
            }
        }
    }
}