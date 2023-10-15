using Autofac;
using CoreMe.Core.AOP.Middleware;
using CoreMe.Core.Common.Configs;
using CoreMe.Core.Extensions.ServiceCollection;
using CoreMe.Modules;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace CoreMe
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
            services.AddController();//����ע��Controller
            services.AddJwtBearer();//����Jwt
            services.AddSwagger();//����ע��Swagger
            services.AddAutoMapper();//����ʵ��ӳ��
            services.AddCsRedisCore();//����ע��Redis����
            services.AddMiniProfiler();//����ע����
            services.AddIpRateLimiting();//����ע������
            services.AddHealthChecks();//����ע�ὡ�����
            services.AddCorsConfig();//���ÿ���
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new AutofacModule());//ע��һЩ����
            builder.RegisterModule(new RepositoryModule());//ע��ִ�
            builder.RegisterModule(new ServiceModule());//ע�����
            builder.RegisterModule(new DependencyModule());//�Զ�ע�ᣬ����Abp�еļ̳ж�Ӧ�Ľӿھͻ�ע���Ӧ�ӿڵ���������
            builder.RegisterModule(new FreeSqlModule());//ע��FreeSql
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger().UseSwaggerUI();
            }
            //����
            app.UseCors(Appsettings.Cors.CorsName);

            //��̬�ļ�
            app.UseStaticFiles();

            // Ip����
            app.UseMiddleware<IpLimitMiddleware>();

            // ��¼ip����
            app.UseMiddleware<IPLogMilddleware>();

            ////�쳣�����м��
            //app.UseMiddleware<ExceptionHandlerMiddleware>();

            //��֤�м��
            app.UseAuthentication();

            // ���ܷ���
            app.UseMiniProfiler();

            app.UseRouting()
                .UseAuthorization()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                    endpoints.MapHealthChecks("/health");
                });
        }
    }
}
