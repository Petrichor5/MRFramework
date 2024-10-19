using System;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Config;

namespace MRFramework
{
    public class PanelBase : PanelBehaviour, IUnRegisterList, ICanGetController, ICanGetSystem, ICanGetUtility
    {
        private CanvasGroup m_UIMask;
        private CanvasGroup m_CanvasGroup;
        private Transform m_UIContent;

        private List<Button> m_AllButtonList; // 所有Button列表
        private List<Toggle> m_ToggleList; // 所有的Toggle列表
        private List<InputField> m_InputList; // 所有的输入框列表
        
        private List<long> m_TimerIDList; // 计时器

        [HideInInspector] public long DestroyTimerID; // 销毁面板计时器ID
        private bool m_DisableAnim = false; // 禁用动画

        private List<IUnRegister> m_AutoEventUnRegisterList;
        public List<IUnRegister> UnRegisterList => m_AutoEventUnRegisterList; // 事件自动销毁

        
        private List<SubPanel> m_SubPanleList; // 由该主面板管理的所有子面板列表
        private Dictionary<string, List<SubPanel>> m_SubPanleItemList; // 存储滚动列表子项的子面板

        private List<ReddotNode> m_ReddotNodeList; // 绑定事件的红点控件

        /// <summary>
        /// 初始化基类组件
        /// </summary>
        private void InitializeBaseComponent()
        {
            m_CanvasGroup = gameObject.GetComponent<CanvasGroup>();
            m_UIContent = transform.Find("UIContent").transform;

            var uiMask = transform.Find("UIMask");
            if (uiMask)
            {
                m_UIMask = uiMask.GetComponent<CanvasGroup>();
            }
        }

        public IArchitecture GetArchitecture()
        {
            return GameArchitecture.Instance;
        }

        #region 生命周期

        public override void OnFirstOpen()
        {
            base.OnFirstOpen();
            InitializeBaseComponent();
        }

        public override void OnOpen()
        {
            base.OnOpen();
            ShowAnimation();
        }

        public override void OnClose()
        {
            base.OnClose();
        }

        public override void OnDispose()
        {
            base.OnDispose();
            RemoveAllEventListener();
            RemoveAllButtonListener();
            RemoveAllToggleListener();
            RemoveAllInputListener();
            ClearReddotData();
        }

        #endregion

        #region UI事件

        public void AddButtonClickListener(Button btn, UnityAction action)
        {
            if (btn != null)
            {
                if (m_AllButtonList == null)
                {
                    m_AllButtonList = new List<Button>();
                }

                if (!m_AllButtonList.Contains(btn))
                {
                    m_AllButtonList.Add(btn);
                }

                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(action);
            }
        }

        public void AddToggleClickListener(Toggle toggle, UnityAction<bool, Toggle> action)
        {
            if (toggle != null)
            {
                if (m_ToggleList == null)
                {
                    m_ToggleList = new List<Toggle>();
                }

                if (!m_ToggleList.Contains(toggle))
                {
                    m_ToggleList.Add(toggle);
                }

                toggle.onValueChanged.RemoveAllListeners();
                toggle.onValueChanged.AddListener((isOn) => { action?.Invoke(isOn, toggle); });
            }
        }

        public void AddInputFieldListener(InputField input, UnityAction<string> onChangeAction,
            UnityAction<string> endAction)
        {
            if (input != null)
            {
                if (m_InputList == null)
                {
                    m_InputList = new List<InputField>();
                }

                if (!m_InputList.Contains(input))
                {
                    m_InputList.Add(input);
                }

                input.onValueChanged.RemoveAllListeners();
                input.onEndEdit.RemoveAllListeners();
                input.onValueChanged.AddListener(onChangeAction);
                input.onEndEdit.AddListener(endAction);
            }
        }

        public void RemoveAllButtonListener()
        {
            if (m_AllButtonList == null) return;

            foreach (var item in m_AllButtonList)
            {
                item.onClick.RemoveAllListeners();
            }

            m_AllButtonList.Clear();
        }

        public void RemoveAllToggleListener()
        {
            if (m_ToggleList == null) return;

            foreach (var item in m_ToggleList)
            {
                item.onValueChanged.RemoveAllListeners();
            }

            m_ToggleList.Clear();
        }

        public void RemoveAllInputListener()
        {
            if (m_InputList == null) return;

            foreach (var item in m_InputList)
            {
                item.onValueChanged.RemoveAllListeners();
                item.onEndEdit.RemoveAllListeners();
            }

            m_InputList.Clear();
        }

        #endregion

        #region 事件接口封装

        /************************** 类型事件 **************************/

        public void AddEventListener<T>(Action<T> callback)
        {
            EventManager.Instance.AddEventListener(callback)
                .AddToUnregisterList(this);
        }

        public void TriggerEventListener<T>(T callback)
        {
            EventManager.Instance.TriggerEventListener(callback);
        }

        public void TriggerEventListener<T>() where T : new()
        {
            EventManager.Instance.TriggerEventListener<T>();
        }

        /************************** 字符串事件 **************************/

        public void AddEventListener(string key, Action callback)
        {
            EventManager.Instance.AddEventListener(key, callback)
                .AddToUnregisterList(this);
        }

