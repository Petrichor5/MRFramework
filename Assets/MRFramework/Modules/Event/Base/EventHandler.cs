using System;

namespace MRFramework
{
    public struct CustomUnRegister : IUnRegister
    {
        private Action m_OnUnRegister { get; set; }
        public CustomUnRegister(Action onUnRegister) => m_OnUnRegister = onUnRegister;

        public void UnRegister()
        {
            m_OnUnRegister.Invoke();
            m_OnUnRegister = null;
        }
    }

    public interface IEventHandler
    {
        IUnRegister Register(Action onEvent);

        void Clear();
    }

    public class EventHandler : IEventHandler
    {
        private Action m_OnEvent = () => { };

        public IUnRegister Register(Action onEvent)
        {
            m_OnEvent += onEvent;
            return new CustomUnRegister(() => { UnRegister(onEvent); });
        }

        public void UnRegister(Action onEvent) => m_OnEvent -= onEvent;

        public void Clear() => m_OnEvent = null;

        public void Trigger() => m_OnEvent?.Invoke();
    }

    public class EventHandler<T> : IEventHandler
    {
        private Action<T> m_OnEvent = e => { };

        public IUnRegister Register(Action<T> onEvent)
        {
            m_OnEvent += onEvent;
            return new CustomUnRegister(() => { UnRegister(onEvent); });
        }

        public void UnRegister(Action<T> onEvent) => m_OnEvent -= onEvent;

        public void Clear() => m_OnEvent = null;

        public void Trigger(T t) => m_OnEvent?.Invoke(t);

        IUnRegister IEventHandler.Register(Action onEvent)
        {
            return Register(Action);
            void Action(T _) => onEvent(); // 局部函数
        }
    }

    public class EventHandler<T, K> : IEventHandler
    {
        private Action<T, K> m_OnEvent = (t, k) => { };

        public IUnRegister Register(Action<T, K> onEvent)
        {
            m_OnEvent += onEvent;
            return new CustomUnRegister(() => { UnRegister(onEvent); });
        }

        public void UnRegister(Action<T, K> onEvent) => m_OnEvent -= onEvent;

        public void Clear() => m_OnEvent = null;

        public void Trigger(T t, K k) => m_OnEvent?.Invoke(t, k);

        IUnRegister IEventHandler.Register(Action onEvent)
        {
            return Register(Action);
            void Action(T _, K __) => onEvent();
        }
    }

    public class EventHandler<T, K, S> : IEventHandler
    {
        private Action<T, K, S> m_OnEvent = (t, k, s) => { };

        public IUnRegister Register(Action<T, K, S> onEvent)
        {
            m_OnEvent += onEvent;
            return new CustomUnRegister(() => { UnRegister(onEvent); });
        }

        public void UnRegister(Action<T, K, S> onEvent) => m_OnEvent -= onEvent;

        public void Clear() => m_OnEvent = null;

        public void Trigger(T t, K k, S s) => m_OnEvent?.Invoke(t, k, s);

        IUnRegister IEventHandler.Register(Action onEvent)
        {
            return Register(Action);
            void Action(T _, K __, S ___) => onEvent();
        }
    }
}