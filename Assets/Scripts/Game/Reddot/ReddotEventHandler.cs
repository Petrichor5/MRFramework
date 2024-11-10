using System;

/// <summary>
/// 红点事件句柄
/// </summary>
public class ReddotEventHandler
{
    private Action<bool> m_OnEvent;

    public void Register(Action<bool> onEvent)
    {
        m_OnEvent += onEvent;
    }

    public void UnRegister(Action<bool> onEvent)
    {
        m_OnEvent -= onEvent;
    }

    public void Clear()
    {
        m_OnEvent = null;
    }

    public void Trigger(bool flag)
    {
        m_OnEvent?.Invoke(flag);
    }
}
