using MRFramework;

/// <summary>
/// 通过属性实现的 Singleton
/// </summary>
public static class SingletonProperty<T> where T : class, ISingleton
{
    /// <summary>
    /// 静态实例
    /// </summary>
    private static T mInstance;

    /// <summary>
    /// 标签锁
    /// </summary>
    private static readonly object mLock = new object();

    /// <summary>
    /// 静态属性
    /// </summary>
    public static T Instance
    {
        get
        {
            lock (mLock)
            {
                if (mInstance == null)
                {
                    mInstance = SingletonCreator.CreateSingleton<T>();
                }
            }

            return mInstance;
        }
    }

    /// <summary>
    /// 资源释放
    /// </summary>
    public static void Dispose()
    {
        mInstance = null;
    }
}