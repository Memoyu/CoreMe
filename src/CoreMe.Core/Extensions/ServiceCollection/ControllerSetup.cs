using CoreMe.Core.AOP.Filters;
using CoreMe.Core.Domains.Common;
using CoreMe.Core.Domains.Common.Enums.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace CoreMe.Core.Extensions.ServiceCollection
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
