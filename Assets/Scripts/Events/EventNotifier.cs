using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Events
{
    public class EventNotifier: INotifier
    {
        private List<ObserverInfo> _observers = new List<ObserverInfo>();

        public void Attach(IObserver observer, EventType eventType)
        {
            _observers.Add(new ObserverInfo() { Observer = observer, EventType = eventType });
        }

        public void Detach(IObserver observer, EventType eventType)
        {
            ObserverInfo obInfo = _observers.FirstOrDefault(x => x.Observer == observer && x.EventType == eventType);
            if (obInfo == null)
            {
                return;
            }

            _observers.Remove(obInfo);
        }

        public void Notify(EventType eventType, EventArgs param)
        {
            foreach (var observerInfo in _observers.Where(x => x.EventType == eventType))
            {
                observerInfo.Observer.Update(eventType, param);
            }
        }
    }
}
