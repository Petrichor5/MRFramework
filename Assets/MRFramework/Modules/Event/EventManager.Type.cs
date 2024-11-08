using System;
using System.Collections.Generic;

namespace MRFramework
{
    public partial class EventManager
    {
        private readonly Dictionary<Type, IEventHandler> m_TypeEventDic = new Dictionary<Type, IEventHandler>();

        public IUnRegister AddEventListener<T>(Action<T> onEvent)
        {
            if (IsOpenEventLog)
                Log.Info("添加事件 Key: " + typeof(T).Name);
            
            return GetOrAddEvent<EventHandler<T>>().Register(onEvent);
        }

        public void RemoveEventListener<T>(Action<T> onEvent)
        {
            var eType = GetEvent<EventHandler<T>>();
            if (eType != null)
            {
                eType.UnRegister(onEvent);
                
                if (IsOpenEventLog)
                    Log.Info("移除事件 Key: " + typeof(T).Name);
            }
            else
            {
                if (IsOpenEventLog)
                    Log.Warning("移除事件失败，没有找到事件 Key: " + typeof(T).Name);
            }
        }

        public void TriggerEventListener<T>() where T : new()
        {
            var eType = GetEvent<EventHandler<T>>();
            if (eType != null)
            {
                eType.Trigger(new T());
                
                if (IsOpenEventLog)
                    Log.Info("触发事件 Key: " + typeof(T).Name);
            }
            else
            {
                if (IsOpenEventLog)
                    Log.Warning("触发事件失败，没有找到事件 Key: " + typeof(T).Name);
            }
        }

        public void TriggerEventListener<T>(T e)
        {
            var eType = GetEvent<EventHandler<T>>();
            if (eType != null)
            {
                eType.Trigger(e);
                
                if (IsOpenEventLog)
                    Log.Info("触发事件 Key: " + typeof(T).Name);
            }
            else
            {
                if (IsOpenEventLog)
                    Log.Warning("触发事件失败，没有找到事件 Key: " + typeof(T).Name);
            }
        }
        
        public void RemoveAllTypeEventListener()
        {
            if (IsOpenEventLog)
                Log.Info("移除所有事件!!!!");
            
            foreach (var handle in m_TypeEventDic.Values)
            {
                handle.Clear();
            }
        }

        #region 内部实现

        private T GetEvent<T>() where T : IEventHandler
        {
            if (m_TypeEventDic.TryGetValue(typeof(T), out var eventHandler))
            {
                return (T)eventHandler;
            }
            return default;
        }

        private T GetOrAddEvent<T>() where T : IEventHandler, new()
        {
            var eType = typeof(T);

            if (m_TypeEventDic.TryGetValue(eType, out var eventHandler))
            {
                return (T)eventHandler;
            }

            eventHandler = new T();
            m_TypeEventDic.Add(eType, eventHandler);
            return (T)eventHandler;
        }

        #endregion
    }
}