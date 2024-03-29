﻿namespace VTU.Infrastructure.Attribute;

/// <summary>
/// 标记服务
/// 如何使用？
/// 1、如果服务是本身 直接在类上使用[Service]
/// 2、如果服务是接口 在类上使用 [Service(ServiceType = typeof(实现接口))]
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class ServiceAttribute : System.Attribute
{
    /// <summary>
    /// 服务声明周期
    /// 不给默认值的话注册的是AddSingleton
    /// </summary>
    public LifeTime ServiceLifetime { get; set; } = LifeTime.Scoped;

    /// <summary>
    /// 指定服务类型
    /// </summary>
    public Type ServiceType { get; set; }

    /// <summary>
    /// 是否可以从第一个接口获取服务类型
    /// </summary>
    public bool InterfaceServiceType { get; set; }
}

public enum LifeTime
{
    Transient,
    Scoped,
    Singleton
}