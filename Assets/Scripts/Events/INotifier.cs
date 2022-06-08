using System;

namespace Assets.Scripts.Events
{
    public interface INotifier
    {
        void Attach(IObserver observer);
        void Detach(IObserver observer);
        void Notify(EventType eventType, EventArgs param);
    }
}
