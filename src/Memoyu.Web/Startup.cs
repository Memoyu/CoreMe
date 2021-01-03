using Autofac;
using Memoyu.Extensions.Configuration;
using Memoyu.Extensions.Middleware.Mid;
using Memoyu.Extensions.ServiceExtensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Memoyu.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddController();//配置注册Controller
            services.AddSwagger();//配置注册Swagger
            services.AddCap(Configuration);//配置CAP
            services.AddAutoMapper();//配置实体映射
            services.AddCsRedisCore(Configuration);//配置注册Redis缓存
            services.AddMiniProfiler();//配置注册监控
            services.AddIpRateLimiting(Configuration);//配置注册限流
            services.AddHealthChecks();//配置注册健康检查
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new AutofacModule(Configuration));//注入一些杂项
            builder.RegisterModule(new RepositoryModule());//注入仓储
            builder.RegisterModule(new ServiceModule());//注入服务
            builder.RegisterModule(new DependencyModule());//自动注入，类似Abp中的继承对应的接口就会注入对应接口的生命周期
            builder.RegisterModule(new FreeSqlModule(Configuration));//注入FreeSql
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Memoyu.Web v1"));
            }
            // Ip限流
            app.UseIpLimitMilddleware(Configuration);

            // 记录ip请求
            app.UseMiddleware<IPLogMilddleware>();


            app.UseRouting();


            app.UseAuthorization();

            // 性能分析
            app.UseMiniProfiler();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });
        }
    }
}
