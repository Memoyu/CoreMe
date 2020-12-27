/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Extensions.Configuration
*   文件名称 ：DependencyModule.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2020-12-27 17:40:45
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using Autofac;
using Memoyu.Extensions.Dependency;
using System;
using System.Linq;
using System.Reflection;

namespace Memoyu.Extensions.Configuration
{
    public class DependencyModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            Assembly[] currentAssemblies = AppDomain.CurrentDomain.GetAssemblies().Where(r => r.FullName.Contains("Memoyu.")).ToArray();

            //每次调用，都会重新实例化对象；每次请求都创建一个新的对象；
            Type transientDependency = typeof(ITransientDependency);
            builder.RegisterAssemblyTypes(currentAssemblies)
                .Where(t => transientDependency.GetTypeInfo().IsAssignableFrom(t) && t.IsClass && !t.IsAbstract && !t.IsGenericType)
                .AsImplementedInterfaces().InstancePerDependency();

            //同一个Lifetime生成的对象是同一个实例
            Type scopeDependency = typeof(IScopedDependency);
            builder.RegisterAssemblyTypes(currentAssemblies)
                .Where(t => scopeDependency.GetTypeInfo().IsAssignableFrom(t) && t.IsClass && !t.IsAbstract && !t.IsGenericType)
                .AsImplementedInterfaces().InstancePerLifetimeScope();

            //单例模式，每次调用，都会使用同一个实例化的对象；每次都用同一个对象；
            Type singletonDependency = typeof(ISingletonDependency);
            builder.RegisterAssemblyTypes(currentAssemblies)
                .Where(t => singletonDependency.GetTypeInfo().IsAssignableFrom(t) && t.IsClass && !t.IsAbstract && !t.IsGenericType)
                .AsImplementedInterfaces().SingleInstance();

        }
    }
}
