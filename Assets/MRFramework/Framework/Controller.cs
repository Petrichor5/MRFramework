namespace MRFramework
{
    public interface IController : IBelongToArchitecture, ICanSetArchitecture,ICanSendCommand, ICanGetSystem,
        ICanSendQuery, ICanGetUtility
    {
        void Init();
    }
    
    public abstract class AbstractController : IController
    {
        private IArchitecture mArchitecturel;

        IArchitecture IBelongToArchitecture.GetArchitecture() => mArchitecturel;

        void ICanSetArchitecture.SetArchitecture(IArchitecture architecture) => mArchitecturel = architecture;

        void IController.Init() => OnInit();

        protected abstract void OnInit();
    }
}