/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Sr.Manager.WebApi.Extensions
*   文件名称 ：ControllerSetup.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-03 13:24:04
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using Sr.Manager.ToolKits.Base;
using Sr.Manager.ToolKits.Base.Enum.Base;
using Sr.Manager.WebApi.Aop.Filter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Sr.Manager.WebApi.Extensions
{
    /// <summary>
    /// 控制器配置注册
    /// </summary>
    public static class ControllerSetup
    {
        public static IServiceCollection AddController(this IServiceCollection services)
        {
            services.AddControllers(options =>
                {
                    options.Filters.Add<LocalExceptionFilter>();
                })
                .AddNewtonsoftJson(opt =>
                {
                    //opt.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:MM:ss";
                    //设置自定义时间戳格式
                    //opt.SerializerSettings.Converters = new List<JsonConverter>()
                    //{
                    //    new LinCmsTimeConverter()
                    //};
                    // 设置下划线方式，首字母是小写
                    //opt.SerializerSettings.ContractResolver = new DefaultContractResolver()
                    //{
                    //    NamingStrategy = new SnakeCaseNamingStrategy()
                    //    {
                    //        ProcessDictionaryKeys = true
                    //    }
                    //};
                })
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.SuppressConsumesConstraintForFormFileParameters = true;
                    //自定义 BadRequest 响应
                    options.InvalidModelStateResponseFactory = context =>
                    {
                        var problemDetails = new ValidationProblemDetails(context.ModelState);

                        var resultDto = new ServiceResult
                        {
                            Code = ServiceResultCode.ParameterError,
                            Message = problemDetails.Errors
                        };

                        return new BadRequestObjectResult(resultDto)
                        {
                            ContentTypes = { "application/json" }
                        };
                    };
                });
            return services;
        }
    }
}