        public void TriggerEventListener(string key)
        {
            EventManager.Instance.TriggerEventListener(key);
        }

        /*********** T1 ***********/

        public void AddEventListener<T>(string key, Action<T> callback)
        {
            EventManager.Instance.AddEventListener(key, callback)
                .AddToUnregisterList(this);
        }

        public void TriggerEventListener<T>(string key, T data)
        {
            EventManager.Instance.TriggerEventListener(key, data);
        }

        /*********** T1 T2 ***********/


        /************************** End **************************/

        public void RemoveAllEventListener()
        {
            if (UnRegisterList == null) return;

            this.UnRegisterAll();
        }

        #endregion

        #region 计时器

        public long StartTimer(float duration, Action onTimerComplete = null, bool isLooping = false,
            float interval = 0f, Action<float> onInterval = null, bool calculateEscapedTime = true)
        {
            if (m_TimerIDList == null) m_TimerIDList = new List<long>();

            long timerID = TimerManager.Instance.AddTimer(duration, onTimerComplete, isLooping, interval, onInterval,
                calculateEscapedTime);
            m_TimerIDList.Add(timerID);
            return timerID;
        }

        public void RemoveTimer(int timerKey)
        {
            if (m_TimerIDList == null) return;

            TimerManager.Instance.RemoveTimer(timerKey);
            m_TimerIDList.Remove(timerKey);
        }

        public void RemoveAllTimer()
        {
            if (m_TimerIDList == null) return;

            foreach (var timerKey in m_TimerIDList)
            {
                TimerManager.Instance.RemoveTimer(timerKey);
            }

            m_TimerIDList.Clear();
        }

        #endregion

        #region 子面板

        public void AddSubPanle(SubPanel subPanel)
        {
            if (m_SubPanleList == null)
                m_SubPanleList = new List<SubPanel>();

            subPanel.OnInit();
            m_SubPanleList.Add(subPanel);
        }

        public void RemoveSubPanel(SubPanel subPanel)
        {
            if (m_SubPanleList == null) return;

            if (m_SubPanleList.Contains(subPanel))
            {
                subPanel.OnClear();
                m_SubPanleList.Remove(subPanel);
                
                Destroy(subPanel.gameObject);
            }
        }

        public void ClearAllSubPanle()
        {
            if (m_SubPanleList == null) return;

            for (int i = 0; i < m_SubPanleList.Count; i++)
            {
                m_SubPanleList[i].OnClear();
                Destroy(m_SubPanleList[i].gameObject);
            }
            m_SubPanleList.Clear();
        }

        #endregion

        #region 红点

        /// <summary>
        /// 清空所有红点刷新数据
        /// </summary>
        public void ResetAllReddotData()
        {
            if (m_ReddotNodeList == null) return;

            foreach (var item in m_ReddotNodeList)
            {
                item.ResetReddotData();
            }
        }

        /// <summary>
        /// 清空红点刷新数据
        /// </summary>
        public void ResetReddotData(ReddotNode item)
        {
            item.ResetReddotData();
        }

        /// <summary>
        /// 为红点控件绑定刷新事件
        /// </summary>
        public void SetReddotData(ReddotNode item, EReddot eReddot, string node)
        {
            if (m_ReddotNodeList == null) m_ReddotNodeList = new List<ReddotNode>();

            if (!m_ReddotNodeList.Contains(item))
            {
                m_ReddotNodeList.Add(item);
            }
            item.SetReddotData(eReddot, node);
        }

        private void ClearReddotData()
        {
            if (m_ReddotNodeList == null) return;

            foreach (var item in m_ReddotNodeList)
            {
                item.ResetReddotData();
            }
            m_ReddotNodeList.Clear();
        }

        #endregion

        #region 内部实现

        public void SetDisableAnim(bool disable)
        {
            m_DisableAnim = disable;
        }

        public void ShowAnimation()
        {
            if (!m_DisableAnim) return;

            //基础弹窗不需要动画
            if (Canvas.sortingOrder >= 100)
            {
                if (m_UIMask)
                {
                    //Mask动画
                    m_UIMask.alpha = 0;
                    m_UIMask.DOFade(1, 0.2f);
                }

                if (m_UIContent)
                {
                    //缩放动画
                    m_UIContent.localScale = Vector3.one * 0.8f;
                    m_UIContent.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack);
                }
            }
        }

        public void HideAnimation()
        {
            if (!m_DisableAnim)
            {
                UIManager.Instance.ClosePanel(Name);
                return;
            }

            if (m_UIContent && Canvas.sortingOrder >= 100)
            {
                m_UIContent.DOScale(Vector3.one * 1.1f, 0.2f).SetEase(Ease.OutBack).OnComplete(() =>
                {
                    UIManager.Instance.ClosePanel(Name);
                });
            }
        }

        public override void SetVisible(bool isVisble)
        {
            m_CanvasGroup.alpha = isVisble ? 1 : 0;
            m_CanvasGroup.blocksRaycasts = isVisble;
            Visible = isVisble;
        }

        public void SetMaskVisible(bool isVisble)
        {
            if (m_UIMask)
            {
                m_UIMask.alpha = isVisble ? 1 : 0;
            }
        }

        #endregion
    }
}