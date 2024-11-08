using System.Collections;
using MRFramework;
using UnityEngine.Events;

[MonoSingletonPath("MRFramework/GlobalManager")]
public class GlobalManager : MonoSingleton<GlobalManager>
{
    private event UnityAction m_UpdateEvent;
    private event UnityAction m_FixedUpdateEvent;
    private event UnityAction m_LateUpdateEvent;

    private void OnDestroy()
    {
        m_UpdateEvent = null;
        m_FixedUpdateEvent = null;
        m_LateUpdateEvent = null;
    }

    private void Update()
    {
        m_UpdateEvent?.Invoke();
    }

    private void FixedUpdate()
    {
        m_FixedUpdateEvent?.Invoke();
    }

    private void LateUpdate()
    {
        m_LateUpdateEvent?.Invoke();
    }

    public void AddUpdateListener(UnityAction updateFun)
    {
        m_UpdateEvent += updateFun;
    }

    public void RemoveUpdateListener(UnityAction updateFun)
    {
        m_UpdateEvent -= updateFun;
    }

    public void AddFixedUpdateListener(UnityAction updateFun)
    {
        m_FixedUpdateEvent += updateFun;
    }

    public void RemoveFixedUpdateListener(UnityAction updateFun)
    {
        m_FixedUpdateEvent -= updateFun;
    }

    public void AddLateUpdateListener(UnityAction updateFun)
    {
        m_LateUpdateEvent += updateFun;
    }

    public void RemoveLateUpdateListener(UnityAction updateFun)
    {
        m_LateUpdateEvent -= updateFun;
    }
}