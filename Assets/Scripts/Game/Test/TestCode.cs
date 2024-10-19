using MRFramework;
using MRFramework.UGUIPro;
using System.Collections.Generic;
using UnityEngine;

public class TestCode : MonoBehaviour
{
    public ScrollViewPro ScrollViewPro;

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

    public void RunTestCode()
    {
        UIManager.Instance.OpenPanel<WBP_GlobalUI_SceneProgressBar>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            ScrollViewPro.InitScrollView();
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
        }
    }
}