namespace Foundations.Architecture.EventObserver
{
    public interface IEventObserverNotifier
    {
        public void NotifyEvent(IEventData eventData);
    }

    public class EventObserverNotifier : IEventObserverNotifier
    {
        public void NotifyEvent(IEventData eventData)
        {
            EventObserverDispatcher.Instance.NotifyEvent(eventData);
        }
    }
}