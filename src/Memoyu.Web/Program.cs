using AspNetCoreRateLimit;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Memoyu.Web
{
    public class Program
    {
        public static IConfiguration Configuration { get; } = new ConfigurationBuilder()//配置配置文件
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"Config/SerilogConfig.json", optional: true, reloadOnChange: true)//添加Serilog配置
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

        public static async Task Main(string[] args)
        {
#if DEBUG
            Serilog.Debugging.SelfLog.Enable(msg =>
            Debug.WriteLine(msg)
            );
#endif

            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(Configuration)
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
                    webBuilder.UseStartup<Startup>()
                              .ConfigureAppConfiguration((host, config) =>
                              {
                                  config.AddJsonFile($"Config/RateLimitConfig.json", optional: true, reloadOnChange: true);//添加限流配置
                              });
                })
                .UseSerilog();//构建Serilog
    }
}
