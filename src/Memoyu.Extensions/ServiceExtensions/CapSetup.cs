/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Extensions.ServiceExtensions
*   文件名称 ：CapSetup.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-01 14:22:14
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using DotNetCore.CAP;
using DotNetCore.CAP.Messages;
using Serilog;
using Memoyu.Core.Data.Enums;
using Savorboard.CAP.InMemoryMessageQueue;

namespace Memoyu.Extensions.ServiceExtensions
{
    /// <summary>
    /// 配置注册CAP
    /// </summary>
    public static class CapSetup
    {
        public static IServiceCollection AddCap(this IServiceCollection services, IConfiguration Configuration)
        {

            services.AddCap(x =>
            {
                try
                {
                    x.UseCapOptions(Configuration);
                }
                catch (Exception ex)
                {
                    Log.Error(ex.Message);
                    throw;
                }
                x.UseDashboard();
                x.FailedRetryCount = 5;
                x.FailedThresholdCallback = (type) =>
                {
                    Log.Error($@"A message of type {type} failed after executing {x.FailedRetryCount} several times, requiring manual troubleshooting. Message name: {type.Message.GetName()}");
                };
            });

            return services;
        }

        /// <summary>
        /// 根据配置文件配置Cap
        /// </summary>
        /// <param name="options"></param>
        /// <param name="Configuration"></param>
        /// <returns></returns>
        private static CapOptions UseCapOptions(this CapOptions options, IConfiguration Configuration)
        {
            IConfigurationSection defaultStorage = Configuration.GetSection("CAP:DefaultStorage");
            IConfigurationSection defaultMessageQueue = Configuration.GetSection("CAP:DefaultMessageQueue");

            //配置Cap默认存储类型
            if (Enum.TryParse(defaultStorage.Value, out CapStorageTypeEnums capStorageType))
            {
                if (!Enum.IsDefined(typeof(CapStorageTypeEnums), capStorageType))//枚举中是否存在该类型定义
                {
                    Log.Error($"CAP配置:DefaultStorage:{defaultStorage.Value}无效");
                }

                switch (capStorageType)
                {
                    case CapStorageTypeEnums.InMemoryStorage:
                        options.UseInMemoryStorage();
                        break;
                    case CapStorageTypeEnums.Mysql:
                        IConfigurationSection mySql = Configuration.GetSection($"ConnectionStrings:MySql");
                        options.UseMySql(mySql.Value);
                        break;
                    default:
                        break;
                }

            }
            else
            {
                Log.Error($"CAP配置:DefaultStorage:{capStorageType}配置无效，仅支持InMemoryStorage，Mysql！更多请增加引用，修改配置项代码");
            }
            //配置Cap默认消息队列
            if (Enum.TryParse(defaultMessageQueue.Value, out CapMessageQueueTypeEnums capMessageQueueType))
            {
                if (!Enum.IsDefined(typeof(CapMessageQueueTypeEnums), capMessageQueueType))//枚举中是否存在该类型定义
                {
                    Log.Error($"CAP配置:DefaultMessageQueue:{defaultMessageQueue.Value}无效");
                }
                //IConfigurationSection configurationSection = Configuration.GetSection($"ConnectionStrings:{capMessageQueueType}");

                switch (capMessageQueueType)
                {
                    case CapMessageQueueTypeEnums.InMemoryQueue:
                        options.UseInMemoryMessageQueue();
                        break;
                    case CapMessageQueueTypeEnums.RabbitMQ:
                        options.UseRabbitMQ(options =>
                        {
                            options.HostName = Configuration["CAP:RabbitMQ:HostName"];
                            options.UserName = Configuration["CAP:RabbitMQ:UserName"];
                            options.Password = Configuration["CAP:RabbitMQ:Password"];
                            options.VirtualHost = Configuration["CAP:RabbitMQ:VirtualHost"];
                        });
                        break;
                    default:
                        break;
                }
            }
            else
            {
                Log.Error($"CAP配置:DefaultMessageQueue:{defaultMessageQueue.Value}无效");
            }

            return options ;
        }
    }
}
