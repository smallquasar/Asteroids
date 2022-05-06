using System.Collections.Generic;

namespace Assets.Scripts.Events
{
    public class EventManager : INotifier
    {
        private List<IObserver> _observers = new List<IObserver>();

        public void Attach(IObserver observer, int id = 0)
        {
            _observers.Add(observer);
        }

        public void Detach(IObserver observer, int id = 0)
        {
            _observers.Remove(observer);
        }

        public void Notify()
        {
            foreach (var observer in _observers)
            {
                observer.Update(this, EventType.Update);
            }
        }
    }
}
