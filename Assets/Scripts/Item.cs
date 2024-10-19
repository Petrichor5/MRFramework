using MRFramework.UGUIPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ItemData
{
    public ItemData(string name)
    {
        Name = name;
    }

    public string Name;
}

public class Item : ScrollViewItem
{
    public override void OnUpdateItemData<T>(int index, T data)
    {
        if (data is ItemData itemData)
        {
            
        }
    }
}
