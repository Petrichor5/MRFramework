using System;

namespace MRFramework
{
    /// <summary>
    /// 面向业务的对象池
    /// </summary>
    public class SimpleObjectPool<T> : Pool<T>
    {
        private readonly Action<T> m_ResetMethod;

        public SimpleObjectPool(Func<T> factoryMethod, Action<T> resetMethod = null, int initCount = 0)
        {
            m_Factory = new CustomObjectFactory<T>(factoryMethod);
            m_ResetMethod = resetMethod;

            for (var i = 0; i < initCount; i++)
            {
                m_CacheStack.Push(m_Factory.Create());
            }
        }

        public override bool Recycle(T obj)
        {
            m_ResetMethod?.Invoke(obj);

            m_CacheStack.Push(obj);

            return true;
        }
    }
}