namespace MRFramework
{
    public interface ISystem : IBelongToArchitecture, ICanSetArchitecture, ICanGetController, ICanGetUtility,
        ICanGetSystem
    {
        void Init();
        void Dispose();
    }

    public abstract class ASystem : ISystem
    {
        private IArchitecture m_Architecture;

        IArchitecture IBelongToArchitecture.GetArchitecture() => m_Architecture;

        void ICanSetArchitecture.SetArchitecture(IArchitecture architecture) => m_Architecture = architecture;

        void ISystem.Init() => OnInit();
        void ISystem.Dispose() => OnDispose();

        protected abstract void OnInit();
        protected abstract void OnDispose();
    }
}