using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace MRFramework
{
    /// <summary>
    /// 资源加载者，提供资源的加载卸载以及实例化功能
    /// </summary>
    public class AssetLoader : IPoolable, IPoolType
    {
        // 资源 AssetHandle 唯一Id
        private int mAssetHandleId = 0;

        public bool IsRecycled { get; set; }

        private static Dictionary<int, AssetHandleBase> mAssetHandleDic;

        [Obsolete("请使用 AssetLoader.Allocate() 获取对象")]
        public AssetLoader()
        {
            mAssetHandleDic = new Dictionary<int, AssetHandleBase>();
        }

        public static AssetLoader Allocate()
        {
            var loader = SafeObjectPool<AssetLoader>.Instance.Allocate();
            AssetManager.AddLoader(loader);
            return loader;
        }

        public void OnRecycled()
        {
            AssetManager.RemoveLoader(this);
            mAssetHandleDic.Clear();
        }

        public void Recycle2Cache()
        {
            if (!SafeObjectPool<AssetLoader>.Instance.Recycle(this))
            {
                Debug.LogWarning("[AssetLoader] 对象池已满，无法回收!");
            }
        }

        public void AddAssetHandle(AssetHandleBase ahb)
        {
            mAssetHandleDic.Add(ahb.Id, ahb);
        }

        public void RemovAssetHandle(AssetHandleBase ahb)
        {
            mAssetHandleDic.Remove(ahb.Id);
        }

        #region 获取已加载的资源

        public T GetAsset<T>(int handleId)
        {
            AssetHandle<T> ahb = null;

            if (mAssetHandleDic.ContainsKey(handleId))
            {
                ahb = mAssetHandleDic[handleId] as AssetHandle<T>;
            }
            else
            {
                Debug.Log("[AssetLoader] 没有找到该资源的 Handle Id：" + handleId);
            }

            return ahb.Result;
        }

        public IList<T> GetAssets<T>(int handleId)
        {
            AssetHandle<T> ahb = null;

            if (mAssetHandleDic.ContainsKey(handleId))
            {
                ahb = mAssetHandleDic[handleId] as AssetHandle<T>;
            }
            else
            {
                Debug.Log("[AssetLoader] 没有找到该资源的 Handle Id：" + handleId);
            }

            return ahb.Results;
        }

        #endregion

        #region 加载资源

        /// <summary>
        /// 同步加载资源，和Release方法配对使用
        /// </summary>
        public AssetHandle<T> LoadAsset<T>(object key)
        {
            AsyncOperationHandle<T> aoh = Addressables.LoadAssetAsync<T>(key);
            aoh.WaitForCompletion();
            mAssetHandleId++;
            AssetHandle<T> ah = new AssetHandle<T>(mAssetHandleId, key, this, aoh);
            return ah;
        }

        /// <summary>
        /// 同步加载资源，和Release方法配对使用
        /// </summary>
        public AssetHandle<T> LoadAssets<T>(object key, Action<T> completed)
        {
            AsyncOperationHandle<IList<T>> aoh = Addressables.LoadAssetsAsync(key, completed);
            aoh.WaitForCompletion();
            mAssetHandleId++;
            AssetHandle<T> ah = new AssetHandle<T>(mAssetHandleId, key, this, aoh);
            return ah;

        }

        /// <summary>
        /// 异步加载资源，和Release方法配对使用
        /// </summary>
        public AssetHandle<T> LoadAssetAsync<T>(object key, Action<AssetHandle<T>> completed)
        {
            AsyncOperationHandle<T> aoh = Addressables.LoadAssetAsync<T>(key);
            mAssetHandleId++;
            AssetHandle<T> ah = new AssetHandle<T>(mAssetHandleId, key, this, aoh);
            aoh.Completed += e =>
            {
                //Debug.Log("AssetOwner.LoadAsetAsync2 " + aoh.Status);     
                ah.Result = aoh.Result;
                completed?.Invoke(ah);
            };
            return ah;
        }

        /// <summary>
        /// 异步加载资源，和Release方法配对使用
        /// </summary>
        public AssetHandle<T> LoadAssetsAsync<T>(object key, Action<T> completed, Action<AssetHandle<T>> allCompleted)
        {
            //Debug.Log("AssetOwner.LoadAssetAsync " + key);
            AsyncOperationHandle<IList<T>> aoh = Addressables.LoadAssetsAsync<T>(key, completed);
            mAssetHandleId++;
            AssetHandle<T> ah = new AssetHandle<T>(mAssetHandleId, key, this, aoh);
            aoh.Completed += e =>
            {
                //Debug.Log("AssetOwner.LoadAsetAsync2 " + aoh.Status);     
                ah.Results = aoh.Result;
                allCompleted?.Invoke(ah);
            };
            return ah;
        }
        #endregion

        #region 实例化资源

        private AssetHandle<GameObject> InstantiateAsync(object key, AsyncOperationHandle<GameObject> aoh, Action<AssetHandle<GameObject>> completed)
        {
            mAssetHandleId++;
            AssetHandle<GameObject> ah = new AssetHandle<GameObject>(mAssetHandleId, key, this, aoh);
            aoh.Completed += e =>
            {
                //Debug.Log("AssetOwner.InstantiateAsync2 " + aoh.Status);   
                ah.Result = aoh.Result;
                completed?.Invoke(ah);
            };
            return ah;
        }

        private AssetHandle<GameObject> Instantiate(object key, AsyncOperationHandle<GameObject> aoh)
        {
            mAssetHandleId++;
            Debug.Log("资源加载 Id " + mAssetHandleId);
            AssetHandle<GameObject> ah = new AssetHandle<GameObject>(mAssetHandleId, key, this, aoh);
            return ah;
        }

        /// <summary>
        /// 异步实例化资源，和Release方法配对使用
        /// </summary>
        public AssetHandle<GameObject> InstantiateAsync(object key, Action<AssetHandle<GameObject>> completed)
        {
            AsyncOperationHandle<GameObject> aoh = Addressables.InstantiateAsync(key);
            return InstantiateAsync(key, aoh, completed);
        }

        /// <summary>
        /// 异步实例化资源，和Release方法配对使用
        /// </summary>
        public AssetHandle<GameObject> InstantiateAsync(object key, Vector3 position, Quaternion rotation, Action<AssetHandle<GameObject>> completed)
        {
            AsyncOperationHandle<GameObject> aoh = Addressables.InstantiateAsync(key, position, rotation);
            return InstantiateAsync(key, aoh, completed);
        }

        /// <summary>
        /// 异步实例化资源，和Release方法配对使用
        /// </summary>
        public AssetHandle<GameObject> InstantiateAsync(object key, Transform parent, Action<AssetHandle<GameObject>> completed)
        {
            AsyncOperationHandle<GameObject> aoh = Addressables.InstantiateAsync(key, parent);
            return InstantiateAsync(key, aoh, completed);
        }

        /// <summary>
        /// 异步实例化资源，和Release方法配对使用
        /// </summary>
        public AssetHandle<GameObject> InstantiateAsync(object key, Transform parent, Vector3 position, Quaternion rotation, Action<AssetHandle<GameObject>> completed)
        {
            AsyncOperationHandle<GameObject> aoh = Addressables.InstantiateAsync(key, position, rotation, parent);
            return InstantiateAsync(key, aoh, completed);
        }

        /// <summary>
        /// 同步实例化资源，和Release方法配对使用
        /// </summary>
        public AssetHandle<GameObject> Instantiate(object key)
        {
            AsyncOperationHandle<GameObject> aoh = Addressables.InstantiateAsync(key);
            aoh.WaitForCompletion();
            return Instantiate(key, aoh);
        }

        /// <summary>
        /// 同步实例化资源，和Release方法配对使用
        /// </summary>
        public AssetHandle<GameObject> Instantiate(object key, Vector3 position, Quaternion rotation)
        {
            AsyncOperationHandle<GameObject> aoh = Addressables.InstantiateAsync(key, position, rotation);
            aoh.WaitForCompletion();
            return Instantiate(key, aoh);
        }

        /// <summary>
        /// 同步实例化资源，和Release方法配对使用
        /// </summary>
        public AssetHandle<GameObject> Instantiate(object key, Transform parent)
        {
            AsyncOperationHandle<GameObject> aoh = Addressables.InstantiateAsync(key, parent);
            aoh.WaitForCompletion();
            return Instantiate(key, aoh);
        }

        /// <summary>
        /// 同步实例化资源，和Release方法配对使用
        /// </summary>
        public AssetHandle<GameObject> Instantiate(object key, Transform parent, Vector3 position, Quaternion rotation)
        {
            AsyncOperationHandle<GameObject> aoh = Addressables.InstantiateAsync(key, position, rotation, parent);
            aoh.WaitForCompletion();
            return Instantiate(key, aoh);
        }

        #endregion

        #region 图集加载图片

        public void LoadTexture()
        {
            // 1. 加载图集
            // 2. 从图集中加载图片
            // 3. 返回图片资源
        }

        #endregion

        #region 加载场景

        public void LoadScene()
        {
            // 0. 关闭输入检测
            // 1. 销毁上个场景的加载资源
            // 2. 加载场景（提供给外部加载进度的接口，用于加载界面的进度条显示）
            // 3. 加载场景结束后隐藏
            // 4. 加载下个场景的初始化资源
            // 5. 确保所有加载完毕后，再打开场景显示（防止玩家配置过低，加载太慢，看到一棵树突然冒出来）
        }

        #endregion

        #region 释放资源

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Release(int handleId)
        {
            if (mAssetHandleDic.TryGetValue(handleId, out var ahb))
            {
                ahb.Release();
            }
            else
            {
                Debug.Log("[AssetLoader] 没有找到该资源的 Handle Id：" + handleId);
            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Release<T>(AssetHandle<T> ah)
        {
            ah.Release();
        }

        /// <summary>
        /// 释放所有资源
        /// </summary>
        public void ReleaseAll()
        {
            foreach (var assetHandle in mAssetHandleDic.Values)
            {
                assetHandle.Release();
            }
            mAssetHandleDic.Clear();
        }


        #endregion
    }
}