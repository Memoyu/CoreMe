/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Extensions.ServiceExtensions
*   文件名称 ：AutoMapperSetup.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-01 23:51:13
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using AutoMapper;
using Memoyu.Extensions.Mapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memoyu.Extensions.ServiceExtensions
{
    /// <summary>
    /// 注册Automapper 服务
    /// </summary>
    public static class AutoMapperSetup
    {
        public static void AddAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AutoMapperConfig));
            AutoMapperConfig.RegisterMappings();
        }
    }
}
