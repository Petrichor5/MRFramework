/// <summary>
/// I pool able.
/// </summary>
public interface IPoolable
{
    /// <summary>
    /// 用于初始化，建议不要做太多逻辑上的操作
    /// </summary>
    void OnAllAllocated();

    /// <summary>
    /// 用于重置数据，建议不要做太多逻辑上的操作
    /// </summary>
    void OnRecycled();
    
    /// <summary>
    /// 是否被回收
    /// </summary>
    bool IsRecycled { get; set; }
}