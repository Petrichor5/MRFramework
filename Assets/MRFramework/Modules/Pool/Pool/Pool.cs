using System;
using System.Collections.Generic;

namespace MRFramework
{
    public abstract class Pool<T> : IPool<T>
    {
        #region ICountObserverable

        /// <summary>
        /// Gets the current count.
        /// </summary>
        /// <value>The current count.</value>
        public int CurCount
        {
            get { return m_CacheStack.Count; }
        }

        #endregion

        protected IObjectFactory<T> m_Factory;
        
        public void SetObjectFactory(IObjectFactory<T> factory)
        {
            m_Factory = factory;
        }

        public void SetFactoryMethod(Func<T> factoryMethod)
        {
            m_Factory = new CustomObjectFactory<T>(factoryMethod);
        }

        /// <summary>
        /// 存储相关数据的栈
        /// </summary>
        protected readonly Stack<T> m_CacheStack = new Stack<T>();

        public virtual void ClearPool(Action<T> onClearItem = null)
        {
            if (onClearItem != null)
            {
                foreach (var poolObject in m_CacheStack)
                {
                    onClearItem(poolObject);
                }
            }
            
            m_CacheStack.Clear();
        }
        
        /// <summary>
        /// default is 5
        /// </summary>
        protected int m_MaxCount = 5;

        public virtual T Allocate()
        {
            return m_CacheStack.Count == 0
                ? m_Factory.Create()
                : m_CacheStack.Pop();
        }

        public abstract bool Recycle(T obj);
    }
}