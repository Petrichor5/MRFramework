using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MRFramework
{
    [MonoSingletonPath("MRFramework/InputMgr")]
    public class InputManager : MonoSingleton<InputManager>, IUnRegisterList
    {
        private Dictionary<string, InputInfo> mInputDic = new Dictionary<string, InputInfo>();
        public List<IUnRegister> UnRegisterList { get; } = new List<IUnRegister>();

        private UnityAction<InputInfo> mGetInputInfoHandle;
        
        private InputInfo nowInputInfo;
        private bool isOpenListener = false;
        private bool isCheckInput = false;

        #region 对外接口

        /// <summary>
        /// 设置是否开启输入监听
        /// </summary>
        public void SetInputListener(bool openOrClose)
        {
            isOpenListener = openOrClose;
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
            if (!mInputDic.ContainsKey(eventKey))
            {
                mInputDic.Add(eventKey, new InputInfo(inputType, key));
            }
            // 改建
            else
            {
                // 如果之前是鼠标 必须要修改它的按键类型
                mInputDic[eventKey].KeyType = EKeyType.Keyboard;
                mInputDic[eventKey].KeyCode = key;
                mInputDic[eventKey].InputType = inputType;
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
            if (!mInputDic.ContainsKey(eventKey))
            {
                mInputDic.Add(eventKey, new InputInfo(inputType, mouseId));
            }
            // 改建
            else
            {
                // 如果之前是键盘 必须要修改它的按键类型
                mInputDic[eventKey].KeyType = EKeyType.Mouse;
                mInputDic[eventKey].MouseId = mouseId;
                mInputDic[eventKey].InputType = inputType;
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
            if (mInputDic.ContainsKey(eventKey))
            {
                mInputDic.Remove(eventKey);
            }
        }

        /// <summary>
        /// 获取输入信息
        /// </summary>
        /// <param name="callback">按下任意键后执行回调</param>
        public void GetInputInfo(UnityAction<InputInfo> callback)
        {
            mGetInputInfoHandle = callback;
            StartCoroutine(BeginCheckInput());
        }

        #endregion

        #region 内部实现

        private void CheckKeyTrigger()
        {
            foreach (var eventKey in mInputDic.Keys)
            {
                nowInputInfo = mInputDic[eventKey];
                // 如果是键盘输入
                if (nowInputInfo.KeyType == EKeyType.Keyboard)
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
            switch (nowInputInfo.InputType)
            {
                case EInputType.Down:
                    if (Input.GetKeyDown(nowInputInfo.KeyCode))
                        EventManager.Instance.TriggerEventListener(eventKey);
                    break;
                case EInputType.Up:
                    if (Input.GetKeyUp(nowInputInfo.KeyCode))
                        EventManager.Instance.TriggerEventListener(eventKey);
                    break;
                case EInputType.Always:
                    if (Input.GetKey(nowInputInfo.KeyCode))
                        EventManager.Instance.TriggerEventListener(eventKey);
                    break;
            }
        }

        private void CheckMouse(string eventKey)
        {
            switch (nowInputInfo.InputType)
            {
                case EInputType.Down:
                    if (Input.GetMouseButtonDown(nowInputInfo.MouseId))
                        EventManager.Instance.TriggerEventListener(eventKey);
                    break;
                case EInputType.Up:
                    if (Input.GetMouseButtonUp(nowInputInfo.MouseId))
                        EventManager.Instance.TriggerEventListener(eventKey);
                    break;
                case EInputType.Always:
                    if (Input.GetMouseButton(nowInputInfo.MouseId))
                        EventManager.Instance.TriggerEventListener(eventKey);
                    break;
            }
        }

        private IEnumerator BeginCheckInput()
        {
            // 开启检测输入时可能伴随着键盘鼠标输入，要延迟一帧再进行检测
            yield return null;

            isCheckInput = true;
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
                    
                mGetInputInfoHandle.Invoke(inputInfo);
                mGetInputInfoHandle = null;
                isCheckInput = false;
            }
        }
        
        #endregion
        
        private void Update()
        {
            if (isOpenListener)
                CheckKeyTrigger();
            
            if (isCheckInput)
                CheckInput();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            this.UnRegisterAll();
        }
    }
}