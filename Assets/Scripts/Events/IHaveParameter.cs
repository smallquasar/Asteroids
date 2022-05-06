namespace Assets.Scripts.Events
{
    public interface IHaveParameter<TParam>
    {
        void SetParameter(TParam param);
        public TParam GetParameter();
    }
}
