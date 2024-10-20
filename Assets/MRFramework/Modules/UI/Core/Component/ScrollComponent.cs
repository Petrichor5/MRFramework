using System.Collections;
using UnityEngine;
using MRFramework.UGUIPro;
using System.Collections.Generic;

public class ScrollComponent
{
    private List<ScrollViewPro> m_ScrollViewList;

    public void InitScrollView(ScrollViewPro scrollView)
    {
        if (m_ScrollViewList == null)
            m_ScrollViewList = new List<ScrollViewPro>();

        if (m_ScrollViewList.Contains(scrollView)) return;

        scrollView.InitScrollView();
        m_ScrollViewList.Add(scrollView);
    }

    public void CloseAllItem()
    {
        if (m_ScrollViewList == null) return;

        foreach (var scrollView in m_ScrollViewList)
        {
            scrollView.CloseAllItem();
        }
    }

    public void OpenAllItem()
    {
        if (m_ScrollViewList == null) return;

        foreach (var scrollView in m_ScrollViewList)
        {
            scrollView.OpenAllItem();
        }
    }

    public void ClearAllItem()
    {
        if (m_ScrollViewList == null) return;

        foreach (var scrollView in m_ScrollViewList)
        {
            scrollView.Clear();
        }
    }

    public void Clear()
    {
        ClearAllItem();
    }
}