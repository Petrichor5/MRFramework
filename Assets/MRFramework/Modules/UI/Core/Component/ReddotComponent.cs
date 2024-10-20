using Config;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReddotComponent
{
    private List<ReddotNode> m_ReddotNodeList; // 绑定事件的红点控件

    /// <summary>
    /// 清空所有红点刷新数据
    /// </summary>
    public void ResetAllReddotData()
    {
        if (m_ReddotNodeList == null) return;

        foreach (var item in m_ReddotNodeList)
        {
            item.ResetReddotData();
        }
    }

    /// <summary>
    /// 清空红点刷新数据
    /// </summary>
    public void ResetReddotData(ReddotNode item)
    {
        item.ResetReddotData();
    }

    /// <summary>
    /// 为红点控件绑定刷新事件
    /// </summary>
    public void SetReddotData(ReddotNode item, EReddot eReddot, string node)
    {
        if (m_ReddotNodeList == null) m_ReddotNodeList = new List<ReddotNode>();

        if (!m_ReddotNodeList.Contains(item))
        {
            m_ReddotNodeList.Add(item);
        }
        item.SetReddotData(eReddot, node);
    }

    public void ClearReddotData()
    {
        if (m_ReddotNodeList == null) return;

        foreach (var item in m_ReddotNodeList)
        {
            item.ResetReddotData();
        }
        m_ReddotNodeList.Clear();
    }

    public void Clear()
    {
        ClearReddotData();
    }
}