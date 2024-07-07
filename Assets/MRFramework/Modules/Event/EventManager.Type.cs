using System;
using System.Collections.Generic;

namespace MRFramework
{
    public partial class EventManager
    {
        private readonly Dictionary<Type, IEventHandler> mTypeEventDic = new Dictionary<Type, IEventHandler>();

        public IUnRegister AddEventListener<T>(Action<T> onEvent)
        {
            if (IsOpenEventLog)
                UnityEngine.Debug.Log("添加事件 Key: " + typeof(T).Name);
            
            return GetOrAddEvent<EventHandler<T>>().Register(onEvent);
        }

        public void RemoveEventListener<T>(Action<T> onEvent)
        {
            var eType = GetEvent<EventHandler<T>>();
            if (eType != null)
            {
                eType.UnRegister(onEvent);
                
                if (IsOpenEventLog)
                    UnityEngine.Debug.Log("移除事件 Key: " + typeof(T).Name);
            }
            else
            {
                if (IsOpenEventLog)
                    UnityEngine.Debug.LogWarning("移除事件失败，没有找到事件 Key: " + typeof(T).Name);
            }
        }

        public void TriggerEventListener<T>() where T : new()
        {
            var eType = GetEvent<EventHandler<T>>();
            if (eType != null)
            {
                eType.Trigger(new T());
                
                if (IsOpenEventLog)
                    UnityEngine.Debug.Log("触发事件 Key: " + typeof(T).Name);
            }
            else
            {
                if (IsOpenEventLog)
                    UnityEngine.Debug.LogWarning("触发事件失败，没有找到事件 Key: " + typeof(T).Name);
            }
        }

        public void TriggerEventListener<T>(T e)
        {
            var eType = GetEvent<EventHandler<T>>();
            if (eType != null)
            {
                eType.Trigger(e);
                
                if (IsOpenEventLog)
                    UnityEngine.Debug.Log("触发事件 Key: " + typeof(T).Name);
            }
            else
            {
                if (IsOpenEventLog)
                    UnityEngine.Debug.LogWarning("触发事件失败，没有找到事件 Key: " + typeof(T).Name);
            }
        }
        
        public void RemoveAllTypeEventListener()
        {
            if (IsOpenEventLog)
                UnityEngine.Debug.Log("移除所有事件!!!!");
            
            foreach (var handle in mTypeEventDic.Values)
            {
                handle.Clear();
            }
        }

        #region 内部实现

        private T GetEvent<T>() where T : IEventHandler
        {
            if (mTypeEventDic.TryGetValue(typeof(T), out var eventHandler))
            {
                return (T)eventHandler;
            }
            return default;
        }

        private T GetOrAddEvent<T>() where T : IEventHandler, new()
        {
            var eType = typeof(T);

            if (mTypeEventDic.TryGetValue(eType, out var eventHandler))
            {
                return (T)eventHandler;
            }

            eventHandler = new T();
            mTypeEventDic.Add(eType, eventHandler);
            return (T)eventHandler;
        }

        #endregion
    }
}