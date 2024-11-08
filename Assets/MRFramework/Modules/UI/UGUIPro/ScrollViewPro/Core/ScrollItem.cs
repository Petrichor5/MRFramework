namespace MRFramework.UGUIPro
{
    public abstract class ScrollItem : SubPanel
    {
        public abstract void OnUpdateItemData<T>(int index, T data);

        public override void ShowAnimation()
        {
            // 重写覆盖父类方法
        }

        public override void HideAnimation()
        {
            // 重写覆盖父类方法
        }
    }
}