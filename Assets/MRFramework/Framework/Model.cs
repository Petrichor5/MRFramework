namespace MRFramework
{
    public interface IModel : IBelongToArchitecture, ICanGetUtility
    {
        
    }

    public abstract class AModel
    {
        public abstract void OnInit();
        public abstract void OnDestroy();
    }
}