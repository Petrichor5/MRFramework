using System.Collections.Generic;

namespace MRFramework
{
    public abstract class PoolableObject<T> where T : PoolableObject<T>, new()
    {
        private static Stack<T> m_Pool = new Stack<T>(10);

        protected bool m_InPool = false;

        public static T Allocate()
        {
            var node = m_Pool.Count == 0 ? new T() : m_Pool.Pop();
            node.m_InPool = false;
            return node;
        }

        public void Recycle2Cache()
        {
            OnRecycle();
            m_InPool = true;
            m_Pool.Push(this as T);
        }

        protected abstract void OnRecycle();
    }
}