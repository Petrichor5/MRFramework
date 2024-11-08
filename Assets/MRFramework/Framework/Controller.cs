namespace MRFramework
{
    public interface IController : IBelongToArchitecture, ICanSetArchitecture, 
        ICanGetController,
        ICanSendCommand, 
        ICanGetSystem,
        ICanSendQuery, 
        ICanGetUtility
    {
        void Init();
        void Dispose();
    }
    
    public abstract class AController : IController
    {
        private IArchitecture m_Architecturel;

        IArchitecture IBelongToArchitecture.GetArchitecture() => m_Architecturel;

        void ICanSetArchitecture.SetArchitecture(IArchitecture architecture) => m_Architecturel = architecture;

        void IController.Init() => OnInit();
        void IController.Dispose() => OnDispose();
        protected abstract void OnInit();
        protected abstract void OnDispose();
    }
}