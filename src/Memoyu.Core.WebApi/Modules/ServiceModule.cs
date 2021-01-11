﻿/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Core.WebApi.Modules
*   文件名称 ：ServiceModule.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-03 14:44:52
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using Autofac;
using Autofac.Extras.DynamicProxy;
using Memoyu.Core.Application.Core.Account;
using Memoyu.Core.Application.Core.Account.Impl;
using Memoyu.Core.WebApi.Aop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Memoyu.Core.WebApi.Modules
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

            string[] notIncludes = new string[]
            {
                typeof(IdentityServer4Service).Name,
                typeof(JwtTokenService).Name,
            };

            Assembly servicesDllFile = Assembly.Load("Memoyu.Core.Application");
            builder.RegisterAssemblyTypes(servicesDllFile)
                .Where(a => a.Name.EndsWith("Service") && !notIncludes.Where(r => r == a.Name).Any() && !a.IsAbstract && !a.IsInterface && a.IsPublic)//!notIncludes.Where(r => r == a.Name).Any() &&
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .PropertiesAutowired()// 属性注入
                .InterceptedBy(interceptorServiceTypes.ToArray())
                .EnableInterfaceInterceptors();

            //使用名称进行实现注册
            builder.RegisterType<IdentityServer4Service>().Named<ITokenService>(typeof(IdentityServer4Service).Name).InstancePerLifetimeScope();
            builder.RegisterType<JwtTokenService>().Named<ITokenService>(typeof(JwtTokenService).Name).InstancePerLifetimeScope();
        }
    }
}
