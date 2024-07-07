using System;
using UnityEngine;

namespace MRFramework
{
    public class TimeClock : IPoolable, IPoolType
    {
        public Guid ID;
        
        private float mTime;                     //计时时间
        private float mCachedTime;               // 缓存时间
        private float mTimePassed;               // 已经过去的时间
        private bool mIgnoreTimescale = false;   // 是否忽略缩放影响
        private bool mIsAutoLoop = false;        // 是否自动循环计时
        private Action mOnComplete = null;
        
        /// <summary>
        /// 是否计时完成
        /// </summary>
        public bool IsCompleted = false;
        
        /// <summary>
        /// 是否暂停
        /// </summary>
        public bool IsPause = false;

        /// <summary>
        /// // 当前时间
        /// </summary>
        private float CurrentTime => mIgnoreTimescale ? Time.realtimeSinceStartup : Time.time;

        /// <summary>
        /// 已过去的时间
        /// </summary>
        public float TimePassed => mTimePassed;

        public bool IsRecycled { get; set; }

        public static TimeClock Allocate()
        {
            return SafeObjectPool<TimeClock>.Instance.Allocate();
        }

        public void Recycle2Cache()
        {
            if (!SafeObjectPool<TimeClock>.Instance.Recycle(this))
            {
                Debug.LogWarning("[TimeClock] 回收失败，对象池已满！！");
            }
        }

        public void OnRecycled()
        {
            Clean();
        }

        /// <summary>
        /// 计时器设置
        /// </summary>
        /// <param name="time">计时时间</param>
        /// <param name="onComplete">计时完成回调</param>
        /// <param name="ignoreTimescale">是否忽略缩放影响</param>
        /// <param name="isAutoLoop">是否循环计时</param>
        public void TimerSetup(float time, Action onComplete, bool ignoreTimescale = false, bool isAutoLoop = false)
        {
            ID = Guid.NewGuid();
            
            this.mTime = time;
            this.mIsAutoLoop = isAutoLoop;
            this.mIgnoreTimescale = ignoreTimescale;
            this.mOnComplete = onComplete;

            mCachedTime = CurrentTime;
            IsCompleted = false;
        }

        /// <summary>
        /// 刷新计时器
        /// </summary>
        public void Update()
        {
            if (!IsPause && !IsCompleted)
            {
                // 过去的时间 = 当前时间 - 计时器创建时的缓存时间
                mTimePassed = CurrentTime - mCachedTime;
                
                // 计时结束
                if (mTimePassed >= mTime)
                {
                    mOnComplete?.Invoke();

                    if (mIsAutoLoop)
                    {
                        mCachedTime = CurrentTime;
                    }
                    else
                    {
                        Clean();
                    }
                }
            }
        }

        /// <summary>
        /// 暂停计时器
        /// </summary>
        public void Pause()
        {
            if(IsCompleted)
            {
                Debug.LogWarning("暂停失败，计时已结束。");
            }
            else
            {
                IsPause = true;
            }
        }

        /// <summary>
        /// 从暂停状态恢复计时
        /// </summary>
        public void Resume()
        {
            if (IsCompleted)
            {
                Debug.LogWarning("恢复计时失败，计时已结束。");
            }
            else
            {
                if(IsPause)
                {
                    // 重新设置缓存时间，继续计时
                    mCachedTime = CurrentTime - mTimePassed;
                    IsPause = false;
                }
                else
                {
                    Debug.LogWarning("计时并未处于暂停状态！");
                }
            }
        }

        /// <summary>
        /// 重置计时器，重新开始计时
        /// </summary>
        public void ReStart()
        {
            // 重新设置缓存时间，重新计时
            mCachedTime = CurrentTime;
            IsPause = false;
            IsCompleted = false;
        }

        /// <summary>
        /// 清除缓存数据
        /// </summary>
        public void Clean()
        {
            mTime = -1;
            IsCompleted = true;
            mOnComplete = null;
        }
    }
}