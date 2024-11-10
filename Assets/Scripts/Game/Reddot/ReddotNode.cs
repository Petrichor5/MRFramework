using Config;
using MRFramework;
using MRFramework.UGUIPro;
using System.Collections.Generic;
using UnityEngine;

public class ReddotNode : MonoBehaviour, ICanGetSystem
{
    public ImagePro Reddot;

    private List<string> m_ReddotKeyList;

    private void Awake()
    {
        if (Reddot == null) Reddot = GetComponent<ImagePro>();
        Reddot.SetVisible(false);
    }

    private void OnDestroy()
    {
        Clear();
    }

    public IArchitecture GetArchitecture()
    {
        return MRF.Instance;
    }

    /// <summary>
    /// 清空红点刷新数据
    /// </summary>
    public void ResetReddotData()
    {
        if (m_ReddotKeyList != null)
        {
            Clear();
        }
    }

    /// <summary>
    /// 绑定红点刷新数据
    /// </summary>
    public void SetReddotData(EReddot eReddot, string node)
    {
        if (m_ReddotKeyList == null) m_ReddotKeyList = new List<string>();
        string key = ReddotTool.MKReddotKey(eReddot, node);
        this.GetSystem<ReddotSystem>().AddEventListener(key, OnRefreshReddot);
        m_ReddotKeyList.Add(key);
    }

    private void OnRefreshReddot(bool flag)
    {
        if (Reddot) Reddot.SetVisible(flag);
    }

    private void Clear()
    {
        ReddotSystem reddotMgr = this.GetSystem<ReddotSystem>();
        foreach (var key in m_ReddotKeyList)
        {
            reddotMgr.RemoveEventListener(key, OnRefreshReddot);
        }
        m_ReddotKeyList.Clear();
    }
}
