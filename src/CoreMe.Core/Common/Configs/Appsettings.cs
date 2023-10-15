﻿using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace CoreMe.Core.Common.Configs
{

    public class Appsettings
    {
        private static readonly IConfigurationRoot _configuration;
        static Appsettings()
        {
            _configuration = new ConfigurationBuilder()//配置配置文件
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"Configs/SerilogConfig.json", optional: true, reloadOnChange: true)//添加Serilog配置
                .AddJsonFile($"Configs/RateLimitConfig.json", optional: true, reloadOnChange: true)//添加限流配置
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
                .Build();
        }

        #region System

        public static IConfiguration Configuration => _configuration;

        /// <summary>
        /// 接口版本
        /// </summary>
        public static string ApiVersion => _configuration["ApiVersion"];

        #endregion

        #region Cors

        public static class Cors
        {
            /// <summary>
            /// 跨域策略名
            /// </summary>
            public static string CorsName => _configuration["Cors:Name"];
            /// <summary>
            /// 跨域源
            /// </summary>
            public static string CorsOrigins => _configuration["Cors:Origins"];
        }

        #endregion

        #region FileStorage

        public static class FileStorage
        {
            /// <summary>
            /// 上传文件总大小
            /// </summary>
            public static long MaxFileSize => long.Parse(_configuration["FileStorage:MaxFileSize"]);
            /// <summary>
            /// 多文件上传时，支持的最大文件数量
            /// </summary>
            public static int NumLimit => int.Parse(_configuration["FileStorage:NumLimit"]);
            /// <summary>
            /// 允许某些类型文件上传，文件格式以,隔开
            /// </summary>
            public static string Include => _configuration["FileStorage:Include"];
            /// <summary>
            /// 禁止某些类型文件上传，文件格式以,隔开
            /// </summary>
            public static string Exclude => _configuration["FileStorage:Exclude"];
            /// <summary>
            /// 服务名
            /// </summary>
            public static string ServiceName => _configuration["FileStorage:ServiceName"];

            /// <summary>
            /// 本地文件信息
            /// </summary>
            public static string LocalFilePrefixPath => _configuration["FileStorage:LocalFile:PrefixPath"];
            public static string LocalFileHost => _configuration["FileStorage:LocalFile:Host"];
        }

        #endregion

        #region Authentication

        /// <summary>
        /// 是否开启IdentityServer4
        /// </summary>
        public static bool IdentityServer4Enable => Convert.ToBoolean(_configuration["Service:UseIdentityServer4"] ?? "false");

        /// <summary>
        /// Ids4 服务地址
        /// </summary>
        public static string Authority => _configuration["Service:Authority"];

        /// <summary>
        /// 是否使用Https
        /// </summary>
        public static bool IsUseHttps => Convert.ToBoolean(_configuration["Service:UseHttps"]);

        /// <summary>
        /// ClientName
        /// </summary>
        public static string ClientName => _configuration["Service:ClientName"];


        /// <summary>
        /// Jwt Token Config
        /// </summary>
        public class JwtBearer
        {
            /// <summary>
            /// 密钥
            /// </summary>
            public static string SecurityKey => _configuration["Authentication:JwtBearer:SecurityKey"];

            /// <summary>
            /// Audience
            /// </summary>
            public static string Audience => _configuration["Authentication:JwtBearer:Audience"];

            /// <summary>
            /// 过期时间(分钟)
            /// </summary>
            public static double Expires => Convert.ToDouble(_configuration["Authentication:JwtBearer:Expires"]);

            /// <summary>
            /// 签发者
            /// </summary>
            public static string Issuer => _configuration["Authentication:JwtBearer:Issuer"];
        }


        #endregion

        #region Db

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
        /// 缓存过期时间 单位：秒
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
        public static string RedisCon => _configuration["ConnectionStrings:Redis"];

        #endregion

    }
}
