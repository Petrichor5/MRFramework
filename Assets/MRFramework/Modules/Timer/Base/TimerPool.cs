using System;
using System.Collections.Generic;
using UnityEngine;

namespace MRFramework
{
    /// <summary>
    /// 内置定时器对象池
    /// </summary>
    public class TimerPool
    {
        private Stack<Timer> mTimerPool;
        private int mMaxCapacity;

        /// <summary>
        /// 初始化对象池
        /// </summary>
        /// <param name="maxCapacity">对象池可存放定时器最大容量</param>
        public TimerPool(int maxCapacity)
        {
            mTimerPool = new Stack<Timer>();
            mMaxCapacity = maxCapacity;
        }

        public void SetPoolCapacity(int maxCapacity)
        {
            mMaxCapacity = maxCapacity;
        }

        public Timer Rent(float delaySeconds, Action callback, bool isLoop = false)
        {
            Timer timer;
            if (mTimerPool.Count > 0)
                timer = mTimerPool.Pop();
            else
                timer = new Timer();

            timer.Init(delaySeconds, callback, isLoop);
            return timer;
        }

        public void Return(Timer timer)
        {
            if (mTimerPool.Count < mMaxCapacity)
            {
                timer.Clear();
                mTimerPool.Push(timer);
            }
            else
            {
                // 对象池已满，等待垃圾回收
                Debug.LogWarning("[TimerPool] 回收失败，对象池已满!!");
                timer.Clear();
            }
        }
        
        public void ClearPool()
        {
            foreach (var timer in mTimerPool)
            {
                timer.Clear();
            }
            mTimerPool.Clear();
        }
    }
}