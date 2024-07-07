using System;

namespace MRFramework
{
    /// <summary>
    /// 定时器
    /// </summary>
    public class Timer : IComparable<Timer>
    {
        public int Id;                                  // Timer唯一Id
        public float DelaySeconds;                      // 计时时长
        public long ExpirationTime;                     // 计时结束时间
        public Action Callback;                         // 计时结束回调
        public bool IsLoop;                             // 是否循环
        // public Action<int> CallbackWithReturnValue;     // 带返回值的回调
        // public int ReturnValue;                         // 每隔多少秒，需要返回的值

        public void Init(float delaySeconds, Action callback, bool isLoop)
        {
            DelaySeconds = delaySeconds;
            Callback = callback;
            IsLoop = isLoop;
            // CallbackWithReturnValue = callbackWithReturnValue;
            // ReturnValue = returnValue;
        }

        public void Clear()
        {
            Id = 0;
            DelaySeconds = 0f;
            ExpirationTime = 0;
            Callback = null;
            IsLoop = false;
            // CallbackWithReturnValue = null;
            // ReturnValue = 0;
        }

        public int CompareTo(Timer other)
        {
            if (ReferenceEquals(this, other)) return 0; // 如果是同一个实例，返回0，表示相等
            if (ReferenceEquals(null, other)) return 1; // 如果比较的对象为空，返回1，表示当前实例大于空对象
            return ExpirationTime.CompareTo(other.ExpirationTime); // 比较两个Timer的ExpirationTime
        }
    }
}