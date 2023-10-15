﻿/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Sr.Manager.Domain.Shared.Configurations
*   文件名称 ：AppSettings.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-03 10:42:43
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace Sr.Manager.Domain.Shared.Configurations
{

    public class AppSettings
    {
        private static readonly IConfigurationRoot _configuration;
        static AppSettings()
        {
            _configuration = new ConfigurationBuilder()//配置配置文件
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"Config/SerilogConfig.json", optional: true, reloadOnChange: true)//添加Serilog配置
                .AddJsonFile($"Config/RateLimitConfig.json", optional: true, reloadOnChange: true)//添加限流配置
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
                .Build();
        }
        #region Db

        public static IConfiguration Configuration =>_configuration;

        /// <summary>
        /// 获取配置默认Db Code
        /// </summary>
        public static string DbTypeCode => _configuration["ConnectionStrings:DefaultDB"];

        /// <summary>
        /// 获取配置 MySql ConnectionString
        /// </summary>
        public static string MySqlCon => _configuration["ConnectionStrings:MySql"];

        /// <summary>
        /// 获取配置默认Db ConnectionString 
        /// </summary>
        public static string DbConnectionString(string dbTypeCode) => _configuration[$"ConnectionStrings:{dbTypeCode}"];
        #endregion

        #region Cache

        /// <summary>
        /// 是否开启Cache
        /// </summary>
        public static bool CacheEnable => Convert.ToBoolean(_configuration["Cache:Enable"]);

        /// <summary>
        /// 缓存过期时间
        /// </summary>
        public static int CacheExpire => Convert.ToInt32(_configuration["Cache:ExpireSeconds"]);

        #endregion

        #region IP

        /// <summary>
        /// 是否开启IP记录
        /// </summary>
        public static bool IpLogEnable => Convert.ToBoolean(_configuration["Middleware:IPLog:Enabled"]);

        /// <summary>
        /// 是否开启IP限流
        /// </summary>
        public static bool IpRateLimitEnable => Convert.ToBoolean(_configuration["Middleware:IpRateLimit:Enabled"]);
        public static IConfigurationSection IpRateLimitingConfig => _configuration.GetSection("IpRateLimiting");
        public static IConfigurationSection IpRateLimitPoliciesConfig => _configuration.GetSection("IpRateLimitPolicies");

        #endregion

        #region CAP

        /// <summary>
        /// Cap默认存储表前缀
        /// </summary>
        public static string CapStorageTablePrefix => _configuration["CAP:TableNamePrefix"];

        /// <summary>
        /// Cap默认存储
        /// </summary>
        public static string CapDefaultStorage => _configuration["CAP:DefaultStorage"];

        /// <summary>
        /// Cap默认队列
        /// </summary>
        public static string CapDefaultMessageQueue => _configuration["CAP:DefaultMessageQueue"];

        /// <summary>
        /// Cap RabbitMq 连接信息
        /// </summary>
        public class CapRabbitMq
        {
            public static string HostName => _configuration["CAP:RabbitMQ:HostName"];
            public static string UserName => _configuration["CAP:RabbitMQ:UserName"];
            public static string Password => _configuration["CAP:RabbitMQ:Password"];
            public static string VirtualHost => _configuration["CAP:RabbitMQ:VirtualHost"];
        }

        #endregion

        #region Redis

        /// <summary>
        /// CsRedis连接字符串
        /// </summary>
        public static string CsRedisCon => _configuration["ConnectionStrings:CsRedis"];

        #endregion

    }
}
