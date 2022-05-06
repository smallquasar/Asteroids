using System.Collections.Generic;

namespace Assets.Scripts.Events
{
    public class DestroyEventManager : INotifier
    {
        private Dictionary<int, IObserver> _observers = new Dictionary<int, IObserver>();
        private List<int> _currentObserversIdForNotify = new List<int>();

        public void Attach(IObserver observer, int id)
        {
            _observers.Add(id, observer);
        }

        public void Detach(IObserver observer, int id)
        {
            _observers.Remove(id);
        }

        public void SetCurrentObserversIdForNotify(List<int> currentObserversIdForNotify)
        {
            _currentObserversIdForNotify = currentObserversIdForNotify;
        }

        public void Notify()
        {
            foreach (var id in _currentObserversIdForNotify)
            {
                if (_observers.TryGetValue(id, out IObserver observerForNotify))
                {
                    observerForNotify.Update(this, EventType.Destroy);
                }                
            }
        }
    }
}
