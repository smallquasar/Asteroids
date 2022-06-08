using System;

namespace Assets.Scripts.Events
{
    public interface IObserver
    {
        void Update(EventType eventType, EventArgs param);
    }
}
