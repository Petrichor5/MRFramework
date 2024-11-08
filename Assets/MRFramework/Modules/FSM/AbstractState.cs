namespace MRFramework
{
    public class AbstractState<TStateId, TTarget> : IFSMState
    {
        protected FSM<TStateId> FSM;
        protected TTarget Global;

        public AbstractState(FSM<TStateId> fsm, TTarget global)
        {
            this.FSM = fsm;
            this.Global = global;
        }

        bool IFSMState.Condition()
        {
            return OnCondition();
        }

        void IFSMState.Enter()
        {
            OnEnter();
        }

        void IFSMState.Update()
        {
            OnUpdate();
        }

        void IFSMState.FixedUpdate()
        {
            OnFixedUpdate();
        }

        public virtual void OnGUI()
        {
        }

        void IFSMState.Exit()
        {
            OnExit();
        }

        protected virtual bool OnCondition() => true;

        protected virtual void OnEnter()
        {
        }

        protected virtual void OnUpdate()
        {

        }

        protected virtual void OnFixedUpdate()
        {

        }

        protected virtual void OnExit()
        {

        }
    }
}

