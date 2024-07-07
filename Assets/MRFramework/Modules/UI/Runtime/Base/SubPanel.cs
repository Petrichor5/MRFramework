namespace MRFramework
{
    /// <summary>
    /// 子面板
    /// 可以创建多个，由所属的主面板统一管理
    /// </summary>
    public class SubPanel : UIBehaviour
    {
        /// <summary>
        /// 初始化子面板
        /// </summary>
        public void InitSubPanel()
        {
            OnAwake();
            SetVisible(true);
            OnOpen();
        }

        /// <summary>
        /// 打开子面板
        /// </summary>
        public void Open()
        {
            OnOpen();
            SetVisible(true);
        }
        
        /// <summary>
        /// 关闭子面板
        /// </summary>
        public void Close()
        {
            SetVisible(false);
            OnClose();
            RemoveAllTimer();
        }
    }
}