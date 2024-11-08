using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MRFramework
{
    [MonoSingletonPath("MRFramework/InputManager")]
    public class InputManager : MonoSingleton<InputManager>, IUnRegisterList
    {
        private Dictionary<string, InputInfo> m_InputDic = new Dictionary<string, InputInfo>();
        public List<IUnRegister> UnRegisterList { get; } = new List<IUnRegister>();

        private UnityAction<InputInfo> m_GetInputInfoHandle;
        
        private InputInfo m_NowInputInfo;
        private bool m_IsOpenListener = false;
        private bool m_IsCheckInput = false;

        private void OnDestroy()
        {
            this.UnRegisterAll();
        }

        #region 对外接口

        /// <summary>
        /// 设置是否开启输入监听
        /// </summary>
        public void SetInputListener(bool openOrClose)
        {
            m_IsOpenListener = openOrClose;
        }

        /// <summary>
        /// 设置键盘输入监听
        /// </summary>
        /// <param name="eventKey">按键事件</param>
        /// <param name="key">哪个按键</param>
        /// <param name="inputType">输入类型：按下 抬起 长按</param>
        public void SetKeyboardInput(string eventKey, KeyCode key, EInputType inputType)
        {
            // 初始化
            if (!m_InputDic.ContainsKey(eventKey))
            {
                m_InputDic.Add(eventKey, new InputInfo(inputType, key));
            }
            // 改建
            else
            {
                // 如果之前是鼠标 必须要修改它的按键类型
                m_InputDic[eventKey].KeyType = EKeyType.Keyboard;
                m_InputDic[eventKey].KeyCode = key;
                m_InputDic[eventKey].InputType = inputType;
            }
        }
        
        /// <summary>
        /// 设置鼠标输入监听
        /// </summary>
        /// <param name="eventKey">按键事件</param>
        /// <param name="mouseId">哪个按键</param>
        /// <param name="inputType">输入类型：按下 抬起 长按</param>
        public void SetMouseInput(string eventKey, int mouseId, EInputType inputType)
        {
            // 初始化
            if (!m_InputDic.ContainsKey(eventKey))
            {
                m_InputDic.Add(eventKey, new InputInfo(inputType, mouseId));
            }
            // 改建
            else
            {
                // 如果之前是键盘 必须要修改它的按键类型
                m_InputDic[eventKey].KeyType = EKeyType.Mouse;
                m_InputDic[eventKey].MouseId = mouseId;
                m_InputDic[eventKey].InputType = inputType;
            }
        }

        /// <summary>
        /// 监听按键输入
        /// </summary>
        /// <param name="eventKey">按键事件</param>
        /// <param name="callback">回调函数</param>
        public void ListenInput(string eventKey, Action callback)
        {
            EventManager.Instance.AddEventListener(eventKey, callback)
                .AddToUnregisterList(this);
        }

        /// <summary>
        /// 移除输入监听
        /// </summary>
        public void RemoveInpu(string eventKey)
        {
            if (m_InputDic.ContainsKey(eventKey))
            {
                m_InputDic.Remove(eventKey);
            }
        }

        /// <summary>
        /// 获取输入信息
        /// </summary>
        /// <param name="callback">按下任意键后执行回调</param>
        public void GetInputInfo(UnityAction<InputInfo> callback)
        {
            m_GetInputInfoHandle = callback;
            StartCoroutine(BeginCheckInput());
        }

        #endregion

        #region 内部实现

        private void CheckKeyTrigger()
        {
            foreach (var eventKey in m_InputDic.Keys)
            {
                m_NowInputInfo = m_InputDic[eventKey];
                // 如果是键盘输入
                if (m_NowInputInfo.KeyType == EKeyType.Keyboard)
                {
                    CheckKeyboard(eventKey);
                }
                // 如果是鼠标输入
                else
                {
                    CheckMouse(eventKey);
                }
            }
        }
        
        private void CheckKeyboard(string eventKey)
        {
            switch (m_NowInputInfo.InputType)
            {
                case EInputType.Down:
                    if (Input.GetKeyDown(m_NowInputInfo.KeyCode))
                        EventManager.Instance.TriggerEventListener(eventKey);
                    break;
                case EInputType.Up:
                    if (Input.GetKeyUp(m_NowInputInfo.KeyCode))
                        EventManager.Instance.TriggerEventListener(eventKey);
                    break;
                case EInputType.Always:
                    if (Input.GetKey(m_NowInputInfo.KeyCode))
                        EventManager.Instance.TriggerEventListener(eventKey);
                    break;
            }
        }

        private void CheckMouse(string eventKey)
        {
            switch (m_NowInputInfo.InputType)
            {
                case EInputType.Down:
                    if (Input.GetMouseButtonDown(m_NowInputInfo.MouseId))
                        EventManager.Instance.TriggerEventListener(eventKey);
                    break;
                case EInputType.Up:
                    if (Input.GetMouseButtonUp(m_NowInputInfo.MouseId))
                        EventManager.Instance.TriggerEventListener(eventKey);
                    break;
                case EInputType.Always:
                    if (Input.GetMouseButton(m_NowInputInfo.MouseId))
                        EventManager.Instance.TriggerEventListener(eventKey);
                    break;
            }
        }

        private IEnumerator BeginCheckInput()
        {
            // 开启检测输入时可能伴随着键盘鼠标输入，要延迟一帧再进行检测
            yield return null;

            m_IsCheckInput = true;
        }
        
        private void CheckInput()
        {
            // 当一个键按下时，遍历所有按键的信息，查找是哪个键按下的
            if (Input.anyKeyDown)
            {
                InputInfo inputInfo = null;
                Array keyCodes = Enum.GetValues(typeof(KeyCode));
                foreach (KeyCode inputKey in keyCodes)
                {
                    if (Input.GetKeyDown(inputKey))
                    {
                        inputInfo = new InputInfo(EInputType.Down, inputKey);
                        break;
                    }
                }

                for (int i = 0; i < 3; i++)
                {
                    if (Input.GetMouseButtonDown(i))
                    {
                        inputInfo = new InputInfo(EInputType.Down, i);
                        break;
                    }
                }
                    
                m_GetInputInfoHandle.Invoke(inputInfo);
                m_GetInputInfoHandle = null;
                m_IsCheckInput = false;
            }
        }
        
        #endregion
        
        private void Update()
        {
            if (m_IsOpenListener)
                CheckKeyTrigger();
            
            if (m_IsCheckInput)
                CheckInput();
        }
    }
}