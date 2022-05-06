namespace Assets.Scripts.Events
{
    public interface IObserver
    {
        void Update(INotifier notifier, EventType eventType);
    }
}
