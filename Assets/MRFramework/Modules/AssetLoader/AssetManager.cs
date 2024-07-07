using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace MRFramework
{
    public class AssetManager : MonoSingleton<AssetManager>
    {
        private static List<AssetLoader> mAssetLoaderList = new List<AssetLoader>();

        public override void OnSingletonInit()
        {
            SafeObjectPool<AssetLoader>.Instance.Init(128, 5);
        }
        
        public void SetAssetLoaderPoolCount(int maxCacheCount)
        {
            SafeObjectPool<AssetLoader>.Instance.MaxCacheCount = maxCacheCount;
        }

        public static void AddLoader(AssetLoader loader)
        {
            mAssetLoaderList.Add(loader);
        }

        public static void RemoveLoader(AssetLoader loader)
        {
            mAssetLoaderList.Remove(loader);
        }

        public static void ClearLoader()
        {
            for (int i = 0; i < mAssetLoaderList.Count; i++)
            {
                AssetLoader loader = mAssetLoaderList[i];
                loader.ReleaseAll();
            }
            mAssetLoaderList.Clear();
        }
    }
}