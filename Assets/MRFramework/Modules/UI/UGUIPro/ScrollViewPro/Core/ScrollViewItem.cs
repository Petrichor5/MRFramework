namespace MRFramework.UGUIPro
{
    public interface IViewItem
    {
        void OnUpdateItemData<T>(int index, T data);
    }

    public abstract class ScrollViewItem : SubPanel, IViewItem
    {
        public abstract void OnUpdateItemData<T>(int index, T data);
    }
}