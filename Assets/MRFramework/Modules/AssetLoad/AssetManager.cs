using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;

namespace MRFramework
{
    [MonoSingletonPath("MRFramework/AssetManager")]
    public class AssetManager : MonoSingleton<AssetManager>
    {
        private Dictionary<string, AssetHandle> m_AssetHandleDic; // 存储资源句柄

        public override void OnSingletonInit()
        {
            m_AssetHandleDic = new Dictionary<string, AssetHandle>();
        }

        private void OnDestroy()
        {
            ReleaseAllAsset();
        }

        #region 加载资源

        /// <summary>
        /// 同步加载资源，和 ReleaseAsset 配对使用
        /// </summary>
        public T LoadAsset<T>(string key)
        {
            AsyncOperationHandle<T> aoh;
            string keyName = GetKeyName<T>(key);

            if (m_AssetHandleDic.ContainsKey(keyName))
            {
                AssetHandle handle = m_AssetHandleDic[keyName];
                aoh = handle.AsyncOperationHandle.Convert<T>();
                handle.RefIncrease();
                return aoh.Result;
            }

            aoh = Addressables.LoadAssetAsync<T>(key);
            AssetHandle assetHandle = new AssetHandle(key, keyName, aoh);
            assetHandle.RefIncrease();
            m_AssetHandleDic.Add(keyName, assetHandle);
            aoh.WaitForCompletion();
            if (aoh.Status == AsyncOperationStatus.Succeeded)
            {
                if (aoh.Result != null)
                    return aoh.Result;
                else
                    OnLoadAssetFailed(assetHandle);
            }

            return aoh.Result;
        }

        /// <summary>
        /// 异步加载资源，和 ReleaseAsset 配对使用
        /// </summary>
        public void LoadAssetAsync<T>(string key, Action<T> completed)
        {
            AsyncOperationHandle<T> aoh;
            string keyName = GetKeyName<T>(key);

            if (m_AssetHandleDic.ContainsKey(keyName))
            {
                AssetHandle handle = m_AssetHandleDic[keyName];
                handle.RefIncrease();
                OnAsyncOperationCompleted(handle, completed);
                return;
            }

            aoh = Addressables.LoadAssetAsync<T>(key);
            AssetHandle assetHandle = new AssetHandle(key, keyName, aoh);
            assetHandle.RefIncrease();
            m_AssetHandleDic.Add(keyName, assetHandle);

            aoh.Completed += e =>
            {
                if (aoh.Status == AsyncOperationStatus.Succeeded && aoh.Result != null)
                    completed?.Invoke(aoh.Result);
                else
                    OnLoadAssetFailed(assetHandle);
            };
        }

        private void OnAsyncOperationCompleted<T>(AssetHandle handle, Action<T> completed)
        {
            AsyncOperationHandle<T> aoh = handle.AsyncOperationHandle.Convert<T>();

            // 资源还没异步加载完
            if (!aoh.IsDone)
            {
                aoh.Completed += e =>
                {
                    if (aoh.Status == AsyncOperationStatus.Succeeded && aoh.Result != null)
                        completed?.Invoke(aoh.Result);
                    else
                        OnLoadAssetFailed(handle);
                };
            }
            else
            {
                if (aoh.Status == AsyncOperationStatus.Succeeded && aoh.Result != null)
                    completed?.Invoke(aoh.Result);
                else
                    OnLoadAssetFailed(handle);
            }
        }

        private void OnLoadAssetFailed(AssetHandle handle)
        {
            handle.RefDecrease();
            if (m_AssetHandleDic.ContainsKey(handle.KeyName))
            {
                m_AssetHandleDic.Remove(handle.KeyName);
            }
            Log.Error($"[AssetManager] 资源加载失败！Key = {handle.ResKey}");
        }

        #endregion

        #region 加载实例化资源

        /// <summary>
        /// 同步加载实例化资源，和 ReleaseInstance 配对使用
        /// </summary>
        public GameObject Instantiate(string key, Transform parent = null)
        {
            AsyncOperationHandle<GameObject> aoh = Addressables.InstantiateAsync(key, parent);
            aoh.WaitForCompletion();
            if (aoh.Status == AsyncOperationStatus.Succeeded)
            {
                return aoh.Result;
            }
            Log.Error($"[AssetManager] 加载实例化资源失败，Key = {key}");
            return null;
        }

        /// <summary>
        /// 异步加载实例化资源，和 ReleaseInstance 配对使用
        /// </summary>
        public void InstantiateAsync(string key, Action<GameObject> completed, Transform parent = null)
        {
            AsyncOperationHandle<GameObject> aoh = Addressables.InstantiateAsync(key, parent);
            aoh.Completed += (aoh) =>
            {
                if (aoh.Status == AsyncOperationStatus.Succeeded)
                {
                    completed?.Invoke(aoh.Result);
                }
                else
                {
                    Log.Error($"[AssetManager] 加载实例化资源失败，Key = {key}");
                }
            };
        }

        #endregion

        #region 释放资源

        /// <summary>
        /// 释放资源, 引用计数为 0 时才会被真正释放
        /// </summary>
        public void ReleaseAsset<T>(string key)
        {
            if (key == null) return;

            string keyName = GetKeyName<T>(key);
            if (m_AssetHandleDic.ContainsKey(keyName))
            {
                AssetHandle assetHandle = m_AssetHandleDic[keyName];
                assetHandle.RefDecrease();

                if (assetHandle.Reference <= 0)
                {
                    AsyncOperationHandle<T> aoh = assetHandle.AsyncOperationHandle.Convert<T>();
                    Addressables.Release(aoh);
                    m_AssetHandleDic.Remove(keyName);
                }
            }
            else
            {
                Log.Info($"[AssetLoader] 没有找到该资源的 Key = {key}");
            }
        }

        /// <summary>
        /// 释放所有资源 (忽略引用计数, 如果还有资源被引用，该资源会直接丢失)
        /// </summary>
        public void ReleaseAllAsset()
        {
            foreach (var handle in m_AssetHandleDic.Values)
            {
                Addressables.Release(handle);
            }
            m_AssetHandleDic.Clear();

            AssetBundle.UnloadAllAssetBundles(true);
            Resources.UnloadUnusedAssets();
            GC.Collect();
        }

        /// <summary>
        /// 释放实例化资源
        /// </summary>
        public void ReleaseInstance(GameObject instance)
        {
            if (instance == null) return;

            Addressables.ReleaseInstance(instance);
        }

        #endregion

        #region 内部方法

        /// <summary>
        /// 构建资源名称
        /// </summary>
        private string GetKeyName<T>(string key)
        {
            // 由于存在同名，不同类型资源
            // 通过名字和类型拼接作为 key
            string keyName = key + "_" + typeof(T).Name;
            return keyName;
        }

        #endregion
    }
}