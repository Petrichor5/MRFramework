using System.Collections;
using UnityEngine;

namespace MRFramework
{
    /// <summary>
    /// 单次计时器，计时结束后会被对象池回收
    /// </summary>
    public class SingleTimer : TimerBase
    {
        public override void UpdateTimer(float deltaTime)
        {
            if (!m_IsRunning) return;

            m_RemainingTime -= deltaTime;

            if (m_Interval > 0)
            {
                m_IntervalCounter -= deltaTime;
                if (m_IntervalCounter <= 0)
                {
                    m_OnInterval?.Invoke(m_Interval);
                    m_IntervalCounter = m_Interval;
                }
            }

            if (m_RemainingTime <= 0)
            {
                m_IsRunning = false;
                m_OnTimerComplete?.Invoke();
            }
        }
    }
}