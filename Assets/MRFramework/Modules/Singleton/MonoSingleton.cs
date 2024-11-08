using MRFramework;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour, ISingleton where T : MonoSingleton<T>
{
    private static T m_Instance;
    public static T Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = SingletonCreator.CreateSingleton<T>();
            }

            return m_Instance;
        }
    }

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
