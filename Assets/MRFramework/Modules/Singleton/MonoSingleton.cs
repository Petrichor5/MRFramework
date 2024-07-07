using MRFramework;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour, ISingleton where T : MonoSingleton<T>
{
    private static T mInstance;
    public static T Instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = SingletonCreator.CreateSingleton<T>();
            }

            return mInstance;
        }
    }

    public virtual void OnSingletonInit()
    {

    }

    /// <summary>
    /// 资源释放
    /// </summary>
    public virtual void Dispose()
    {
        Destroy(gameObject);
    }
    
    public virtual void OnDestroy()
    {
        mInstance = null;
    }
}
