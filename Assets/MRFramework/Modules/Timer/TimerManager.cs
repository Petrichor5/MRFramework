using System;
using System.Collections.Generic;
using UnityEngine;

namespace MRFramework
{
    [MonoSingletonPath("MRFramework/TimerManager")]
    public class TimerManager : MonoSingleton<TimerManager>
    {
        private List<TimerBase> m_TimerList = new List<TimerBase>();
        private Dictionary<long, TimerBase> m_TimerDic = new Dictionary<long, TimerBase>();

        // 对象池
        private SimpleObjectPool<SingleTimer> m_SingleTimerPool;
        private SimpleObjectPool<LoopTimer> m_LoopTimerPool;

        private long m_NextTimerId = 0;

        public override void OnSingletonInit()
        {
            m_SingleTimerPool = new SimpleObjectPool<SingleTimer>(() =>
            {
                return new SingleTimer();
            },
            (timer) =>
            {
                timer.Clear();
            });

            m_LoopTimerPool = new SimpleObjectPool<LoopTimer>(() =>
            {
                return new LoopTimer();
            },
            (timer) =>
            {
                timer.Clear();
            });
        }

        private void OnDestroy()
        {
            m_TimerList.Clear();
            m_TimerDic.Clear();
            m_SingleTimerPool.ClearPool();
            m_LoopTimerPool.ClearPool();
        }

        #region 对外接口

        /// <summary>
        /// 添加计时器
        /// </summary>
        /// <param name="duration">计时时间</param>
        /// <param name="onTimerComplete">计时结束回调</param>
        /// <param name="isLooping">是否循环计时</param>
        /// <param name="interval">多少秒执行一次onInterval回调</param>
        /// <param name="onInterval">每过interval秒执行一次这个回调</param>
        /// <param name="calculateEscapedTime">是否计算逃逸时间(只对循环计时器有效)</param>
        /// <returns>计时器唯一ID</returns>
        public long AddTimer(float duration, Action onTimerComplete = null, bool isLooping = false, float interval = 0f, Action<float> onInterval = null, bool calculateEscapedTime = true)
        {
            long timerId = m_NextTimerId++;
            TimerBase timer;

            if (isLooping)
            {
                var loopTimer = m_LoopTimerPool.Allocate();
                loopTimer.SetCalculateEscapedTime(calculateEscapedTime);
                timer = loopTimer;
            }
            else
            {
                timer = m_SingleTimerPool.Allocate();
            }

            timer.StartTimer(duration, () => OnTimerComplete(timerId, onTimerComplete), isLooping, interval, onInterval);
            m_TimerDic.Add(timerId, timer);
            m_TimerList.Add(timer);
            return timerId;
        }

        /// <summary>
        /// 移除计时器
        /// </summary>
        /// <param name="timerId">计时器唯一ID</param>
        public void RemoveTimer(long timerId)
        {
            if (m_TimerDic.TryGetValue(timerId, out var timer))
            {
                m_TimerDic.Remove(timerId);
                m_TimerList.Remove(timer);
                RecycleTimer(timer);
            }
        }

        /// <summary>
        /// 获取计时器
        /// </summary>
        /// <param name="timerId">计时器唯一ID</param>
        /// <returns>计时器对象</returns>
        public TimerBase GetTimer(long timerId)
        {
            if (m_TimerDic.ContainsKey(timerId))
            {
                return m_TimerDic[timerId];
            }
            return null;
        }

        /// <summary>
        /// 清空计时器对象池
        /// </summary>
        public void ClearTimerPool()
        {
            m_SingleTimerPool.ClearPool();
            m_LoopTimerPool.ClearPool();
        }

        #endregion

        #region 内部实现

        private void OnTimerComplete(long timerId, Action onTimerComplete)
        {
            if (m_TimerDic.ContainsKey(timerId))
            {
                if (m_TimerDic[timerId] is LoopTimer)
                {
                    m_TimerDic[timerId].ResetTimer();
                }
                else
                {
                    RemoveTimer(timerId);
                }

                onTimerComplete?.Invoke();
            }
        }

        private void RecycleTimer(TimerBase timer)
        {
            if (timer is LoopTimer loopTimer)
            {
                m_LoopTimerPool.Recycle(loopTimer);
            }
            else if (timer is SingleTimer singleTimer)
            {
                m_SingleTimerPool.Recycle(singleTimer);
            }
        }

        private void Update()
        {
            for (int i = 0; i < m_TimerList.Count; i++)
            {
                if (m_TimerList[i].IsRunning())
                {
                    m_TimerList[i].UpdateTimer(Time.deltaTime);
                }
            }
        }

        #endregion
    }
}