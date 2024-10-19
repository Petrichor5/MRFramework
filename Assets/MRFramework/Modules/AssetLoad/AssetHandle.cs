using UnityEngine.ResourceManagement.AsyncOperations;

namespace MRFramework
{
    /// <summary>
    /// 资源句柄
    /// </summary>
    public class AssetHandle
    {
        public AsyncOperationHandle AsyncOperationHandle;

        /// <summary>
        /// 引用存储构建的Key
        /// </summary>
        public string KeyName;

        /// <summary>
        /// 获取加载资源的Key
        /// </summary>
        public string ResKey;

        /// <summary>
        /// 获取该资源引用计数
        /// </summary>
        public int Reference;

        /// <summary>
        /// 该资源句柄已释放被释放
        /// </summary>
        public bool IsRelease;

        public AssetHandle(string key, string keyName, AsyncOperationHandle aoh)
        {
            ResKey = key;
            KeyName = keyName;
            AsyncOperationHandle = aoh;
            IsRelease = false;
        }

        /// <summary>
        /// 增加引用计数
        /// </summary>
        public void RefIncrease()
        {
            Reference++;
        }

        /// <summary>
        /// 减少引用计数
        /// </summary>
        public void RefDecrease()
        {
            Reference--;
        }
    }
}