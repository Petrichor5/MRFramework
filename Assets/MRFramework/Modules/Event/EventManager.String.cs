using System;
using System.Collections.Generic;

namespace MRFramework
{
    public partial class EventManager
    {
        private readonly Dictionary<string, IEventHandler> mStringEventDic = new Dictionary<string, IEventHandler>();

        public IUnRegister AddEventListener(string key, Action onEvent)
        {
            EventHandler eventHandler;
            if (mStringEventDic.TryGetValue(key, out var e))
            {
                eventHandler = e.As<EventHandler>();
            }
            else
            {
                eventHandler = new EventHandler();
                mStringEventDic.Add(key, eventHandler);
            }

            if (IsOpenEventLog)
                UnityEngine.Debug.Log("添加事件 Key: " + key);

            return eventHandler.Register(onEvent);
        }

        public void RemoveEventListener(string key, Action onEvent)
        {
            if (mStringEventDic.TryGetValue(key, out var e))
            {
                var eventHandler = e.As<EventHandler>();
                eventHandler?.UnRegister(onEvent);
                
                if (IsOpenEventLog)
                    UnityEngine.Debug.Log("移除事件 Key: " + key);
            }
            else
            {
                if (IsOpenEventLog)
                    UnityEngine.Debug.LogWarning("移除事件失败，没有找到事件 Key: " + key);
            }
        }

        public void TriggerEventListener(string key)
        {
            if (mStringEventDic.TryGetValue(key, out var e))
            {
                if (IsOpenEventLog)
                    UnityEngine.Debug.Log("触发事件 Key: " + key);
                
                var eventHandler = e.As<EventHandler>();
                eventHandler?.Trigger();
                
            }
            else
            {
                if (IsOpenEventLog)
                    UnityEngine.Debug.LogWarning("触发事件失败，没有找到事件 Key: " + key);
            }
        }

        public IUnRegister AddEventListener<T>(string key, Action<T> onEvent)
        {
            EventHandler<T> eventHandler;
            if (mStringEventDic.TryGetValue(key, out var e))
            {
                eventHandler = e.As<EventHandler<T>>();
            }
            else
            {
                eventHandler = new EventHandler<T>();
                mStringEventDic.Add(key, eventHandler);
            }
            
            if (IsOpenEventLog)
                UnityEngine.Debug.Log("添加事件 Key: " + key);
            
            return eventHandler.Register(onEvent);
        }

        public void RemoveEventListener<T>(string key, Action<T> onEvent)
        {

            if (mStringEventDic.TryGetValue(key, out var e))
            {
                var eventHandler = e.As<EventHandler<T>>();
                eventHandler?.UnRegister(onEvent);
                
                if (IsOpenEventLog)
                    UnityEngine.Debug.Log("移除事件 Key: " + key);
            }
            else
            {
                if (IsOpenEventLog)
                    UnityEngine.Debug.LogWarning("移除事件失败，没有找到事件 Key: " + key);
            }
        }

        public void TriggerEventListener<T>(string key, T data)
        {
            if (mStringEventDic.TryGetValue(key, out var e))
            {
                if (IsOpenEventLog)
                    UnityEngine.Debug.Log("触发事件 Key: " + key);
                
                var eventHandler = e.As<EventHandler<T>>();
                eventHandler?.Trigger(data);
            }
            else
            {
                if (IsOpenEventLog)
                    UnityEngine.Debug.LogWarning("触发事件失败，没有找到事件 Key: " + key);
            }
        }
        
        public void RemoveAllStringEventListener()
        {
            if (IsOpenEventLog)
                UnityEngine.Debug.Log("移除所有事件!!!!");
            
            foreach (var handle in mStringEventDic.Values)
            {
                handle.Clear();
            }
        }
    }
}