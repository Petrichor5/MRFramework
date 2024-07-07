namespace MRFramework
{
    public interface ISystem : IBelongToArchitecture, ICanSetArchitecture, ICanGetController, ICanGetUtility,
        ICanGetSystem
    {
        void Init();
    }

    public abstract class AbstractSystem : ISystem
    {
        private IArchitecture mArchitecture;

        IArchitecture IBelongToArchitecture.GetArchitecture() => mArchitecture;

        void ICanSetArchitecture.SetArchitecture(IArchitecture architecture) => mArchitecture = architecture;

        void ISystem.Init() => OnInit();

        protected abstract void OnInit();
    }
}