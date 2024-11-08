namespace MRFramework
{
    public interface IFSMState
    {
        bool Condition();
        
        void Enter();
        void Update();
        void FixedUpdate();
        void Exit();
        
        void OnGUI();
    }
}
