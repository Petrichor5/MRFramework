using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MRFramework
{
    [MonoSingletonPath("MRFramework/UIModule")]
    public class UIManager : Singleton<UIManager>
    {
        private UIManager() { }
        
        private Transform mUIRoot;
        private AssetLoader mAssetLoader;

        private Dictionary<string, AssetHandle<GameObject>> mMainPanelDic; // 存储所有面板
        private List<string> mLoadingPanel; // 正在加载中的面板
        private List<MainPanel> mVisiblePanelList; // 所有打开的面板
        private Dictionary<string, string> mPanelPathCache; // 缓存面板资源路径

        public override void OnSingletonInit()
        {
            GameObject uIRootObj = GameObject.Find("UIRoot");
            if (uIRootObj == null)
            {
                uIRootObj = GameObject.Instantiate(Resources.Load<GameObject>("UIRoot"));
                uIRootObj.name = "UIRoot";
            }

            mUIRoot = uIRootObj.transform;
            GameObject.DontDestroyOnLoad(mUIRoot);

            mAssetLoader = AssetLoader.Allocate();

            mMainPanelDic = new Dictionary<string, AssetHandle<GameObject>>();
            mLoadingPanel = new List<string>();
            mVisiblePanelList = new List<MainPanel>();
            mPanelPathCache = new Dictionary<string, string>();
        }

        #region 对外接口

        /// <summary>
        /// 打开面板
        /// </summary>
        public void OpenPanel<T>(UnityAction<T> callback = null) where T : MainPanel
        {
            var type = typeof(T);
            string panelName = type.Name;

            // 面板还在加载中，直接退出，避免重复加载
            if (mLoadingPanel.Contains(panelName))
                return;
            
            // 已经加载出来了
            if (mMainPanelDic.ContainsKey(panelName))
            {
                MainPanel panel = mMainPanelDic[panelName].Result.GetComponent<T>();
                if (!panel.IsVisible)
                {
                    TimerManager.Instance.RemoveTimer(panel.DiposeTimerKey);
                    panel.transform.SetAsLastSibling();
                    panel.OnOpen();
                    panel.OpenAnim();
                    mVisiblePanelList.Add(panel);
                    SetWindowMaskVisible();
                    Debug.Log("[UIManager] => 打开面板 Name: " + panelName);
                }
                else
                {
                    Debug.LogWarning("[UIManager] => 不能重复打开主面板 Name: " + panelName);
                }
            }
            else
            {
                string resKey = GetPanleResKeyByName(panelName);
                if (resKey != null)
                {
                    mLoadingPanel.Add(panelName);
                    mAssetLoader.InstantiateAsync(resKey, mUIRoot, (handle) =>
                    {
                        GameObject panelObj = handle.Result;
                        panelObj.name = panelName;

                        MainPanel mainPanel = panelObj.GetComponent<T>();
                        mainPanel.PanelName = panelName;
                        mainPanel.transform.SetAsLastSibling();
                        mainPanel.OnAwake();
                        mainPanel.OnOpen();
                        mainPanel.OpenAnim();

                        mVisiblePanelList.Add(mainPanel);
                        mMainPanelDic.Add(panelName, handle);
                        mLoadingPanel.Remove(panelName);

                        SetWindowMaskVisible();

                        Debug.Log("[UIManager] => 打开面板 Name: " + panelName);
                    });
                }
            }
        }

        /// <summary>
        /// 关闭面板
        /// </summary>
        public void ClosePanel<T>() where T : MainPanel
        {
            var type = typeof(T);
            string panelName = type.Name;

            if (mMainPanelDic.TryGetValue(panelName, out var handle))
            {
                var panel = handle.Result.GetComponent<T>();
                
                if (!panel.IsVisible)
                {
                    Debug.LogWarning("[UIManager] => 尝试关闭已关闭的面板 Name: " + panelName);
                    return;
                }

                SetWindowMaskVisible();
                mVisiblePanelList.Remove(panel);
                panel.CloseAnim();
                panel.OnClose();
                panel.RemoveAllTimer();
                panel.DiposeTimerKey = TimerManager.Instance.StartTimer(UIDefine.PanelDiposeTime,
                    () => { DisposePanel<T>(); });
                Debug.Log("[UIManager] => 关闭面板 Name: " + panelName);
            }
            else
            {
                Debug.LogError("[UIManager] => 关闭面板失败，没有找到主面板 Name: " + panelName);
            }
        }

        /// <summary>
        /// 销毁面板
        /// </summary>
        public void DisposePanel<T>() where T : MainPanel
        {
            var type = typeof(T);
            string panelName = type.Name;
            if (mMainPanelDic.TryGetValue(panelName, out var handle))
            {
                var panel = handle.Result.GetComponent<T>();
                TimerManager.Instance.RemoveTimer(panel.DiposeTimerKey);
                panel.OnDispose();
                mMainPanelDic.Remove(panelName);
                mAssetLoader.Release(handle);
                SetWindowMaskVisible();
                Debug.Log("[UIManager] => 销毁面板 Name: " + panelName);
            }
            else
            {
                Debug.Log("[UIManager] => 销毁面板失败，没有找到改面板 Name: " + panelName);
            }
        }

        public T TryGetPanel<T>() where T : MainPanel
        {
            var type = typeof(T);
            string panelName = type.Name;

            if (mMainPanelDic.TryGetValue(panelName, out var handle))
            {
                var panel = handle.Result.GetComponent<T>();
                if (panel != null)
                {
                    return panel.As<T>();
                }
            }

            return null;
        }

        public void ClearAllPanel()
        {
            foreach (var handle in mMainPanelDic.Values)
            {
                mAssetLoader.Release(handle);
            }
            mMainPanelDic.Clear();
            mLoadingPanel.Clear();
            mVisiblePanelList.Clear();
        }

        #endregion

        #region 内部实现

        /// <summary>
        /// 通过面板名称找到资源路径
        /// </summary>
        private string GetPanleResKeyByName(string panelName)
        {
            if (string.IsNullOrEmpty(panelName))
            {
                Debug.LogError("[UIManager] => 面板名称为空");
                return null;
            }

            // 使用缓存来减少重复计算
            if (mPanelPathCache.TryGetValue(panelName, out string cachedPath))
            {
                return cachedPath;
            }

            // 检查面板名称是否以 "WBP" 开头
            if (!panelName.StartsWith("WBP"))
            {
                Debug.LogError("[UIManager] => 主面板名称错误：" + panelName);
                return null;
            }

            // 构建资源路径
            string[] strs = StringUtil.SplitStr(panelName, 7);
            string resKey = "Assets/AssetPackage/UI";
            for (int i = 1; i < strs.Length - 1; i++)
            {
                resKey = resKey + "/" + strs[i];
            }
            resKey = resKey + "/" + panelName + ".prefab";

            // 缓存结果
            mPanelPathCache[panelName] = resKey;

            return resKey;
        }

        /// <summary>
        /// 面板遮罩设置
        /// </summary>
        private void SetWindowMaskVisible()
        {
            if (!UISetting.Instance.SINGMASK_SYSTEM) return;

            MainPanel maxOrderPanelBase = null; // 最大渲染层级的面板
            int maxOrder = 0; // 最大渲染层级
            int maxIndex = 0; // 最大排序下标 在相同父节点下的位置下标

            for (int i = 0; i < mVisiblePanelList.Count; i++)
            {
                MainPanel panel = mVisiblePanelList[i];
                if (panel != null && panel.gameObject != null)
                {
                    // 关闭所有面板的 Mask，设置为不可见
                    panel.SetMaskVisible(false);
                    // 从所有可见面板中找到一个层级最大的面板
                    if (maxOrderPanelBase == null)
                    {
                        maxOrderPanelBase = panel;
                        maxOrder = panel.SortingGroup.sortingOrder;
                        maxIndex = panel.transform.GetSiblingIndex();
                    }
                    else
                    {
                        // 找到最大渲染层级的面板
                        if (maxOrder < panel.SortingGroup.sortingOrder)
                        {
                            maxOrderPanelBase = panel;
                            maxOrder = panel.SortingGroup.sortingOrder;
                        }
                        // 如果两个面板的渲染层级相同，就找到同节点下最靠下一个物体，优先渲染 Mask
                        else if (maxOrder == panel.SortingGroup.sortingOrder &&
                                 maxIndex < panel.transform.GetSiblingIndex())
                        {
                            maxOrderPanelBase = panel;
                            maxIndex = panel.transform.GetSiblingIndex();
                        }
                    }
                }
            }

            // 把找到渲染层级最大的面板 Mask 设为可见
            if (maxOrderPanelBase != null)
            {
                maxOrderPanelBase.SetMaskVisible(true);
            }
        }

        #endregion
    }
}