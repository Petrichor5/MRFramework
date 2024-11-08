using System;

namespace MRFramework
{
    /// <summary>
    /// 计时器基类
    /// </summary>
    public abstract class TimerBase
    {
        protected float m_Duration;
        protected float m_RemainingTime;
        protected bool m_IsRunning;
        protected Action m_OnTimerComplete;
        protected Action<float> m_OnInterval;
        protected float m_Interval;
        protected float m_IntervalCounter;
        protected bool m_IsLoop;

        #region 对外接口

        /// <summary>
        /// 暂停计时器
        /// </summary>
        public void Pause()
        {
            m_IsRunning = false;
        }

        /// <summary>
        /// 恢复计时器计时
        /// </summary>
        public void Resume()
        {
            m_IsRunning = true;
        }

        /// <summary>
        /// 计时器是否在运行
        /// </summary>
        public bool IsRunning()
        {
            return m_IsRunning;
        }

        /// <summary>
        /// 计时器剩余时间
        /// </summary>
        public float GetRemainingTime()
        {
            return m_RemainingTime;
        }

        /// <summary>
        /// 是否是循环计时器
        /// </summary>
        public bool IsLoop()
        {
            return m_IsLoop;
        }

        #endregion

        #region 内部实现

        public abstract void UpdateTimer(float deltaTime);

        public virtual void StartTimer(float duration, Action onTimerComplete, bool isLoop, float interval = 0f, Action<float> onInterval = null)
        {
            m_Duration = duration;
            m_RemainingTime = duration;
            m_OnTimerComplete = onTimerComplete;
            m_Interval = interval;
            m_OnInterval = onInterval;
            m_IntervalCounter = interval;
            m_IsRunning = true;
            m_IsLoop = isLoop;
        }

        public void ResetTimer()
        {
            m_RemainingTime = m_Duration;
            m_IntervalCounter = m_Interval;
            m_IsRunning = true;
        }

        public virtual void Clear()
        {
            m_Duration = 0f;
            m_Interval = 0f;
            m_IsRunning = false;
            m_RemainingTime = 0f;
            m_IntervalCounter = 0f;
            m_OnTimerComplete = null;
            m_OnInterval = null;
        }

        #endregion
    }
}