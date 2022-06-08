using System;
using System.Collections.Generic;

namespace Assets.Scripts.Events
{
    public class EventNotifier: INotifier
    {
        private List<IObserver> _observers = new List<IObserver>();

        public void Attach(IObserver observer)
        {
            _observers.Add(observer);
        }

        public void Detach(IObserver observer)
        {
            _observers.Remove(observer);
        }

        public void Notify(EventType eventType, EventArgs param)
        {
            foreach (var observer in _observers)
            {
                observer.Update(eventType, param);
            }
        }
    }
}
