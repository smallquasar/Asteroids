namespace Assets.Scripts.Events
{
    public interface INotifier
    {
        void Attach(IObserver observer, int id);
        void Detach(IObserver observer, int id);
        void Notify();
    }
}
