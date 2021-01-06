/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Core.WebApi.Modules
*   文件名称 ：RepositoryModule.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-03 14:44:41
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using Autofac;
using Memoyu.Core.Domain.Base;
using Memoyu.Core.Domain.Base.Impl;
using System.Reflection;

namespace Memoyu.Core.WebApi.Modules
{
    public class RepositoryModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            Assembly assemblysRepository = Assembly.Load("Memoyu.Core.Domain");
            builder.RegisterAssemblyTypes(assemblysRepository)
                    .Where(a => a.Name.EndsWith("Repository"))
                    .AsImplementedInterfaces()
                    .InstancePerLifetimeScope();
             
            builder.RegisterGeneric(typeof(AuditBaseRepository<>)).As(typeof(IAuditBaseRepository<>)).InstancePerLifetimeScope();//注册泛型仓储
            builder.RegisterGeneric(typeof(AuditBaseRepository<,>)).As(typeof(IAuditBaseRepository<,>)).InstancePerLifetimeScope();

        }
    }
}
