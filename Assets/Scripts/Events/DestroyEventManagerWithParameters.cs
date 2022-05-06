namespace Assets.Scripts.Events
{
    public class DestroyEventManagerWithParameters<TParam> : DestroyEventManager, IHaveParameter<TParam>
    {
        private TParam _parameter;

        public void SetParameter(TParam param)
        {
            _parameter = param;
        }

        public TParam GetParameter()
        {
            return _parameter;
        }
    }
}
