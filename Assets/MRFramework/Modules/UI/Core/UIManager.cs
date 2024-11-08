using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

namespace MRFramework
{
    [MonoSingletonPath("MRFramework/UIManager")]
    public class UIManager : MonoSingleton<UIManager>
    {
        private Camera m_UICamera;
        private Transform m_UIRoot;

        private UIAssetManager m_UIAssetManager;

        private Dictionary<string, PanelBase> m_AllPanelDic;      //所有面板的Dic
        private List<PanelBase> m_AllPanelList;                   //所有面板的列表
        private List<PanelBase> m_VisiblePanelList;               //所有可见面板的列表

        private Queue<PanelBase> m_PanelStack;                    // 面板队列，用来管理面板的循环弹出
        private bool m_StartPopStackStatus = false;               // 开始弹出堆栈的标识，可以用来处理多种情况，比如：正在出栈中有其它界面弹出，可以直接放到栈内进行弹出 等

        public override void OnSingletonInit()
        {
            GameObject uIRootObj = FindOrCreateUIComponent("UIRoot");
            GameObject uiCamera = FindOrCreateUIComponent("UICamera");
            FindOrCreateUIComponent("EventSystem");

            m_UIRoot = uIRootObj.transform;
            m_UICamera = uiCamera.GetComponent<Camera>();

            m_UIAssetManager = new UIAssetManager();
            m_UIAssetManager.Init(m_UIRoot);
            
            m_AllPanelDic = new Dictionary<string, PanelBase>();
            m_AllPanelList = new List<PanelBase>();
            m_VisiblePanelList = new List<PanelBase>();
            m_PanelStack = new Queue<PanelBase>();
        }

        private void OnDestroy()
        {
            DestroyAllPanel();
            m_UIAssetManager.OnDispose();
        }

        private GameObject FindOrCreateUIComponent(string cmpName)
        {
            GameObject go = GameObject.Find(cmpName);
            if (!go)
            {
                go = GameObject.Instantiate(Resources.Load<GameObject>(cmpName));
                go.name = cmpName;
            }

            GameObject.DontDestroyOnLoad(go);
            return go;
        }

        #region 对外接口

        /// <summary>
        /// 预加载面板：只加载 GameObject，不调用生命周期
        /// </summary>
        public void PreLoadPanel<T>() where T : PanelBase
        {
            Type type = typeof(T);
            string panelName = type.Name;

            // 加载对应的面板预制体
            m_UIAssetManager.LoadPanelAsync(panelName, (GameObject panel) =>
            {
                // 初始化对应管理类
                if (panel != null)
                {
                    T panelBase = panel.GetComponent<T>();
                    
                    // 初始化设置
                    panelBase.Canvas = panel.GetComponent<Canvas>();
                    panelBase.Canvas.worldCamera = m_UICamera;
                    panelBase.Name = panel.name;

                    RectTransform rectTrans = panel.GetComponent<RectTransform>();
                    rectTrans.anchorMax = Vector2.one;
                    rectTrans.offsetMax = Vector2.zero;
                    rectTrans.offsetMin = Vector2.zero;

                    // 面板生命周期
                    panelBase.OnFirstOpen();
                    panelBase.SetVisible(false);

                    m_AllPanelDic.Add(panelName, panelBase);
                    m_AllPanelList.Add(panelBase);

                    Log.Info("[UIManager] 预加载面板 PanelName: " + panelName);
                }
                else
                {
                    Log.Error("[UIManager] 预加载面板失败 PanelName: " + panelName);
                }
            });
        }

        /// <summary>
        /// 打开面板
        /// </summary>
        public void OpenPanel<T>(Action<T> callback = null) where T : PanelBase
        {
            Type type = typeof(T);
            string panelName = type.Name;
            PanelBase wnd = GetPanel(panelName);
            if (wnd != null)
            {
                Log.Info("[UIManager] 打开面板 PanelName: " + panelName);
                wnd = ShowPanel(panelName);
                callback?.Invoke(wnd as T);
                return;
            }
            
            InitializePanel(panelName, (PanelBase panelBase) =>
            {
                Log.Info("[UIManager] 打开面板 PanelName: " + panelName);
                callback?.Invoke(panelBase as T);
            });
        }
        
        /// <summary>
        /// 关闭面板
        /// </summary>
        public void ClosePanel(string panelName)
        {
            PanelBase panel = GetPanel(panelName);
            HidePanel(panel);
            Log.Info("[UIManager] 关闭面板 PanelName: " + panelName);
        }

        /// <summary>
        /// 关闭面板
        /// </summary>
        public void ClosePanel<T>() where T : PanelBase
        {
            ClosePanel(typeof(T).Name);
        }
        
        /// <summary>
        /// 销毁面板
        /// </summary>
        public void DestroyPanel(string panelName)
        {
            PanelBase panel = GetPanel(panelName);
            DestoryPanel(panel);
        }

