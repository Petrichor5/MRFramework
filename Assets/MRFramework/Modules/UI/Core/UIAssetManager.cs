using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace MRFramework
{
    /// <summary>
    /// UI资源加载管理器
    /// </summary>
    public class UIAssetManager
    {
        private Transform m_UIRoot;

        private Dictionary<object, AsyncOperationHandle<GameObject>> m_AssetHandleDic; // UI加载资源句柄字典
        private Dictionary<string, string> m_PanelPathCache; // 缓存面板资源路径，减少字符串运算

        public void Init(Transform uiRoot)
        {
            m_UIRoot = uiRoot;

            m_AssetHandleDic = new Dictionary<object, AsyncOperationHandle<GameObject>>();
            m_PanelPathCache = new Dictionary<string, string>();
        }

        public void OnDispose()
        {
            // 释放所有加载的面板
            foreach (var aoh in m_AssetHandleDic.Values)
            {
                Addressables.ReleaseInstance(aoh);
            }
            m_AssetHandleDic.Clear();

            // 释放缓存
            m_PanelPathCache.Clear();

            // 清理 UI 根节点引用
            m_UIRoot = null;
        }

        /// <summary>
        /// 加载主面板预制体
        /// </summary>
        public void LoadPanelAsync(string panelName, Action<GameObject> onCompleted)
        {
            if (m_AssetHandleDic.ContainsKey(panelName)) return;
            InstantiateAsync(panelName, onCompleted);
        }

        /// <summary>
        /// 销毁主面板预制体
        /// </summary>
        public void DestroyPanel(string panelName)
        {
            if (m_AssetHandleDic.ContainsKey(panelName))
            {
                var aoh = m_AssetHandleDic[panelName];
                Addressables.ReleaseInstance(aoh);
                m_AssetHandleDic.Remove(panelName);
                m_PanelPathCache.Remove(panelName);
            }
            else
            {
                Log.Error("销毁面板预制体失败，没有找到该面板 PanelName: " + panelName);
            }
        }

        private void InstantiateAsync(string panelName, Action<GameObject> onCompleted)
        {
            string key = GetPanleResKeyByName(panelName);
            AsyncOperationHandle<GameObject> aoh = Addressables.InstantiateAsync(key, m_UIRoot);
            m_AssetHandleDic.Add(panelName, aoh);
            aoh.Completed += e =>
            {
                if (aoh.Status == AsyncOperationStatus.Succeeded)
                {
                    GameObject panel = aoh.Result;
                    panel.transform.localScale = Vector3.one;
                    panel.transform.localPosition = Vector3.zero;
                    panel.transform.rotation = Quaternion.identity;
                    panel.name = panelName;
                    onCompleted?.Invoke(panel);
                }
                else
                {
                    Log.Error("加载面板预制体失败 PanelName: " + panelName);
                }
            };
        }

        private void InstantiateSubPanelAsync(string panelName, Action<GameObject> onCompleted)
        {
            string key = GetPanleResKeyByName(panelName);
            AsyncOperationHandle<GameObject> aoh = Addressables.InstantiateAsync(key);

            aoh.Completed += e =>
            {
                if (aoh.Status == AsyncOperationStatus.Succeeded)
                {
                    GameObject panel = aoh.Result;
                    panel.transform.localScale = Vector3.one;
                    panel.transform.localPosition = Vector3.zero;
                    panel.transform.rotation = Quaternion.identity;
                    panel.name = panelName;
                    onCompleted?.Invoke(panel);
                }
                else
                {
                    Log.Error("加载子面板预制体失败 PanelName: " + panelName);
                }
            };
        }

        /// <summary>
        /// 通过面板名称找到资源路径
        /// </summary>
        private string GetPanleResKeyByName(string panelName)
        {
            if (string.IsNullOrEmpty(panelName))
            {
                Log.Error("[UIManager] => 面板名称为空");
                return null;
            }

            // 使用缓存来减少重复计算
            if (m_PanelPathCache.TryGetValue(panelName, out string cachedPath))
            {
                return cachedPath;
            }

            // 检查面板名称是否以 "WBP" 开头
            if (!panelName.StartsWith("WBP"))
            {
                Log.Error("[UIManager] => 主面板名称错误：" + panelName);
                return null;
            }

            // 构建资源路径
            string[] strs = StringUtil.SplitStr(panelName, 7);
            // string resKey = "Assets/AssetPackage/UI";
            string path = "";
            for (int i = 1; i < strs.Length - 1; i++)
            {
                path = path + strs[i] + "/";
            }
            string resKey = Path.Combine(path, panelName + ".prefab");

            // 缓存结果
            m_PanelPathCache[panelName] = resKey;

            return resKey;
        }
    }
}