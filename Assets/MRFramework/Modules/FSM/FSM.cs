using System;
using System.Collections.Generic;

namespace MRFramework
{
    public class FSM<T>
    {
        private readonly Dictionary<T, IFSMState> m_StateDic = new Dictionary<T, IFSMState>();

        private Action<T, T> m_OnStateChanged;
        private IFSMState m_CurrentState;
        private T m_CurrentStateId;
        private T m_PreviousStateId;

        /// <summary>
        /// 当前状态
        /// </summary>
        public IFSMState CurrentState => m_CurrentState;

        /// <summary>
        /// 当前状态 ID
        /// </summary>
        public T CurrentStateId => m_CurrentStateId;

        /// <summary>
        /// 上一个状态 ID
        /// </summary>
        public T PreviousStateId => m_PreviousStateId;

        public void AddState(T stateId, IFSMState state)
        {
            m_StateDic.Add(stateId, state);
        }

        public CustomState State(T stateId)
        {
            if (m_StateDic.ContainsKey(stateId))
            {
                return m_StateDic[stateId] as CustomState;
            }

            CustomState state = new CustomState();
            m_StateDic.Add(stateId, state);
            return state;
        }

        public void ChangeState(T stateId)
        {
            if (stateId.Equals(m_CurrentStateId))
                return;

            if (m_StateDic.TryGetValue(stateId, out IFSMState state))
            {
                if(m_CurrentState != null && state.Condition())
                {
                    m_CurrentState.Exit();
                    m_PreviousStateId = m_CurrentStateId;
                    m_CurrentState = state;
                    m_CurrentStateId = stateId;
                    m_OnStateChanged?.Invoke(PreviousStateId, CurrentStateId);
                    m_CurrentState.Enter();
                }
            }
        }

        public void OnStateChanged(Action<T, T> onStateChanged)
        {
            this.m_OnStateChanged += onStateChanged;
        }

        public void StartState(T stateId)
        {
            if (m_StateDic.TryGetValue(stateId, out var state))
            {
                m_PreviousStateId = stateId;
                m_CurrentState = state;
                m_CurrentStateId = stateId;
                state.Enter();
            }
        }

        public void FixedUpdate()
        {
            m_CurrentState?.FixedUpdate();
        }

        public void Update()
        {
            m_CurrentState?.Update();
        }

        public void OnGUI()
        {
            m_CurrentState?.OnGUI();
        }

        public void Clear()
        {
            m_CurrentState = null;
            m_CurrentStateId = default;
            m_StateDic.Clear();
        }
    }
}