        /// <summary>
        /// 销毁面板
        /// </summary>
        public void DestroyWinodw<T>() where T : PanelBase
        {
            DestroyPanel(typeof(T).Name);
        }
        
        /// <summary>
        /// 销毁所有面板
        /// </summary>
        /// <param name="filterlist">过滤面板列表：过滤掉不销毁的面板</param>
        public void DestroyAllPanel(List<string> filterlist = null)
        {
            for (int i = 0; i < m_AllPanelList.Count; i++)
            {
                var panel = m_AllPanelList[i];
                if (panel == null || (filterlist != null && filterlist.Contains(panel.Name)))
                {
                    continue;
                }
                DestroyPanel(panel.Name);
            }
        }
        
        /// <summary>
        /// 获取已经弹出的面板
        /// </summary>
        public T GetPanel<T>() where T : PanelBase
        {
            Type type = typeof(T);
            foreach (var item in m_VisiblePanelList)
            {
                if (item.Name == type.Name)
                {
                    return (T)item;
                }
            }

            Log.Error("[UIManager] 获取面板失败 PanelName: " + type.Name);
            return null;
        }

        #endregion

        #region 子面板

        /// <summary>
        /// 加载子面板
        /// </summary>
        public void GetSubPanel()
        {
            
        }

        /// <summary>
        /// 从对象池中加载子面板
        /// </summary>
        public void GetSubPanelFormPool()
        {
            
        }
        
        /// <summary>
        /// 把子面板回收进对象池
        /// </summary>
        public void ReturnSubPanelToPool()
        {
            
        }

        #endregion

        #region 内部实现

        private void PopUpPanel(PanelBase panel, Action<PanelBase> callback)
        {
            Type type = panel.GetType();
            string panelName = type.Name;
            PanelBase wnd = GetPanel(panelName);
            if (wnd != null)
            {
                wnd = ShowPanel(panelName);
                callback?.Invoke(wnd);
                return;
            }

            InitializePanel(panelName, callback);
        }

        private void InitializePanel(string panelName, Action<PanelBase> callback)
        {
            // 加载对应的面板预制体
            m_UIAssetManager.LoadPanelAsync(panelName, (GameObject panel) =>
            {
                // 初始化对应管理类
                if (panel != null)
                {
                    PanelBase panelBase = panel.GetComponent<PanelBase>();
                    
                    // 初始化设置
                    panelBase.Canvas = panel.GetComponent<Canvas>();
                    panelBase.Canvas.worldCamera = m_UICamera;
                    panelBase.transform.SetAsLastSibling();
                    panelBase.Name = panel.name;

                    RectTransform rectTrans = panel.GetComponent<RectTransform>();
                    rectTrans.anchorMax = Vector2.one;
                    rectTrans.offsetMax = Vector2.zero;
                    rectTrans.offsetMin = Vector2.zero;

                    // 面板生命周期
                    panelBase.OnFirstOpen();
                    panelBase.SetVisible(true);
                    panelBase.OnOpen();

                    m_AllPanelDic.Add(panelName, panelBase);
                    m_AllPanelList.Add(panelBase);
                    m_VisiblePanelList.Add(panelBase);

                    SetWidnowMaskVisible();

                    callback?.Invoke(panelBase);
                }
                else
                {
                    Log.Error("[UIManager] 加载面板失败 PanelName: " + panelName);
                }
            });
        }

        private PanelBase ShowPanel(string panelName)
        {
            if (m_AllPanelDic.TryGetValue(panelName, out PanelBase panel))
            {
                // 移除计时销毁面板
                TimerManager.Instance.RemoveTimer(panel.DestroyTimerID);
                
                if (panel.gameObject != null && panel.Visible == false)
                {
                    m_VisiblePanelList.Add(panel);
                    panel.transform.SetAsLastSibling();
                    panel.SetVisible(true);
                    SetWidnowMaskVisible();
                    panel.OnOpen();
                }

                return panel;
            }
            else
            {
                Log.Error("[UIManager] " +  panelName + " 面板不存在，请调用 OpenPanel 打开面板");
            }

            return null;
        }

        private void HidePanel(PanelBase panel)
        {
            if (panel != null && panel.Visible)
            {
                m_VisiblePanelList.Remove(panel);
                panel.SetVisible(false);
                SetWidnowMaskVisible();
                panel.OnClose();
                
                // 计时销毁面板
                panel.DestroyTimerID = TimerManager.Instance.AddTimer(UIDefine.PanelDiposeTime, () => { 
                    DestoryPanel(panel);
                });
            }

            // 在出栈的情况下，上一个界面隐藏时，自动打开栈中的下一个界面
            PopNextStackPanel(panel);
        }

