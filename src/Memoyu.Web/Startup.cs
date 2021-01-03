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
            services.AddController();//����ע��Controller
            services.AddSwagger();//����ע��Swagger
            services.AddCap(Configuration);//����CAP
            services.AddAutoMapper();//����ʵ��ӳ��
            services.AddCsRedisCore(Configuration);//����ע��Redis����
            services.AddMiniProfiler();//����ע����
            services.AddIpRateLimiting(Configuration);//����ע������
            services.AddHealthChecks();//����ע�ὡ�����
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new AutofacModule(Configuration));//ע��һЩ����
            builder.RegisterModule(new RepositoryModule());//ע��ִ�
            builder.RegisterModule(new ServiceModule());//ע�����
            builder.RegisterModule(new DependencyModule());//�Զ�ע�룬����Abp�еļ̳ж�Ӧ�Ľӿھͻ�ע���Ӧ�ӿڵ���������
            builder.RegisterModule(new FreeSqlModule(Configuration));//ע��FreeSql
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
            app.UseIpLimitMilddleware(Configuration);

            // ��¼ip����
            app.UseMiddleware<IPLogMilddleware>();


            app.UseRouting();


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
