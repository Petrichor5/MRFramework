using System.Collections;
using UnityEngine;

namespace MRFramework
{
    /// <summary>
    /// 循环计时器，计时结束后重置继续计时，只有被Remove的时候才会被对象池回收
    /// </summary>
    public class LoopTimer : TimerBase
    {
        private bool m_CalculateEscapedTime = true;

        // 设置一个最大允许的 deltaTime，防止逃逸时间过大
        // 选择0.1秒的理由：
        // 适应帧率波动：0.1秒（即100毫秒）相当于大约6帧的时间间隔，这可以帮助处理短暂的帧率波动，而不会让时间跳跃过于明显。
        // 确保流畅性：这个值足够小，不会对玩家的计时体验造成显著影响，同时可以避免因偶发的帧率下降导致计时器失去同步。
        private const float m_MaxDeltaTime = 0.1f;

        public void SetCalculateEscapedTime(bool value)
        {
            m_CalculateEscapedTime = value;
        }

        public override void UpdateTimer(float deltaTime)
        {
            if (!m_IsRunning)
                return;

            // 限制 deltaTime，防止因帧率问题导致的巨大时间跳跃
            deltaTime = Mathf.Min(deltaTime, m_MaxDeltaTime);

            m_RemainingTime -= deltaTime;

            if (m_Interval > 0)
            {
                m_IntervalCounter -= deltaTime;

                while (m_IntervalCounter <= 0 && m_Interval > 0)
                {
                    m_OnInterval?.Invoke(m_RemainingTime);
                    m_IntervalCounter += m_Interval;
                }
            }

            if (m_RemainingTime <= 0)
            {
                m_OnTimerComplete?.Invoke();

                if (m_CalculateEscapedTime)
                {
                    // 计算逃逸时间
                    m_RemainingTime += m_Duration;
                }
                else
                {
                    // 不计算逃逸时间，重置计时
                    m_RemainingTime = m_Duration;
                }
            }
        }

        public override void Clear()
        {
            base.Clear();
            m_CalculateEscapedTime = true;
        }
    }
}