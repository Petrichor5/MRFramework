using MRFramework;

public class Singleton<T> : ISingleton where T : Singleton<T>
{
    private static T mInstance;

    private static object mLock = new object();

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
    /// 单例初始化
    /// </summary>
    public virtual void OnSingletonInit()
    {

    }

    /// <summary>
    /// 资源释放
    /// </summary>
    public virtual void Dispose()
    {
        mInstance = null;
    }
}