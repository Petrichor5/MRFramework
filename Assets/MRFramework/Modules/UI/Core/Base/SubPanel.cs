namespace MRFramework
{
    /// <summary>
    /// 子面板: 可以创建多个，由所属的主面板统一管理
    /// </summary>
    public class SubPanel : PanelBase
    {
        #region 内部实现

        /// <summary>
        /// 初始化子面板
        /// </summary>
        public void OnInit()
        {
            OnFirstOpen();
            OnOpen();
        }

        /// <summary>
        /// 销毁子面板时调用
        /// </summary>
        public void OnClear()
        {
            OnClose();
            OnDispose();
        }

        #endregion

        /// <summary>
        /// 打开子面板
        /// </summary>
        public void Open()
        {
            SetVisible(true);
            OnOpen();
        }

        /// <summary>
        /// 关闭子面板
        /// </summary>
        public void Close()
        {
            SetVisible(false);
            OnClose();
        }
    }
}