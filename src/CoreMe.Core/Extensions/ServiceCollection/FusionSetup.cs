/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Core.WebApi.Extensions
*   文件名称 ：FusionSetup.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-03 13:23:32
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using AspNetCoreRateLimit;
using CoreMe.Core.Common.Configs;
using EasyCaching.FreeRedis;
using EasyCaching.Serialization.SystemTextJson.Configurations;
using FreeRedis;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CoreMe.Core.Extensions.ServiceCollection
{
    /// <summary>
    /// 配置注册服务融合
    /// </summary>
    public static class FusionSetup
    {
        /// <summary>
        /// 配置注册Mapster服务
        /// </summary>
        public static IServiceCollection AddMapper(this IServiceCollection services, params Assembly[] assemblies)
        {
            // 获取全局映射配置
            var config = TypeAdapterConfig.GlobalSettings;

            // 扫描所有继承  IRegister 接口的对象映射配置
            if (assemblies != null && assemblies.Length > 0) config.Scan(assemblies);

            // 配置默认全局映射（支持覆盖）
            config.Default
                  .NameMatchingStrategy(NameMatchingStrategy.Flexible)
                  .PreserveReference(true);

            // 配置默认全局映射（忽略大小写敏感）
            config.Default
                  .NameMatchingStrategy(NameMatchingStrategy.IgnoreCase)
                  .PreserveReference(true);

            // 配置支持依赖注入
            services.AddSingleton(config);
            services.AddScoped<IMapper, ServiceMapper>();

            return services;
        }

        /// <summary>
        /// 配置注册监控
        /// </summary>
        public static void AddMiniProfiler(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddMiniProfiler(options =>
            {
                options.RouteBasePath = "/profiler";
            }
           );
        }

        /// <summary>
        /// 配置注册限流依赖的服务
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddIpRateLimiting(this IServiceCollection services)
        {
            //加载配置
            services.AddOptions();
            //从IpRateLimiting.json获取相应配置
            services.Configure<IpRateLimitOptions>(Appsettings.IpRateLimitingConfig);
            services.Configure<IpRateLimitPolicies>(Appsettings.IpRateLimitPoliciesConfig);
            services.AddDistributedRateLimiting();
            //配置（计数器密钥生成器）
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

            return services;
        }

        /// <summary>
        /// 配置注册EasyCaching
        /// </summary>
        public static IServiceCollection AddEasyCaching(this IServiceCollection services)
        {
            services.AddEasyCaching(ecops =>
                 ecops.UseFreeRedis(frops =>
                 {
                     frops.DBConfig = new FreeRedisDBOptions
                     {
                         ConnectionStrings = new List<ConnectionStringBuilder>
                         {
                            Appsettings.RedisCon
                         }
                     };
                 }).UseRedisLock().WithSystemTextJson());
            return services;
        }
        /// <summary>
        /// 配置注册跨域
        /// </summary>
        /// <param name="services"></param>
        public static void AddCorsConfig(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(Appsettings.Cors.CorsName, builder =>
                {
                    builder
                        .WithOrigins(
                            Appsettings.Cors
                                      .CorsOrigins
                                      .Split(",", StringSplitOptions.RemoveEmptyEntries)
                                      .ToArray()
                        )
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });
        }
    }

}
