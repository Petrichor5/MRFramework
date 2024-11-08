using MRFramework.UGUIPro;

public struct ItemData
{
    public ItemData(string name)
    {
        Name = name;
    }

    public string Name;
}

public class Item : ScrollItem
{
    public override void OnUpdateItemData<T>(int index, T data)
    {
        if (data is ItemData itemData)
        {
            
        }
    }
}
