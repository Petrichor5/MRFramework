using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace MRFramework
{
    /// <summary>
    /// 资源异步加载结果处理器
    /// </summary>
    public class AssetHandle<T> : AssetHandleBase
    {
        public object Key;
        public T Result;
        
        public IList<object> Keys;
        public IList<T> Results;
        
        public AsyncOperationHandle<T> AsyncHandle;
        public AsyncOperationHandle<IList<T>> AsyncHandles;
        
        private AssetLoader mLoader;
        private bool mIsMultiple = false; // 多资源加载
        
        private void Create(int id,  AssetLoader loader)
        {
            Id = id;
            mLoader = loader;
            mLoader.AddAssetHandle(this);
        }
        
        public AssetHandle(int id, object key, AssetLoader loader, AsyncOperationHandle<T> aoh)
        {
            Create(id, loader);
            Key = key;
            AsyncHandle = aoh;
            Result = aoh.Result;        
            mIsMultiple = false;
        }

        public AssetHandle(int id, object key, AssetLoader loader, AsyncOperationHandle<IList<T>> aoh)
        {
            Create(id, loader);
            Key = key;
            AsyncHandles = aoh;
            Results = aoh.Result;          
            mIsMultiple = true;
        }      

        /// <summary>
        /// 释放资源
        /// </summary>
        public override void Release()
        {
            if(!mIsMultiple)
                Addressables.Release(AsyncHandle);
            else
                Addressables.Release(AsyncHandles);
            mLoader.RemovAssetHandle(this);
        }
    
    }
}