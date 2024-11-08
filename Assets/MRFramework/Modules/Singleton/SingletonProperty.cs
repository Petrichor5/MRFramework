using MRFramework;

/// <summary>
/// 通过属性实现的 Singleton
/// </summary>
public static class SingletonProperty<T> where T : class, ISingleton
{
    /// <summary>
    /// 静态实例
    /// </summary>
    private static T m_Instance;

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
                if (m_Instance == null)
                {
                    m_Instance = SingletonCreator.CreateSingleton<T>();
                }
            }

            return m_Instance;
        }
    }

    /// <summary>
    /// 资源释放
    /// </summary>
    public static void Dispose()
    {
        m_Instance = null;
    }
}