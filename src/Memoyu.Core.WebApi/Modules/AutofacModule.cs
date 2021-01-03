/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Core.WebApi.Modules
*   文件名称 ：AutofacModule.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-03 14:43:08
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/

using Autofac;
using Memoyu.Core.Domain.Shared.Security;
using Memoyu.Core.Domain.Shared.Security.Impl;
using Microsoft.AspNetCore.Http;

namespace Memoyu.Core.WebApi.Modules
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>().SingleInstance();

            builder.RegisterType<CurrentUser>().As<ICurrentUser>().InstancePerDependency();
        }
    }
}
