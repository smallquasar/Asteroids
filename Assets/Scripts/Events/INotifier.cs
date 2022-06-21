using System;

namespace Assets.Scripts.Events
{
    public interface INotifier
    {
        void Attach(IObserver observer, EventType eventType);
        void Detach(IObserver observer);
        void Notify(EventType eventType, EventArgs param);
    }
}
