using System;

namespace MRFramework
{
    public class CustomState : IFSMState
    {
        private Func<bool> mOnCondition;
        private Action mOnEnter;
        private Action mOnUpdate;
        private Action mOnFixedUpdate;
        private Action mOnGUI;
        private Action mOnExit;

        /// <summary>
        /// 条件：满足条件切换状态
        /// </summary>
        public CustomState OnCondition(Func<bool> onCondition)
        {
            this.mOnCondition = onCondition;
            return this;
        }

        public CustomState OnEnter(Action onEnter)
        {
            this.mOnEnter = onEnter;
            return this;
        }


        public CustomState OnUpdate(Action onUpdate)
        {
            this.mOnUpdate = onUpdate;
            return this;
        }

        public CustomState OnFixedUpdate(Action onFixedUpdate)
        {
            this.mOnFixedUpdate = onFixedUpdate;
            return this;
        }

        public CustomState OnGUI(Action onGUI)
        {
            this.mOnGUI = onGUI;
            return this;
        }

        public CustomState OnExit(Action onExit)
        {
            this.mOnExit = onExit;
            return this;
        }

        public bool Condition()
        {
            var result = mOnCondition?.Invoke();
            return result == null || result.Value;
        }

        public void Enter()
        {
            mOnEnter?.Invoke();
        }


        public void Update()
        {
            mOnUpdate?.Invoke();
        }

        public void FixedUpdate()
        {
            mOnFixedUpdate?.Invoke();
        }

        public void OnGUI()
        {
            mOnGUI?.Invoke();
        }

        public void Exit()
        {
            mOnExit?.Invoke();
        }
    }
}