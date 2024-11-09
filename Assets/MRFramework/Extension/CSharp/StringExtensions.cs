namespace MRFramework
{
    public static class StringExtensions
    {
        /// <summary>
        /// 计算 FNV-1a 哈希值
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int ComputeFNV1aHash(this string str)
        {
            uint hash = 2166136261;
            foreach (char c in str)
            {
                hash = (hash ^ c) * 16777619;
            }

            return unchecked((int)hash);
        }
    }
}