using AspNetCoreRateLimit;
using Autofac.Extensions.DependencyInjection;
using Memoyu.Core.Domain.Shared.Configurations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Memoyu.Core.WebApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
#if DEBUG
            Serilog.Debugging.SelfLog.Enable(msg =>
            Debug.WriteLine(msg)
            );
#endif

            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(AppSettings.Configuration)
                .Enrich.FromLogContext()
                .CreateLogger();

            try
            {
                Log.Information("init main");
                IHost webHost = CreateHostBuilder(args).Build();
                try
                {
                    using var scope = webHost.Services.CreateScope();
                    // get the IpPolicyStore instance
                    var ipPolicyStore = scope.ServiceProvider.GetRequiredService<IIpPolicyStore>();
                    // seed IP data from appsettings
                    await ipPolicyStore.SeedAsync();
                }
                catch (Exception ex)
                {
                    Log.Fatal(ex, "IIpPolicyStore RUN Error");
                }
                await webHost.RunAsync();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())//添加Autofac服务工厂
                .ConfigureWebHostDefaults(webBuilder =>
                    {
                        webBuilder.UseStartup<Startup>();
                    })
                .UseSerilog();//构建Serilog;
    }
}
