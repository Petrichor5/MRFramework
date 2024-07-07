using System;

namespace MRFramework
{
    public struct CustomUnRegister : IUnRegister
    {
        private Action mOnUnRegister { get; set; }
        public CustomUnRegister(Action onUnRegister) => mOnUnRegister = onUnRegister;

        public void UnRegister()
        {
            mOnUnRegister.Invoke();
            mOnUnRegister = null;
        }
    }

    public interface IEventHandler
    {
        IUnRegister Register(Action onEvent);

        void Clear();
    }

    public class EventHandler : IEventHandler
    {
        private Action mOnEvent = () => { };

        public IUnRegister Register(Action onEvent)
        {
            mOnEvent += onEvent;
            return new CustomUnRegister(() => { UnRegister(onEvent); });
        }

        public void UnRegister(Action onEvent) => mOnEvent -= onEvent;

        public void Clear() => mOnEvent = null;

        public void Trigger() => mOnEvent?.Invoke();
    }

    public class EventHandler<T> : IEventHandler
    {
        private Action<T> mOnEvent = e => { };

        public IUnRegister Register(Action<T> onEvent)
        {
            mOnEvent += onEvent;
            return new CustomUnRegister(() => { UnRegister(onEvent); });
        }

        public void UnRegister(Action<T> onEvent) => mOnEvent -= onEvent;

        public void Clear() => mOnEvent = null;

        public void Trigger(T t) => mOnEvent?.Invoke(t);

        IUnRegister IEventHandler.Register(Action onEvent)
        {
            return Register(Action);
            void Action(T _) => onEvent(); // 局部函数
        }
    }

    public class EventHandler<T, K> : IEventHandler
    {
        private Action<T, K> mOnEvent = (t, k) => { };

        public IUnRegister Register(Action<T, K> onEvent)
        {
            mOnEvent += onEvent;
            return new CustomUnRegister(() => { UnRegister(onEvent); });
        }

        public void UnRegister(Action<T, K> onEvent) => mOnEvent -= onEvent;

        public void Clear() => mOnEvent = null;

        public void Trigger(T t, K k) => mOnEvent?.Invoke(t, k);

        IUnRegister IEventHandler.Register(Action onEvent)
        {
            return Register(Action);
            void Action(T _, K __) => onEvent();
        }
    }

    public class EventHandler<T, K, S> : IEventHandler
    {
        private Action<T, K, S> mOnEvent = (t, k, s) => { };

        public IUnRegister Register(Action<T, K, S> onEvent)
        {
            mOnEvent += onEvent;
            return new CustomUnRegister(() => { UnRegister(onEvent); });
        }

        public void UnRegister(Action<T, K, S> onEvent) => mOnEvent -= onEvent;

        public void Clear() => mOnEvent = null;

        public void Trigger(T t, K k, S s) => mOnEvent?.Invoke(t, k, s);

        IUnRegister IEventHandler.Register(Action onEvent)
        {
            return Register(Action);
            void Action(T _, K __, S ___) => onEvent();
        }
    }
}