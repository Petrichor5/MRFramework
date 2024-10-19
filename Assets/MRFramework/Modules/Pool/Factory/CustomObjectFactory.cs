using System;

/// <summary>
/// 自定义对象工厂：相关对象是 自己定义 
/// </summary>
/// <typeparam name="T"></typeparam>
public class CustomObjectFactory<T> : IObjectFactory<T>
{
    protected Func<T> FactoryMethod;
    
    public CustomObjectFactory(Func<T> factoryMethod)
    {
        FactoryMethod = factoryMethod;
    }

    public T Create()
    {
        return FactoryMethod();
    }
}