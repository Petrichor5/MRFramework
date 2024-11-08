namespace MRFramework
{
    public interface ICommand : IBelongToArchitecture, ICanSetArchitecture, ICanGetSystem, ICanGetController, ICanGetUtility,
        ICanSendCommand, ICanSendQuery
    {
        void Execute();
    }

    public interface ICommand<TResult> : IBelongToArchitecture, ICanSetArchitecture, ICanGetSystem, ICanGetController,
        ICanGetUtility,ICanSendCommand, ICanSendQuery
    {
        TResult Execute();
    }

    public abstract class ACommand : ICommand
    {
        private IArchitecture m_Architecture;

        IArchitecture IBelongToArchitecture.GetArchitecture() => m_Architecture;

        void ICanSetArchitecture.SetArchitecture(IArchitecture architecture) => m_Architecture = architecture;

        void ICommand.Execute() => OnExecute();

        protected abstract void OnExecute();
    }

    public abstract class ACommand<TResult> : ICommand<TResult>
    {
        private IArchitecture m_Architecture;

        IArchitecture IBelongToArchitecture.GetArchitecture() => m_Architecture;

        void ICanSetArchitecture.SetArchitecture(IArchitecture architecture) => m_Architecture = architecture;

        TResult ICommand<TResult>.Execute() => OnExecute();

        protected abstract TResult OnExecute();
    }
}