﻿using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using VTU.Infrastructure.Attribute;

namespace VTU.Infrastructure.Extension
{
    /// <summary>
    /// App服务注册
    /// </summary>
    public static class AppServiceExtensions
    {
        /// <summary>
        /// 注册引用程序域中所有有AppService标记的类的服务
        /// </summary>
        /// <param name="services"></param>
        public static void AddAppService(this IServiceCollection services)
        {
            // 需要扫描的application路径
            var appSettings = AppSettingInstants.GetAppSettings();
            foreach (var item in appSettings.ScanServicePaths)
            {
                Register(services, item);
            }
        }

        private static void Register(IServiceCollection services, string item)
        {
            var assembly = Assembly.Load(item);
            foreach (var type in assembly.GetTypes())
            {
                var serviceAttribute = type.GetCustomAttribute<ServiceAttribute>();

                if (serviceAttribute != null)
                {
                    var serviceType = serviceAttribute.ServiceType;
                    //情况1 适用于依赖抽象编程，注意这里只获取第一个
                    if (serviceType == null && serviceAttribute.InterfaceServiceType)
                    {
                        serviceType = type.GetInterfaces().FirstOrDefault();
                    }

                    //情况2 不常见特殊情况下才会指定ServiceType，写起来麻烦
                    serviceType ??= type;

                    switch (serviceAttribute.ServiceLifetime)
                    {
                        case LifeTime.Singleton:
                            services.AddSingleton(serviceType, type);
                            break;
                        case LifeTime.Scoped:
                            services.AddScoped(serviceType, type);
                            break;
                        case LifeTime.Transient:
                            services.AddTransient(serviceType, type);
                            break;
                        default:
                            services.AddTransient(serviceType, type);
                            break;
                    }
                    //System.Console.WriteLine($"注册：{serviceType}");
                }
            }
        }
    }
}