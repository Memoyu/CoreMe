using Autofac;
using Memoyu.Core.WebApi.Extensions;
using Memoyu.Core.WebApi.Middleware;
using Memoyu.Core.WebApi.Modules;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Memoyu.Core.WebApi
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
            services.AddSwagger();//����ע��Swagger
            services.AddCap();//����CAP
            services.AddAutoMapper();//����ʵ��ӳ��
            services.AddCsRedisCore();//����ע��Redis����
            services.AddMiniProfiler();//����ע����
            services.AddIpRateLimiting();//����ע������
            services.AddHealthChecks();//����ע�ὡ�����
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new AutofacModule());//ע��һЩ����
            builder.RegisterModule(new RepositoryModule());//ע��ִ�
            builder.RegisterModule(new ServiceModule());//ע�����
            builder.RegisterModule(new DependencyModule());//�Զ�ע�룬����Abp�еļ̳ж�Ӧ�Ľӿھͻ�ע���Ӧ�ӿڵ���������
            builder.RegisterModule(new FreeSqlModule());//ע��FreeSql
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Memoyu.Web v1"));
            }
            // Ip����
            app.UseIpLimitMilddleware();

            // ��¼ip����
            app.UseMiddleware<IPLogMilddleware>();


            app.UseRouting();

            ////�쳣�����м��
            //app.UseMiddleware<ExceptionHandlerMiddleware>();

            app.UseAuthorization();

            // ���ܷ���
            app.UseMiniProfiler();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });
        }
    }
}