using System.Collections;
using UnityEngine;
using MRFramework.UGUIPro;
using System.Collections.Generic;

public class ScrollComponent
{
    private List<ScrollBase> m_ScrollBaseList;

    public void AddScrollWidget(ScrollBase scroll)
    {
        if (m_ScrollBaseList == null)
            m_ScrollBaseList = new List<ScrollBase>();

        if (m_ScrollBaseList.Contains(scroll)) return;

        scroll.OnInit();
        m_ScrollBaseList.Add(scroll);
    }

    public void CloseAllItem()
    {
        if (m_ScrollBaseList == null) return;

        foreach (var scrollView in m_ScrollBaseList)
        {
            scrollView.CloseAllItem();
        }
    }

    public void OpenAllItem()
    {
        if (m_ScrollBaseList == null) return;

        foreach (var scrollView in m_ScrollBaseList)
        {
            scrollView.OpenAllItem();
        }
    }

    public void ClearAllItem()
    {
        if (m_ScrollBaseList == null) return;

        foreach (var scrollView in m_ScrollBaseList)
        {
            scrollView.Clear();
        }
    }

    public void Clear()
    {
        ClearAllItem();
    }
}