        private void DestoryPanel(PanelBase panel)
        {
            if (panel != null)
            {
                Log.Info("[UIManager] 销毁面板 PanelName: " + panel.Name);
                
                // 移除计时销毁面板
                TimerManager.Instance.RemoveTimer(panel.DestroyTimerID);
                
                if (m_AllPanelDic.ContainsKey(panel.Name))
                {
                    m_AllPanelDic.Remove(panel.Name);
                    m_AllPanelList.Remove(panel);
                    m_VisiblePanelList.Remove(panel);
                }

                panel.SetVisible(false);
                SetWidnowMaskVisible();
                panel.OnClose();
                panel.OnDispose();
                
                m_UIAssetManager.DestroyPanel(panel.Name);

                // 在出栈的情况下，上一个界面隐藏时，自动打开栈中的下一个界面
                PopNextStackPanel(panel);
            }
        }
        
        private PanelBase GetPanel(string winName)
        {
            if (m_AllPanelDic.ContainsKey(winName))
            {
                return m_AllPanelDic[winName];
            }

            return null;
        }

        private void SetWidnowMaskVisible()
        {
            PanelBase maxOrderWndBase = null; // 最大渲染层级的面板
            int maxOrder = 0; // 最大渲染层级
            int maxIndex = 0; // 最大排序下标，在相同父节点下的位置下标

            // 1.关闭所有面板的Mask 设置为不可见
            // 2.从所有可见面板中找到一个层级最大的面板，把Mask设置为可见
            for (int i = 0; i < m_VisiblePanelList.Count; i++)
            {
                PanelBase panel = m_VisiblePanelList[i];
                if (panel != null && panel.gameObject != null)
                {
                    panel.SetMaskVisible(false);
                    if (maxOrderWndBase == null)
                    {
                        maxOrderWndBase = panel;
                        maxOrder = panel.Canvas.sortingOrder;
                        maxIndex = panel.transform.GetSiblingIndex();
                    }
                    else
                    {
                        // 找到最大渲染层级的面板
                        if (maxOrder < panel.Canvas.sortingOrder)
                        {
                            maxOrderWndBase = panel;
                            maxOrder = panel.Canvas.sortingOrder;
                        }
                        // 如果两个面板的渲染层级相同，就找到同节点下最靠下一个面板，优先渲染Mask
                        else if (maxOrder == panel.Canvas.sortingOrder &&
                                 maxIndex < panel.transform.GetSiblingIndex())
                        {
                            maxOrderWndBase = panel;
                            maxIndex = panel.transform.GetSiblingIndex();
                        }
                    }
                }
            }

            if (maxOrderWndBase != null)
            {
                maxOrderWndBase.SetMaskVisible(true);
            }
        }

        #endregion

        #region 堆栈系统

        /// <summary>
        /// 进栈一个面板
        /// </summary>
        public void PushPanelToStack<T>(Action<PanelBase> popCallBack = null) where T : PanelBase, new()
        {
            T wndBase = new T();
            wndBase.PopStackListener = popCallBack;
            m_PanelStack.Enqueue(wndBase);
        }

        /// <summary>
        /// 弹出堆栈中第一个面板
        /// </summary>
        public void StartPopFirstStackPanel()
        {
            if (m_StartPopStackStatus) return;
            m_StartPopStackStatus = true; //已经开始进行堆栈弹出的流程，
            PopStackPanel();
        }

        /// <summary>
        /// 压入并且弹出堆栈面板
        /// </summary>
        public void PushAndPopStackPanel<T>(Action<PanelBase> popCallBack = null) where T : PanelBase, new()
        {
            PushPanelToStack<T>(popCallBack);
            StartPopFirstStackPanel();
        }

        /// <summary>
        /// 弹出堆栈中的下一个面板
        /// </summary>
        private void PopNextStackPanel(PanelBase panelBase)
        {
            if (panelBase != null && m_StartPopStackStatus && panelBase.PopStack)
            {
                panelBase.PopStack = false;
                PopStackPanel();
            }
        }

        /// <summary>
        /// 弹出堆栈面板
        /// </summary>
        /// <returns></returns>
        private void PopStackPanel()
        {
            if (m_PanelStack.Count > 0)
            {
                PanelBase panel = m_PanelStack.Dequeue();
                PopUpPanel(panel, (PanelBase popPanel) =>
                {
                    popPanel.PopStackListener = panel.PopStackListener;
                    popPanel.PopStack = true;
                    popPanel.PopStackListener?.Invoke(popPanel);
                    popPanel.PopStackListener = null;
                });
            }
            else
            {
                m_StartPopStackStatus = false;
            }
        }

        /// <summary>
        /// 清空面板堆栈
        /// </summary>
        public void ClearStackPanels()
        {
            m_PanelStack.Clear();
        }

        #endregion
    }
}