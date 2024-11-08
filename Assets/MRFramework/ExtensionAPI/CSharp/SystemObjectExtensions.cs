using System;

/// <summary>
/// 针对 System.Object 提供的链式扩展，理论上任何对象都可以使用
/// </summary>
public static class SystemObjectExtensions
{
    /// <summary>
    /// 将自己传到 Action 委托中
    /// </summary>
    public static T Self<T>(this T self, Action<T> onDo)
    {
        onDo?.Invoke(self);
        return self;
    }

    /// <summary>
    /// 将自己传到 Func&lt;T,T&gt; 委托中,然后返回自己
    /// </summary>
    public static T Self<T>(this T self, Func<T, T> onDo)
    {
        return onDo.Invoke(self);
    }

    /// <summary>
    /// 判断是否为空
    /// </summary>
    public static bool IsNull<T>(this T selfObj) where T : class
    {
        return null == selfObj;
    }

    /// <summary>
    /// 判断不是为空
    /// </summary>
    public static bool IsNotNull<T>(this T selfObj) where T : class
    {
        return null != selfObj;
    }

    /// <summary>
    /// 转型
    /// </summary>
    public static T As<T>(this object selfObj) where T : class
    {
        return selfObj as T;
    }
}
