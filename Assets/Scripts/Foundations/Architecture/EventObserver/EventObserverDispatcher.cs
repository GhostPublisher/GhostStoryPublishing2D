using System;
using System.Collections.Generic;

using Foundations.Patterns.Singleton;

namespace Foundations.Architecture.EventObserver
{
    public interface IEventObserverListener
    {
        // �ɽ����Ͽ� ���. ( ������ IEventData.EventObserverType�� 'EventData'�� 1 �� 1 ���� )
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
        /// Ư�� IEventData Ÿ�Կ� ���� Subscriber ���
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
        /// Ư�� IEventData Ÿ�Կ� ���� Subscriber ����
        /// </summary>
        /// <param name="eventType">���� ������ IEventData Ÿ�� (Type ���)</param>
        /// <param name="subscriber">������ ������</param>
        public void RemoveSubscriberListener(Type eventType, IEventObserverListener eventObserverListener)
        {
            // �ش� Ÿ�Կ� ���� Subscriber ����Ʈ�� �����ϴ��� Ȯ��
            if (this.eventObserverListenerContainer.TryGetValue(eventType, out var list))
            {
                // Subscriber ����Ʈ���� �ش� Subscriber ����
                list.Remove(eventObserverListener);

                // ����Ʈ�� �������:
                // - �ش� Ÿ�� Key ��ü ���� (���ʿ��� �� Key ����)
                // - �ش� Ÿ���� NotifyBlock ������ �Բ� ���� (������ ���� ����)
                if (list.Count == 0)
                {
                    this.eventObserverListenerContainer.Remove(eventType);
                    this.notifyBlock.Remove(eventType);
                }
            }
        }

        /// <summary>
        /// �̹� ���� ���� Notify�� ������ �����ϰ�, ������ ��� ���� ȣ��
        /// </summary>
        public void NotifyEvent(IEventData eventData)
        {
            Type eventType = eventData.GetType();

            // �����ڰ� ������ ����
            if (!this.eventObserverListenerContainer.TryGetValue(eventType, out var listenerList))
                return;

            // Block ���̸� ����
            if (this.notifyBlock.Contains(eventType))
                return;

            // Block ���� (�ߺ� ����)
            this.notifyBlock.Add(eventType);

            // listener.UpdateData(eventData) �� �ϳ��� ���� �߻� ����.
            try
            {
                // ��ϵ� Subscriber�鿡�� ��� ���� (����)
                foreach (var listener in listenerList)
                {
                    listener.UpdateData(eventData);
                }
            }
            finally
            {
                // Block ����
                this.notifyBlock.Remove(eventType);
            }
        }
    }
}