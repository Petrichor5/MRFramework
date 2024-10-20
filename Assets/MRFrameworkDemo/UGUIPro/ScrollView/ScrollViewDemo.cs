using MRFramework.UGUIPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollViewDemo : MonoBehaviour
{
    public ScrollViewPro ScrollViewPro;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            ScrollViewPro.InitScrollView();
            Debug.Log("初始化 ScrollViewPro");
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            ScrollViewPro.SetItems(itemDatas);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            ScrollViewPro.ClearItems();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            // 销毁时调用
            ScrollViewPro.Clear();
            Debug.Log("销毁 ScrollViewPro");
        }
    }

    private List<ItemData> itemDatas = new List<ItemData>()
    {
        new ItemData("123"),
        new ItemData("321"),
        new ItemData("1"),
        new ItemData("2"),
        new ItemData("3"),
        new ItemData("4"),
        new ItemData("5"),
        new ItemData("5"),
        new ItemData("5"),
        new ItemData("5"),
        new ItemData("5"),
        new ItemData("5"),
        new ItemData("5"),
        new ItemData("5"),
        new ItemData("5"),
        new ItemData("5"),
        new ItemData("5"),
        new ItemData("5"),
        new ItemData("5"),
        new ItemData("1"),
        new ItemData("2"),
        new ItemData("3"),
        new ItemData("4"),
        new ItemData("5"),
        new ItemData("5"),
        new ItemData("5"),
        new ItemData("5"),
        new ItemData("5"),
        new ItemData("5"),
        new ItemData("5"),
        new ItemData("5"),
        new ItemData("5"),
        new ItemData("5"),
        new ItemData("5"),
        new ItemData("5"),
        new ItemData("5"),
    };
}
