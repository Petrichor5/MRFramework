using System;
using UnityEngine;

namespace MRFramework
{
    public abstract class PanelBehaviour : MonoBehaviour
    {
        /// <summary>
        /// 当前面板 Canvas
        /// </summary>
        public Canvas Canvas { get; set; }
        
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// 可见性
        /// </summary>
        public bool Visible { get; set; }
        
        /// <summary>
        /// 是否是通过堆栈系统弹出的弹窗
        /// </summary>
        public bool PopStack { get; set; }
        
        /// <summary>
        /// 弹出堆栈回调
        /// </summary>
        public Action<PanelBase> PopStackListener { get; set; }
        
        /// <summary>
        /// 只会在创建时执行一次
        /// </summary>
        public virtual void OnFirstOpen() { }
        
        /// <summary>
        /// 在显示时执行一次
        /// </summary>
        public virtual void OnOpen() { }
        
        /// <summary>
        /// 在隐藏时执行一次
        /// </summary>
        public virtual void OnClose() { }
        
        /// <summary>
        /// 在被销毁时调用一次
        /// </summary>
        public virtual void OnDispose() { }

        /// <summary>
        /// 设置可见性
        /// </summary>
        public virtual void SetVisible(bool isVisble) { }
    }
}