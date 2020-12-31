/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Extensions.ServiceExtensions
*   文件名称 ：CsRedisCoreSetup.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2020-12-27 18:54:43
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using CSRedis;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Memoyu.Extensions.ServiceExtensions
{
    /// <summary>
    /// 配置注册CSRedis
    /// </summary>
    public static class CsRedisCoreSetup
    {
        public static IServiceCollection AddCsRedisCore(this IServiceCollection services, IConfiguration configuration)
        {

            IConfigurationSection csRediSection = configuration.GetSection("ConnectionStrings:CsRedis");
            CSRedisClient csRedisClient = new CSRedisClient(csRediSection.Value);
            //初始化 RedisHelper
            RedisHelper.Initialization(csRedisClient);

            //注册mvc分布式缓存
            services.AddSingleton<IDistributedCache>(new CSRedisCache(RedisHelper.Instance));
            return services;
        }
    }
}
