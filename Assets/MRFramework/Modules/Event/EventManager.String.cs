using System;
using System.Collections.Generic;

namespace MRFramework
{
    public partial class EventManager
    {
        private readonly Dictionary<string, IEventHandler> m_StringEventDic = new Dictionary<string, IEventHandler>();

        #region None
        public IUnRegister AddEventListener(string key, Action onEvent)
        {
            EventHandler eventHandler;
            if (m_StringEventDic.TryGetValue(key, out var e))
            {
                eventHandler = e.As<EventHandler>();
            }
            else
            {
                eventHandler = new EventHandler();
                m_StringEventDic.Add(key, eventHandler);
            }

            if (IsOpenEventLog)
                Log.Info("添加事件 Key: " + key);

            return eventHandler.Register(onEvent);
        }

        public void RemoveEventListener(string key, Action onEvent)
        {
            if (m_StringEventDic.TryGetValue(key, out var e))
            {
                var eventHandler = e.As<EventHandler>();
                eventHandler?.UnRegister(onEvent);

                if (IsOpenEventLog)
                    Log.Info("移除事件 Key: " + key);
            }
            else
            {
                if (IsOpenEventLog)
                    Log.Warning("移除事件失败，没有找到事件 Key: " + key);
            }
        }

        public void TriggerEventListener(string key)
        {
            if (m_StringEventDic.TryGetValue(key, out var e))
            {
                if (IsOpenEventLog)
                    Log.Info("触发事件 Key: " + key);

                var eventHandler = e.As<EventHandler>();
                eventHandler?.Trigger();

            }
            else
            {
                if (IsOpenEventLog)
                    Log.Warning("触发事件失败，没有找到事件 Key: " + key);
            }
        }
        #endregion

        #region T
        public IUnRegister AddEventListener<T>(string key, Action<T> onEvent)
        {
            EventHandler<T> eventHandler;
            if (m_StringEventDic.TryGetValue(key, out var e))
            {
                eventHandler = e.As<EventHandler<T>>();
            }
            else
            {
                eventHandler = new EventHandler<T>();
                m_StringEventDic.Add(key, eventHandler);
            }

            if (IsOpenEventLog)
                Log.Info("添加事件 Key: " + key);

            return eventHandler.Register(onEvent);
        }

        public void RemoveEventListener<T>(string key, Action<T> onEvent)
        {

            if (m_StringEventDic.TryGetValue(key, out var e))
            {
                var eventHandler = e.As<EventHandler<T>>();
                eventHandler?.UnRegister(onEvent);

                if (IsOpenEventLog)
                    Log.Info("移除事件 Key: " + key);
            }
            else
            {
                if (IsOpenEventLog)
                    Log.Warning("移除事件失败，没有找到事件 Key: " + key);
            }
        }

        public void TriggerEventListener<T>(string key, T data)
        {
            if (m_StringEventDic.TryGetValue(key, out var e))
            {
                if (IsOpenEventLog)
                    Log.Info($"触发事件 Key: {key}, data: {data}");

                var eventHandler = e.As<EventHandler<T>>();
                eventHandler?.Trigger(data);
            }
            else
            {
                if (IsOpenEventLog)
                    Log.Warning($"触发事件失败，没有找到事件 Key: {key}, data: {data}");
            }
        }
        #endregion

        #region T, K
        public IUnRegister AddEventListener<T, K>(string key, Action<T, K> onEvent)
        {
            EventHandler<T, K> eventHandler;
            if (m_StringEventDic.TryGetValue(key, out var e))
            {
                eventHandler = e.As<EventHandler<T, K>>();
            }
            else
            {
                eventHandler = new EventHandler<T, K>();
                m_StringEventDic.Add(key, eventHandler);
            }

            if (IsOpenEventLog)
                Log.Info("添加事件 Key: " + key);

            return eventHandler.Register(onEvent);
        }

        public void RemoveEventListener<T, K>(string key, Action<T, K> onEvent)
        {

            if (m_StringEventDic.TryGetValue(key, out var e))
            {
                var eventHandler = e.As<EventHandler<T, K>>();
                eventHandler?.UnRegister(onEvent);

                if (IsOpenEventLog)
                    Log.Info("移除事件 Key: " + key);
            }
            else
            {
                if (IsOpenEventLog)
                    Log.Warning("移除事件失败，没有找到事件 Key: " + key);
            }
        }

        public void TriggerEventListener<T, K>(string key, T data1, K data2)
        {
            if (m_StringEventDic.TryGetValue(key, out var e))
            {
                if (IsOpenEventLog)
                    Log.Info($"触发事件 Key: {key}, data1: {data1}, data2: {data2}");

                var eventHandler = e.As<EventHandler<T, K>>();
                eventHandler?.Trigger(data1, data2);
            }
            else
            {
                if (IsOpenEventLog)
                    Log.Warning($"触发事件失败，没有找到事件 Key: {key}, data1: {data1}, data2: {data2}");
            }
        }
        #endregion

        public void RemoveAllStringEventListener()
        {
            if (IsOpenEventLog)
                Log.Info("移除所有事件!!!!");
            
            foreach (var handle in m_StringEventDic.Values)
            {
                handle.Clear();
            }
        }
    }
}