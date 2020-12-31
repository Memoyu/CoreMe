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
            
            services.AddController();//ÅäÖÃ×¢²áController
            services.AddSwagger();//ÅäÖÃ×¢²áSwagger
            services.AddCsRedisCore(Configuration);//ÅäÖÃ×¢²áRedis»º´æ
            services.AddMiniProfilerSetup();//ÅäÖÃ×¢²á¼à¿Ø
            services.AddHttpContext();//ÅäÖÃ×¢²áHttpContext
            services.AddIpRateLimiting(Configuration);//ÅäÖÃ×¢²áÏÞÁ÷
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new RepositoryModule());
            builder.RegisterModule(new ServiceModule());
            builder.RegisterModule(new AutofacModule(Configuration));
            builder.RegisterModule(new DependencyModule());
            builder.RegisterModule(new FreeSqlModule(Configuration));
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Memoyu.Web v1"));
            }
            // IpÏÞÁ÷
            app.UseIpLimitMilddleware(Configuration);

            // ¼ÇÂ¼ipÇëÇó
            app.UseMiddleware<IPLogMilddleware>();

            app.UseRouting();

            app.UseAuthorization();

            // ÐÔÄÜ·ÖÎö
            app.UseMiniProfiler();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
