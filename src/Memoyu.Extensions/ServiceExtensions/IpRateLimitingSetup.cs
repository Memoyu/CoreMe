/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Extensions.ServiceExtensions
*   文件名称 ：IpRateLimitingSetup.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2020-12-27 17:23:35
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using AspNetCoreRateLimit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Memoyu.Extensions.ServiceExtensions
{
    public static  class IpRateLimitingSetup
    {
        /// <summary>
        /// 配置限流依赖的服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddIpRateLimiting(this IServiceCollection services, IConfiguration configuration)
        {
            //加载配置
            services.AddOptions();
            //从IpRateLimiting.json获取相应配置
            services.Configure<IpRateLimitOptions>(configuration.GetSection("IpRateLimiting"));
            services.Configure<IpRateLimitPolicies>(configuration.GetSection("IpRateLimitPolicies"));
            //注入计数器和规则存储
            services.AddSingleton<IIpPolicyStore, DistributedCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitCounterStore, DistributedCacheRateLimitCounterStore>();
            //配置（计数器密钥生成器）
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

            return services;
        }

    }
}
