using MRFramework.UGUIPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollViewDemo : MonoBehaviour
{
    public ScrollViewPro ScrollViewProVertical;
    public ScrollViewPro ScrollViewProHorizontal;

    public ScrollListPro ScrollListProHorizontal;
    public ScrollListPro ScrollListProVertical;

    void Update()
    {
        #region ScrollView

        if (Input.GetKeyDown(KeyCode.Q))
        {
            ScrollViewProVertical.OnInit();
            ScrollViewProHorizontal.OnInit();
            Debug.Log("初始化 ScrollViewPro");
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            ScrollViewProVertical.SetItems(itemDatas);
            ScrollViewProHorizontal.SetItems(itemDatas);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            ScrollViewProVertical.ClearItems();
            ScrollViewProHorizontal.ClearItems();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            // 销毁时调用
            ScrollViewProVertical.Clear();
            ScrollViewProHorizontal.Clear();
            Debug.Log("销毁 ScrollViewPro");
        }

        #endregion

        #region ScrollList Horizontal

        if (Input.GetKeyDown(KeyCode.Q))
        {
            ScrollListProHorizontal.OnInit();
            Debug.Log("初始化 ScrollViewPro");
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            ScrollListProHorizontal.SetItems(itemDatas);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            ScrollListProHorizontal.ClearItems();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            // 销毁时调用
            ScrollListProHorizontal.Clear();
            Debug.Log("销毁 ScrollViewPro");
        }

        #endregion

        #region ScrollList Vertical

        if (Input.GetKeyDown(KeyCode.Q))
        {
            ScrollListProVertical.OnInit();
            Debug.Log("初始化 ScrollViewPro");
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            ScrollListProVertical.SetItems(itemDatas);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            ScrollListProVertical.ClearItems();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            // 销毁时调用
            ScrollListProVertical.Clear();
            Debug.Log("销毁 ScrollViewPro");
        }

        #endregion
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
