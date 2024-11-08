using MRFramework;

public class Singleton<T> : ISingleton where T : Singleton<T>
{
    private static T m_Instance;

    private static readonly object m_Lock = new object();

    public static T Instance
    {
        get
        {
            lock (m_Lock)
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
    /// 单例初始化
    /// </summary>
    public virtual void OnSingletonInit()
    {

    }

    /// <summary>
    /// 资源释放，手动调用
    /// </summary>
    public virtual void OnDispose()
    {
        m_Instance = null;
    }
}