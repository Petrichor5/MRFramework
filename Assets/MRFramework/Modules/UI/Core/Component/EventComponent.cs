using MRFramework;
using System;
using System.Collections.Generic;

public class EventComponent : IUnRegisterList
{
    public List<IUnRegister> UnRegisterList { get; } = new List<IUnRegister>();

    /************************** 类型事件 **************************/

    public void AddEventListener<T>(Action<T> callback)
    {
        EventManager.Instance.AddEventListener(callback)
            .AddToUnregisterList(this);
    }

    public void TriggerEventListener<T>(T callback)
    {
        EventManager.Instance.TriggerEventListener(callback);
    }

    public void TriggerEventListener<T>() where T : new()
    {
        EventManager.Instance.TriggerEventListener<T>();
    }

    /************************** 字符串事件 **************************/

    public void AddEventListener(string key, Action callback)
    {
        EventManager.Instance.AddEventListener(key, callback)
            .AddToUnregisterList(this);
    }

    public void TriggerEventListener(string key)
    {
        EventManager.Instance.TriggerEventListener(key);
    }

    /*********** T1 ***********/

    public void AddEventListener<T>(string key, Action<T> callback)
    {
        EventManager.Instance.AddEventListener(key, callback)
            .AddToUnregisterList(this);
    }

    public void TriggerEventListener<T>(string key, T data)
    {
        EventManager.Instance.TriggerEventListener(key, data);
    }

    /*********** T1 T2 ***********/


    /************************** End **************************/

    public void Clear()
    {
        if (UnRegisterList == null) return;

        this.UnRegisterAll();
    }
}