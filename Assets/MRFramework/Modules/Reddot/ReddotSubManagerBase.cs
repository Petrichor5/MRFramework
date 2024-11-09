using Config;
using MRFramework;
using System;
using System.Collections.Generic;

public abstract class ReddotSubManagerBase : IUnRegisterList
{
    public List<IUnRegister> UnRegisterList { get; } = new List<IUnRegister>();

    public void OnRegister()
    {
        AddEventListeners();
        InitReddot();
    }

    public void OnUnRegister()
    {
        this.UnRegisterAll();
    }

    #region 事件接口封装，按需要扩展

    /************************** 类型事件 **************************/

    public void AddEventListener<T>(Action<T> callback)
    {
        EventManager.Instance.AddEventListener(callback).AddToUnregisterList(this);
    }

    public void TriggerEventListener<T>(T callback)
    {
        EventManager.Instance.TriggerEventListener(callback);
    }

    public void TriggerEventListener<T>() where T : new()
    {
        EventManager.Instance.TriggerEventListener<T>();
    }

    /************************** 字符串事件 **************************/

    public void AddEventListener(string key, Action callback)
    {
        EventManager.Instance.AddEventListener(key, callback).AddToUnregisterList(this);
    }

    public void TriggerEventListener(string key)
    {
        EventManager.Instance.TriggerEventListener(key);
    }

    /*********** T1 ***********/

    public void AddEventListener<T>(string key, Action<T> callback)
    {
        EventManager.Instance.AddEventListener(key, callback).AddToUnregisterList(this);
    }

    public void TriggerEventListener<T>(string key, T data)
    {
        EventManager.Instance.TriggerEventListener(key, data);
    }

    /*********** T1 T2 ***********/



    #endregion

    /// <summary>
    /// 添加红点事件
    /// </summary>
    public abstract void AddEventListeners();

    /// <summary>
    /// 初始化红点管理器
    /// </summary>
    public abstract void InitReddot();

    /// <summary>
    /// 构建红点Key
    /// </summary>
    public string MKReddotKey(EReddot eReddot, string node)
    {
        return ReddotTool.MKReddotKey(eReddot, node);
    }

    /// <summary>
    /// 添加红点节点
    /// </summary>
    public void AddReddotNode(EReddot eReddot, string node = null)
    {
        ReddotManager.Instance.AddReddotNode(eReddot, node);
    }

    /// <summary>
    /// 移除红点节点
    /// </summary>
    public void RemoveReddotNode(EReddot eReddot, string node = null)
    {
        ReddotManager.Instance.RemoveReddotNode(eReddot, node);
    }

    /// <summary>
    /// 获取红点的状态
    /// </summary>
    public bool GetReddotState(EReddot eReddot, string node = null)
    {
        return ReddotManager.Instance.GetReddotState(eReddot, node);
    }

    /// <summary>
    /// 设置红点状态
    /// </summary>
    public void SetReddotState(EReddot eReddot, string node, bool state)
    {
        ReddotManager.Instance.SetReddotState(eReddot, node, state);
    }

    /// <summary>
    /// 刷新根节点状态
    /// </summary>
    public void RefreshRootReddotState(EReddot eReddot)
    {
        ReddotManager.Instance.RefreshRootReddotState(eReddot);
    }

    /// <summary>
    /// 重置整颗红点树状态为False
    /// </summary>
    public void ResetReddotState(EReddot eReddot)
    {
        ReddotManager.Instance.ResetReddotState(eReddot);
    }
}
