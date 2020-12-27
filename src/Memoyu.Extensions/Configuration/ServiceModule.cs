/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Extensions.Configuration
*   文件名称 ：ServiceModule.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2020-12-27 17:39:10
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using Autofac;
using Autofac.Extras.DynamicProxy;
using Memoyu.Extensions.Middleware;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Memoyu.Extensions.Configuration
{
    public class ServiceModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UnitOfWorkAsyncInterceptor>();
            builder.RegisterType<UnitOfWorkInterceptor>();

            builder.RegisterType<CacheIntercept>();

            List<Type> interceptorServiceTypes = new List<Type>()
            {
                typeof(UnitOfWorkInterceptor),
                typeof(CacheIntercept),
            };

            Assembly servicesDllFile = Assembly.Load("Memoyu.Application");
            builder.RegisterAssemblyTypes(servicesDllFile)
                .Where(a => a.Name.EndsWith("Service") && !a.IsAbstract && !a.IsInterface && a.IsPublic)//!notIncludes.Where(r => r == a.Name).Any() &&
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .PropertiesAutowired()// 属性注入
                .InterceptedBy(interceptorServiceTypes.ToArray())
                .EnableInterfaceInterceptors();
        }
    }
}
