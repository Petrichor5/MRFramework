using System;
using System.Collections.Generic;
using MRFramework;

namespace MRFramework
{
    public class FSM<T>
    {
        private readonly Dictionary<T, IFSMState> mStateDic = new Dictionary<T, IFSMState>();

        private Action<T, T> onStateChanged;
        private IFSMState mCurrentState;
        private T mCurrentStateId;
        private T mPreviousStateId;

        /// <summary>
        /// 当前状态
        /// </summary>
        public IFSMState CurrentState => mCurrentState;

        /// <summary>
        /// 当前状态 ID
        /// </summary>
        public T CurrentStateId => mCurrentStateId;

        /// <summary>
        /// 上一个状态 ID
        /// </summary>
        public T PreviousStateId => mPreviousStateId;

        public void AddState(T stateId, IFSMState state)
        {
            mStateDic.Add(stateId, state);
        }

        public CustomState State(T stateId)
        {
            if (mStateDic.ContainsKey(stateId))
            {
                return mStateDic[stateId] as CustomState;
            }

            CustomState state = new CustomState();
            mStateDic.Add(stateId, state);
            return state;
        }

        public void ChangeState(T stateId)
        {
            if (stateId.Equals(mCurrentStateId))
                return;

            if (mStateDic.TryGetValue(stateId, out IFSMState state))
            {
                if(mCurrentState != null && state.Condition())
                {
                    mCurrentState.Exit();
                    mPreviousStateId = mCurrentStateId;
                    mCurrentState = state;
                    mCurrentStateId = stateId;
                    onStateChanged?.Invoke(PreviousStateId, CurrentStateId);
                    mCurrentState.Enter();
                }
            }
        }

        public void OnStateChanged(Action<T, T> onStateChanged)
        {
            this.onStateChanged += onStateChanged;
        }

        public void StartState(T stateId)
        {
            if (mStateDic.TryGetValue(stateId, out var state))
            {
                mPreviousStateId = stateId;
                mCurrentState = state;
                mCurrentStateId = stateId;
                state.Enter();
            }
        }

        public void FixedUpdate()
        {
            mCurrentState?.FixedUpdate();
        }

        public void Update()
        {
            mCurrentState?.Update();
        }

        public void OnGUI()
        {
            mCurrentState?.OnGUI();
        }

        public void Clear()
        {
            mCurrentState = null;
            mCurrentStateId = default;
            mStateDic.Clear();
        }
    }
}

