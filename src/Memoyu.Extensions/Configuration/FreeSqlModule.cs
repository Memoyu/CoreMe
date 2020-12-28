/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Extensions.Configuration
*   文件名称 ：FreeSqlModule.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2020-12-27 17:23:35
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using Autofac;
using FreeSql;
using FreeSql.Internal;
using Memoyu.Extensions.FreeSqlExt;
using Memoyu.Infrastructure.Common;
using Memoyu.Infrastructure.Entities;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Diagnostics;
using System.Threading;

namespace Memoyu.Extensions.Configuration
{
    public class FreeSqlModule : Module
    {
        private readonly IConfiguration _configuration;
        public FreeSqlModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            IFreeSql fsql = new FreeSqlBuilder()
              .UseConnectionString(_configuration)
              .UseNameConvert(NameConvertType.PascalCaseToUnderscoreWithLower)
              .UseAutoSyncStructure(true)
              .UseNoneCommandParameter(true)
              .UseMonitorCommand(cmd =>
              {
                  Trace.WriteLine(cmd.CommandText + ";");
              }
              )
              .CreateDatabaseIfNotExists()
              .Build()
              .SetDbContextOptions(opt =>
              {
                  opt.EnableAddOrUpdateNavigateList = true;
                  opt.OnEntityChange = rep =>
                  {
                      //进行审计
                  };
              });//联级保存功能开启（默认为关闭）

            fsql.Aop.CurdAfter += (s, e) =>
            {
                Log.Debug($"ManagedThreadId:{Thread.CurrentThread.ManagedThreadId}: FullName:{e.EntityType.FullName}" + $" ElapsedMilliseconds:{e.ElapsedMilliseconds}ms, {e.Sql}");

                if (e.ElapsedMilliseconds > 200)
                {
                    //记录日志
                    //发送短信给负责人
                }
            };
            builder.RegisterInstance(fsql).SingleInstance();//以FreeSql注册为单例
            fsql.GlobalFilter.Apply<IDeleteAduitEntity>("IsDeleted", a => a.IsDeleted == false);

            //在运行时直接生成表结构
            try
            {
                fsql.CodeFirst
                    .SyncStructure(ReflexUtil.GetTypesByTableAttribute());
            }
            catch (Exception e)
            {
                Log.Logger.Error(e + e.StackTrace + e.Message + e.InnerException);
            }
        }

      
    }
}
