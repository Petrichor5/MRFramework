using System.Collections.Generic;

namespace MRFramework
{
    /// <summary>
    /// 存储 List 对象池，用于优化减少 new 调用次数。
    /// </summary>
    public static class ListPool<T>
    {
        /// <summary>
        /// 栈对象：存储多个List
        /// </summary>
        static Stack<List<T>> m_ListStack = new Stack<List<T>>(8);

        /// <summary>
        /// 出栈：获取某个List对象
        /// </summary>
        /// <returns></returns>
        public static List<T> Get()
        {
            if (m_ListStack.Count == 0)
            {
                return new List<T>(8);
            }

            return m_ListStack.Pop();
        }

        /// <summary>
        /// 入栈：将List对象添加到栈中
        /// </summary>
        /// <param name="toRelease"></param>
        public static void Release(List<T> toRelease)
        {
            if (m_ListStack.Contains(toRelease))
            {
                throw new System.InvalidOperationException ("重复回收 List，The List is released even though it is in the pool");
            }

            toRelease.Clear();
            m_ListStack.Push(toRelease);
        }
    }

    public static class ListPoolExtensions
    {
        public static void Release2Pool<T>(this List<T> self)
        {
            ListPool<T>.Release(self);
        }
    }
}