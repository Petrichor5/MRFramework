using System.Collections;
using UnityEngine;

namespace MRFramework.UGUIPro
{
    public interface IViewItem
    {
        void OnUpdateItemData<T>(int index, T data);
    }
}