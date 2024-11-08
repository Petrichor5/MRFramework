using System;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Config;
using MRFramework.UGUIPro;

namespace MRFramework
{
    public class PanelBase : PanelBehaviour, ICanGetController, ICanGetSystem, ICanGetUtility
    {
        private CanvasGroup m_UIMask;
        private CanvasGroup m_CanvasGroup;
        private Transform m_UIContent;

        [HideInInspector] public long DestroyTimerID; // 销毁面板计时器ID
        private bool m_DisableAnim = false; // 禁用动画

        private List<Button> m_AllButtonList;
        private List<Toggle> m_ToggleList;
        private List<InputField> m_InputList;

        private TimerComponent m_TimerComponent;
        private EventComponent m_EventComponent;
        private SubPanleComponent m_SubPanleComponent;
        private ScrollComponent m_ScrollComponent;
        private ReddotComponent m_ReddotComponent;

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
            return MRF.Instance;
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

            if (m_SubPanleComponent != null)
                m_SubPanleComponent.OpenAllSubPanel();

            if (m_ScrollComponent != null)
                m_ScrollComponent.OpenAllItem();
        }

        public override void OnClose()
        {
            base.OnClose();
            HideAnimation();

            if (m_SubPanleComponent != null)
                m_SubPanleComponent.CloseAllSubPanel();

            if (m_ScrollComponent != null)
                m_ScrollComponent.CloseAllItem();
        }

        public override void OnDispose()
        {
            base.OnDispose();

            RemoveAllButtonListener();
            RemoveAllToggleListener();
            RemoveAllInputListener();

            if (m_EventComponent != null)
                m_EventComponent.Clear();

            if (m_TimerComponent != null)
                m_TimerComponent.Clear();

            if (m_SubPanleComponent != null)
                m_SubPanleComponent.Clear();

            if (m_ScrollComponent != null)
                m_ScrollComponent.Clear();

            if (m_ReddotComponent != null)
                m_ReddotComponent.Clear();
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

        private EventComponent GetEventComponent()
        {
            if (m_EventComponent == null)
            {
                m_EventComponent = new EventComponent();
            }

            return m_EventComponent;
        }

        public void AddEventListener<T>(Action<T> callback)
        {
            GetEventComponent().AddEventListener(callback);
        }

        public void TriggerEventListener<T>(T callback)
        {
            GetEventComponent().TriggerEventListener(callback);
        }

        public void TriggerEventListener<T>() where T : new()
        {
            GetEventComponent().TriggerEventListener<T>();
        }

        public void AddEventListener(string key, Action callback)
        {
            GetEventComponent().AddEventListener(key, callback);
        }

        public void TriggerEventListener(string key)
        {
            GetEventComponent().TriggerEventListener(key);
        }

        public void AddEventListener<T>(string key, Action<T> callback)
        {
            GetEventComponent().AddEventListener(key, callback);
        }

        public void TriggerEventListener<T>(string key, T data)
        {
            GetEventComponent().TriggerEventListener(key, data);
        }

        #endregion

        #region 计时器

        private TimerComponent GetTimerComponent()
        {
            if (m_TimerComponent == null)
                m_TimerComponent = new TimerComponent();
            return m_TimerComponent;
        }

        public long StartTimer(float duration, Action onTimerComplete = null, bool isLooping = false,
            float interval = 0f, Action<float> onInterval = null, bool calculateEscapedTime = true)
        {
            return GetTimerComponent().StartTimer(duration, onTimerComplete, isLooping, interval, onInterval, calculateEscapedTime);
        }

        public void RemoveTimer(int timerKey)
        {
            GetTimerComponent().RemoveTimer(timerKey);
        }

        #endregion

        #region 子面板

        private SubPanleComponent GetSubPanleComponent()
        {
            if (m_SubPanleComponent == null)
                m_SubPanleComponent = new SubPanleComponent();
            return m_SubPanleComponent;
        }

        public void AddSubPanle(SubPanel subPanel)
        {
            GetSubPanleComponent().AddSubPanle(subPanel);
        }

        public void RemoveSubPanel(SubPanel subPanel)
        {
            GetSubPanleComponent().RemoveSubPanel(subPanel);
        }

        #endregion

        #region 滚动列表

        private ScrollComponent GetScrollComponent()
        {
            if (m_ScrollComponent == null)
                m_ScrollComponent = new ScrollComponent();
            return m_ScrollComponent;
        }

        public void AddScrollWidget(ScrollBase scroll)
        {
            GetScrollComponent().AddScrollWidget(scroll);
        }

        #endregion

        #region 红点

        private ReddotComponent GetReddotComponent()
        {
            if (m_ReddotComponent == null)
                m_ReddotComponent = new ReddotComponent();
            return m_ReddotComponent;
        }

        /// <summary>
        /// 清空所有红点刷新数据
        /// </summary>
        public void ResetAllReddotData()
        {
            GetReddotComponent().ResetAllReddotData();
        }

        /// <summary>
        /// 清空红点刷新数据
        /// </summary>
        public void ResetReddotData(ReddotNode item)
        {
            GetReddotComponent().ResetReddotData(item);
        }

        /// <summary>
        /// 为红点控件绑定刷新事件
        /// </summary>
        public void SetReddotData(ReddotNode item, EReddot eReddot, string node)
        {
            GetReddotComponent().SetReddotData(item, eReddot, node);
        }

        #endregion

        #region 内部实现

        public void SetDisableAnim(bool disable)
        {
            m_DisableAnim = disable;
        }

        public virtual void ShowAnimation()
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

        public virtual void HideAnimation()
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