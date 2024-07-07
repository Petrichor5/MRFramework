using UnityEngine;
using UnityEngine.Rendering;

namespace MRFramework
{
    /// <summary>
    /// UI面板的基类
    /// </summary>
    public class UIBase : MonoBehaviour
    {
        /// <summary>
        /// 面板名称
        /// </summary>
        public string PanelName { get; set; }
        
        /// <summary>
        /// 面板遮罩
        /// </summary>
        [HideInInspector]
        public CanvasGroup UIMask;
        
        /// <summary>
        /// 控制显隐
        /// </summary>
        [HideInInspector]
        public CanvasGroup CanvasGroup;

        /// <summary>
        /// UI控件容器
        /// </summary>
        [HideInInspector]
        public Transform UIContent;
        
        /// <summary>
        /// 层级排序
        /// </summary>
        public SortingGroup SortingGroup { get; set; }
        
        /// <summary>
        /// 当前窗口的可见性
        /// </summary>
        public bool IsVisible { get; set; }

        /// <summary>
        /// 只会在窗口创建时执行一次
        /// </summary>
        public virtual void OnAwake()
        {
        }

        /// <summary>
        /// 窗口打开时执行
        /// </summary>
        public virtual void OnOpen()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void OnUpdate()
        {
        }

        /// <summary>
        /// 窗口关闭时执行
        /// </summary>
        public virtual void OnClose()
        {
        }

        /// <summary>
        /// 窗口销毁时执行
        /// </summary>
        public virtual void OnDispose()
        {
        }

        /// <summary>
        /// 设置当前窗口的可见性
        /// </summary>
        public virtual void SetVisible(bool isVisible)
        {
        }
    }
}