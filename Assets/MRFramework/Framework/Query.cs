namespace MRFramework
{
    public interface IQuery<TResult> : IBelongToArchitecture, ICanSetArchitecture, ICanGetController, ICanGetSystem,
        ICanSendQuery
    {
        TResult Do();
    }

    public abstract class AQuery<T> : IQuery<T>
    {
        public T Do() => OnDo();

        protected abstract T OnDo();


        private IArchitecture m_Architecture;

        public IArchitecture GetArchitecture() => m_Architecture;

        public void SetArchitecture(IArchitecture architecture) => m_Architecture = architecture;
    }
}