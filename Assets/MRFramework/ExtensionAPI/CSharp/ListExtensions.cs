using System;
using System.Collections.Generic;

namespace MRFramework
{
    /// <summary>
    /// List 列表拓展
    /// </summary>
    public static class ListExtensions
    {
        private static Random m_Random;

        /// <summary>
        /// 随机打乱列表（洗牌）
        /// </summary>
        public static IList<T> Shuffle<T>(this IList<T> list)
        {
            if (m_Random == null) m_Random = new Random();

            int count = list.Count;
            while (count > 1)
            {
                count--;
                int index = m_Random.Next(count + 1);
                (list[index], list[count]) = (list[count], list[index]);
            }

            return list;
        }
    }
}