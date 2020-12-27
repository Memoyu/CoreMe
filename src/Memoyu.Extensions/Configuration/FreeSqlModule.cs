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
using Microsoft.Extensions.Configuration;
using System.Diagnostics;

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
              .SetDbContextOptions(opt => opt.EnableAddOrUpdateNavigateList = true)//联级保存功能开启（默认为关闭）
              ;
        }

      
    }
}
