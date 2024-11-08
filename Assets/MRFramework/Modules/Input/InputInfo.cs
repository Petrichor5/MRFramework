using UnityEngine;

namespace MRFramework
{
    /// <summary>
    /// 按键类型
    /// </summary>
    public enum EKeyType
    {
        /// <summary>
        /// 键盘输入
        /// </summary>
        Keyboard,
        /// <summary>
        /// 鼠标输入
        /// </summary>
        Mouse,
    }

    /// <summary>
    /// 输入的类型
    /// </summary>
    public enum EInputType
    {
        /// <summary>
        /// 按下
        /// </summary>
        Down,
        /// <summary>
        /// 抬起
        /// </summary>
        Up,
        /// <summary>
        /// 长按
        /// </summary>
        Always,
    }
    
    /// <summary>
    /// 输入信息
    /// </summary>
    public class InputInfo
    {
        public EKeyType KeyType;
        public EInputType InputType;
        public KeyCode KeyCode;
        public int MouseId;

        /// <summary>
        /// 键盘输入初始化
        /// </summary>
        /// <param name="inputType">输入的类型</param>
        /// <param name="key">哪个按键</param>
        public InputInfo(EInputType inputType, KeyCode key)
        {
            this.KeyType = EKeyType.Keyboard;
            this.InputType = inputType;
            this.KeyCode = key;
        }

        /// <summary>
        /// 鼠标输入初始化
        /// </summary>
        /// <param name="inputType">输入的类型</param>
        /// <param name="mouseID">哪个按键</param>
        public InputInfo(EInputType inputType, int mouseID)
        {
            this.KeyType = EKeyType.Mouse;
            this.InputType = inputType;
            this.MouseId = mouseID;
        }
    }
}