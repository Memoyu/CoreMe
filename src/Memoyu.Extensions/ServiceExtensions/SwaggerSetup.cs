/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Extensions.ServiceExtensions
*   文件名称 ：SwaggerSetup.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2020-12-27 17:28:50
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Memoyu.Extensions.ServiceExtensions
{
    /// <summary>
    /// 配置注册Swagger
    /// </summary>
    public static class SwaggerSetup
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Memoyu.Web", Version = "v1" });
            });

            return services;
        }
    }
}
