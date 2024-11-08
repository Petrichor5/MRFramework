using System;

namespace MRFramework
{
    public class CustomState : IFSMState
    {
        private Func<bool> m_OnCondition;
        private Action m_OnEnter;
        private Action m_OnUpdate;
        private Action m_OnFixedUpdate;
        private Action m_OnGUI;
        private Action m_OnExit;

        /// <summary>
        /// 条件：满足条件切换状态
        /// </summary>
        public CustomState OnCondition(Func<bool> onCondition)
        {
            this.m_OnCondition = onCondition;
            return this;
        }

        public CustomState OnEnter(Action onEnter)
        {
            this.m_OnEnter = onEnter;
            return this;
        }


        public CustomState OnUpdate(Action onUpdate)
        {
            this.m_OnUpdate = onUpdate;
            return this;
        }

        public CustomState OnFixedUpdate(Action onFixedUpdate)
        {
            this.m_OnFixedUpdate = onFixedUpdate;
            return this;
        }

        public CustomState OnGUI(Action onGUI)
        {
            this.m_OnGUI = onGUI;
            return this;
        }

        public CustomState OnExit(Action onExit)
        {
            this.m_OnExit = onExit;
            return this;
        }

        public bool Condition()
        {
            var result = m_OnCondition?.Invoke();
            return result == null || result.Value;
        }

        public void Enter()
        {
            m_OnEnter?.Invoke();
        }


        public void Update()
        {
            m_OnUpdate?.Invoke();
        }

        public void FixedUpdate()
        {
            m_OnFixedUpdate?.Invoke();
        }

        public void OnGUI()
        {
            m_OnGUI?.Invoke();
        }

        public void Exit()
        {
            m_OnExit?.Invoke();
        }
    }
}