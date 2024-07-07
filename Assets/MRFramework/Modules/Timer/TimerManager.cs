using System;
using System.Diagnostics;

namespace MRFramework
{
    [MonoSingletonPath("MRFramework/TimerManager")]
    public class TimerManager : MonoSingleton<TimerManager>
    {
        private TimerPool mTimerPool;
        private Stopwatch mStopwatch;
        private TimerPriorityQueue mTimerQueue;
        private int mNextTimerId = 1;

        public override void OnSingletonInit()
        {
            mTimerPool = new TimerPool(128);
            mStopwatch = new Stopwatch();
            mTimerQueue = new TimerPriorityQueue();
            mStopwatch.Start();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            mStopwatch.Stop();
            lock (mTimerQueue)
            {
                mTimerQueue.Clear();
            }
        }
        
        public void SetPoolCapacity(int maxCapacity)
        {
            mTimerPool.SetPoolCapacity(maxCapacity);
        }

        // 局部变量
        private long currentTime;
        private Timer timer;
        private void Update()
        {
            currentTime = mStopwatch.ElapsedMilliseconds;
            lock (mTimerQueue) timer = mTimerQueue.PeekFirst();
            if (timer != null)
            {
                if (currentTime >= timer.ExpirationTime)
                {
                    lock (mTimerQueue)
                    {
                        timer.Callback?.Invoke();
                        if (timer.IsLoop)
                        {
                            timer.ExpirationTime = currentTime + (long)(timer.DelaySeconds * 1000);
                            mTimerQueue.PopFirst();
                            mTimerQueue.AddTimer(timer);
                        }
                        else
                        {
                            mTimerPool.Return(timer);
                            mTimerQueue.PopFirst();
                        }
                    }
                }
            }
        }

        public int StartTimer(float delaySeconds, Action callback, bool isLoop = false)
        {
            if (delaySeconds <= 0) return -1;
            
            // 计算未来的计时器到期时间
            long expirationTime = mStopwatch.ElapsedMilliseconds + (long)(delaySeconds * 1000);

            // 创建计时器并添加到优先列表中
            Timer timer = mTimerPool.Rent(delaySeconds, callback, isLoop);
            timer.ExpirationTime = expirationTime;
            timer.Id = mNextTimerId++;
            lock (mTimerQueue)
            {
                mTimerQueue.AddTimer(timer);
            }
            
            return timer.Id;
        }

        public void RemoveTimer(int timerId)
        {
            lock (mTimerQueue)
            {
                Timer timerToRemove = mTimerQueue.TryGetTimer(timerId);
                 if (timerToRemove != null)
                 {
                     mTimerPool.Return(timerToRemove);
                     mTimerQueue.RemoveTimer(timerToRemove);
                 }
            }
        }

        public void RemoveAllTimer()
        {
            mStopwatch.Stop();
            lock (mTimerQueue)
            {
                mTimerQueue.Clear();
            }
        }

        public void ClearCache()
        {
            mTimerPool.ClearPool();
        }
    }
}