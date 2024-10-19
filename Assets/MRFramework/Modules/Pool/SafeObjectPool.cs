using System;

namespace MRFramework
{
    /// <summary>
    /// 更安全的对象池，带有一定的约束。
    /// </summary>
    public class SafeObjectPool<T> : Pool<T>, ISingleton where T : IPoolable, new()
    {
        #region Singleton

        void ISingleton.OnSingletonInit() { }
        
        void ISingleton.OnDispose() { }

        protected SafeObjectPool()
        {
            m_Factory = new DefaultObjectFactory<T>();
        }

        public static SafeObjectPool<T> Instance
        {
            get { return SingletonProperty<SafeObjectPool<T>>.Instance; }
        }

        public void Dispose()
        {
            SingletonProperty<SafeObjectPool<T>>.Dispose();
        }

        #endregion

        /// <summary>
        /// Init the specified maxCount and initCount.
        /// </summary>
        /// <param name="maxCount">Max Cache count.</param>
        /// <param name="initCount">Init Cache count.</param>
        public void Init(int maxCount, int initCount)
        {
            MaxCacheCount = maxCount;

            if (maxCount > 0)
            {
                initCount = Math.Min(maxCount, initCount);
            }

            if (CurCount < initCount)
            {
                for (var i = CurCount; i < initCount; ++i)
                {
                    T t = new T();
                    t.OnAllAllocated();
                    Recycle(t);
                }
            }
        }

        /// <summary>
        /// Gets or sets the max cache count.
        /// </summary>
        /// <value>The max cache count.</value>
        public int MaxCacheCount
        {
            get { return m_MaxCount; }
            set
            {
                m_MaxCount = value;

                if (m_CacheStack != null)
                {
                    if (m_MaxCount > 0)
                    {
                        if (m_MaxCount < m_CacheStack.Count)
                        {
                            int removeCount = m_CacheStack.Count - m_MaxCount;
                            while (removeCount > 0)
                            {
                                m_CacheStack.Pop();
                                --removeCount;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Allocate T instance.
        /// </summary>
        public override T Allocate()
        {
            var result = base.Allocate();
            result.IsRecycled = false;
            result.OnAllAllocated();
            return result;
        }

        /// <summary>
        /// Recycle the T instance
        /// </summary>
        /// <param name="t">T.</param>
        public override bool Recycle(T t)
        {
            if (t == null || t.IsRecycled)
            {
                return false;
            }

            if (m_MaxCount > 0)
            {
                if (m_CacheStack.Count >= m_MaxCount)
                {
                    t.OnRecycled();
                    return false;
                }
            }

            t.IsRecycled = true;
            t.OnRecycled();
            m_CacheStack.Push(t);

            return true;
        }
    }
}