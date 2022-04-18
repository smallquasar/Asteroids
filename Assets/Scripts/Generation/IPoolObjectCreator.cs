namespace Assets.Scripts.Generation
{
    public interface IPoolObjectCreator<T>
    {
        T Create();
    }
}
