using Config;
using MRFramework;

/// <summary>
/// 红点子系统基类
/// </summary>
public abstract class ReddotSubSystemBase : ICanGetSystem
{
    public EventComponent EventComponent = new EventComponent();

    public void OnRegister()
    {
        AddEventListeners();
        InitReddot();
    }

    public void OnUnRegister()
    {
        EventComponent.Clear();
    }

    public IArchitecture GetArchitecture()
    {
        return MRF.Instance;
    }

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
        this.GetSystem<ReddotSystem>().AddReddotNode(eReddot, node);
    }

    /// <summary>
    /// 移除红点节点
    /// </summary>
    public void RemoveReddotNode(EReddot eReddot, string node = null)
    {
        this.GetSystem<ReddotSystem>().RemoveReddotNode(eReddot, node);
    }

    /// <summary>
    /// 获取红点的状态
    /// </summary>
    public bool GetReddotState(EReddot eReddot, string node = null)
    {
        return this.GetSystem<ReddotSystem>().GetReddotState(eReddot, node);
    }

    /// <summary>
    /// 设置红点状态
    /// </summary>
    public void SetReddotState(EReddot eReddot, string node, bool state)
    {
        this.GetSystem<ReddotSystem>().SetReddotState(eReddot, node, state);
    }

    /// <summary>
    /// 刷新根节点状态
    /// </summary>
    public void RefreshRootReddotState(EReddot eReddot)
    {
        this.GetSystem<ReddotSystem>().RefreshRootReddotState(eReddot);
    }

    /// <summary>
    /// 重置整颗红点树状态为False
    /// </summary>
    public void ResetReddotState(EReddot eReddot)
    {
        this.GetSystem<ReddotSystem>().ResetReddotState(eReddot);
    }
}
