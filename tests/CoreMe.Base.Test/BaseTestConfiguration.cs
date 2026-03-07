using CoreMe.Application.Common.Models.Settings;
using CoreMe.Domain.Constants;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CoreMe.Base.Test
{
    public class BaseTestConfiguration
    {
        protected readonly IServiceCollection Services;

        public BaseTestConfiguration()
        {
            var services = new ServiceCollection();
            services.AddLogging();
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            Services = services;

            // 注册服务配置
            services.Configure<AppSettings>(configuration.GetSection(AppConst.AppSettingSection));
            services.Configure<AuthorizationSettings>(configuration.GetSection(AppConst.AuthorizationSection));
        }

        protected TService GetTestService<TService, TImplementation>() where TService : class where TImplementation : class, TService
        {
            Services.AddSingleton<TService, TImplementation>();
            var sp = Services.BuildServiceProvider();

            return sp.GetRequiredService<TService>();
        }
    }
}
