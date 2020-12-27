/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Extensions.Configuration
*   文件名称 ：RepositoryModule.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2020-12-27 17:38:29
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using Autofac;
using Memoyu.Infrastructure.IRepositories;
using Memoyu.Infrastructure.Repositories;
using System.Reflection;

namespace Memoyu.Extensions.Configuration
{
    public class RepositoryModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            Assembly assemblysRepository = Assembly.Load("Memoyu.Infrastructure");
            builder.RegisterAssemblyTypes(assemblysRepository)
                    .Where(a => a.Name.EndsWith("Repository"))
                    .AsImplementedInterfaces()
                    .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(AuditBaseRepository<>)).As(typeof(IAuditBaseRepository<>)).InstancePerLifetimeScope();//注册泛型仓储
            builder.RegisterGeneric(typeof(AuditBaseRepository<,>)).As(typeof(IAuditBaseRepository<,>)).InstancePerLifetimeScope();

        }
    }
}
