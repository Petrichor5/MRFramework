using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MRFramework
{
    public class UIBehaviour : UIBase, IUnRegisterList
    {
        // Button 按钮控件
        private List<Button> mButtons;
        private List<Button> mButtonList => mButtons ?? (mButtons = new List<Button>());

        // Toggle 勾选控件
        private List<Toggle> mToggles;
        private List<Toggle> mToggleList => mToggles ?? (mToggles = new List<Toggle>());

        // InputField 输入控件
        private List<InputField> mInputFields;
        private List<InputField> mInputFieldList => mInputFields ?? (mInputFields = new List<InputField>());

        // 事件
        List<IUnRegister> UnRegisters;
        public List<IUnRegister> UnRegisterList => UnRegisters ?? (UnRegisters = new List<IUnRegister>());

        // 计时器
        private List<int> mTimerKeys;
        private List<int> mTimerKeyList => mTimerKeys ?? (mTimerKeys = new List<int>());

        // 子面板
        private Dictionary<string, SubPanel> mSubPanels;
        private Dictionary<string, SubPanel> mAllSubPanelDic => mSubPanels ?? (mSubPanels = new Dictionary<string, SubPanel>());

        #region UI 控件事件管理

        public void AddButtonClickListener(Button btn, UnityAction action)
        {
            if (btn == null) return;
            if (!mButtonList.Contains(btn))
                mButtonList.Add(btn);
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(action);
        }

        public void AddToggleClickListener(Toggle toggle, UnityAction<bool, Toggle> action)
        {
            if (toggle == null) return;
            if (!mToggleList.Contains(toggle))
                mToggleList.Add(toggle);
            toggle.onValueChanged.RemoveAllListeners();
            toggle.onValueChanged.AddListener((isOn) => { action?.Invoke(isOn, toggle); });
        }

        public void AddInputListener(InputField input, UnityAction<string> onChangeAction,
            UnityAction<string> endAction)
        {
            if (input == null) return;
            if (!mInputFieldList.Contains(input))
                mInputFieldList.Add(input);
            input.onValueChanged.RemoveAllListeners();
            input.onEndEdit.RemoveAllListeners();
            input.onValueChanged.AddListener(onChangeAction);
            input.onEndEdit.AddListener(endAction);
        }

        public void RemoveAllButtonListener()
        {
            if (mButtonList == null) return;

            foreach (var item in mButtonList)
            {
                item.onClick.RemoveAllListeners();
            }
            mButtonList.Clear();
        }

        public void RemoveAllToggleListener()
        {
            if (mToggleList == null) return;

            foreach (var item in mToggleList)
            {
                item.onValueChanged.RemoveAllListeners();
            }
            mToggleList.Clear();
        }

        public void RemoveAllInputListener()
        {
            if (mInputFieldList == null) return;

            foreach (var item in mInputFieldList)
            {
                item.onValueChanged.RemoveAllListeners();
                item.onEndEdit.RemoveAllListeners();
            }
            mInputFieldList.Clear();
        }

        #endregion

        #region 子面板

        /// <summary>
        /// 初始化子面板
        /// </summary>
        public SubPanel AddSubPanel(SubPanel panel)
        {
            GameObject panelObj = panel.gameObject;
            string type = panelObj.name;
            SubPanel subPanel = panelObj.GetComponent(type) as SubPanel;
            if (subPanel)
            {
                mAllSubPanelDic.Add(type, subPanel);
                subPanel.InitSubPanel();
            }
            else
            {
                Debug.LogError($"{PanelName} AddSubPanel error! Type: {type}");
            }
            return subPanel;
        }

        public SubPanel TryGetSubPanel(string panelName)
        {
            return mAllSubPanelDic.TryGetValue(panelName, out var panel) ? panel : null;
        }

        public void DestroyAllSubView()
        {
            if (mAllSubPanelDic == null) return;

            foreach (var panel in mAllSubPanelDic.Values)
            {
                panel.Close();
                panel.OnDispose();
            }
            mAllSubPanelDic.Clear();
        }

        #endregion
        
        #region 原生封装

        public override void SetVisible(bool isVisible)
        {
            CanvasGroup.alpha = isVisible ? 1 : 0;
            CanvasGroup.blocksRaycasts = isVisible;
            IsVisible = isVisible;
        }

        #endregion
        
        #region 事件封装

        /************************** 类型事件 **************************/
        
        public void AddEventListener<T>(Action<T> callback)
        {
            EventManager.Instance.AddEventListener((T t) =>
            {
                if (IsVisible) callback?.Invoke(t);
            }).AddToUnregisterList(this);
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
            EventManager.Instance.AddEventListener(key, () =>
            {
                if (IsVisible) callback?.Invoke();
            }).AddToUnregisterList(this);
        }
        
        public void TriggerEventListener(string key)
        {
            EventManager.Instance.TriggerEventListener(key);
        }
        
        /*********** T1 ***********/
        
        public void AddEventListener<T>(string key, Action<T> callback)
        {
            EventManager.Instance.AddEventListener(key, (T t) =>
            {
                if (IsVisible) callback?.Invoke(t);
            }).AddToUnregisterList(this);
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

        #region 计时器封装

        public int StartTimer(float delaySeconds, Action callback, bool isLoop = false)
        {
            int timerKey = TimerManager.Instance.StartTimer(delaySeconds, callback, isLoop);
            mTimerKeyList.Add(timerKey);
            return timerKey;
        }

        public void RemoveTimer(int timerKey)
        {
            TimerManager.Instance.RemoveTimer(timerKey);
            mTimerKeyList.Remove(timerKey);
        }
        
        public void RemoveAllTimer()
        {
            if(mTimerKeyList == null) return;

            foreach (var timerKey in mTimerKeyList)
            {
                TimerManager.Instance.RemoveTimer(timerKey);
            }
            mTimerKeyList.Clear();
        }

        #endregion

        #region DoTween

        public void OpenAnim()
        {
            UIMask.alpha = 1;

            // 一级面板通常都是全屏面板，不执行打开动画
            if (SortingGroup.sortingOrder < 100)
            {
                SetVisible(true);
                return;
            }

            UIContent.localScale = Vector3.zero;
            SetVisible(true);
            UIContent.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack);
        }
        
        public void CloseAnim(Action callBack = null)
        {
            UIMask.alpha = 0;

            // 一级面板通常都是全屏面板，不执行关闭动画
            if (SortingGroup.sortingOrder < 100)
            {
                UIContent.localScale = Vector3.one;
                SetVisible(false);
                callBack?.Invoke();
                return;
            }

            UIContent.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack)
                .OnComplete(() =>
                {
                    SetVisible(false);
                    callBack?.Invoke();
                });
        }

        #endregion

        #region 内部实现

        public void SetMaskVisible(bool isVisible)
        {
            if (!UISetting.Instance.SINGMASK_SYSTEM)
            {
                return;
            }

            UIMask.alpha = isVisible ? 1 : 0;
        }
        
        private void OnDestroy()
        {
            RemoveAllButtonListener();
            RemoveAllToggleListener();
            RemoveAllInputListener();
            
            RemoveAllEventListener();
            RemoveAllTimer();

            DestroyAllSubView();
        }

        #endregion
    }
}