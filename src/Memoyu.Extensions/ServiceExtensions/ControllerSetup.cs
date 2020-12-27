/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Extensions.ServiceExtensions
*   文件名称 ：ControllerSetup.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2020-12-27 17:35:58
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using Microsoft.Extensions.DependencyInjection;

namespace Memoyu.Extensions.ServiceExtensions
{
    public static class ControllerSetup
    {
        public static IServiceCollection AddController(this IServiceCollection services)
        {
            services.AddControllers();
            return services;
        }
    }
}
