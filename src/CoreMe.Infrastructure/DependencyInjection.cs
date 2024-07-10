using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Serilog.Events;
using Serilog;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System.Configuration;
using IP2Region.Net.Abstractions;
using IP2Region.Net.XDB;
using Microsoft.AspNetCore.Hosting;
using CoreMe.Infrastructure.Region;
using EasyCaching.FreeRedis;
using EasyCaching.Serialization.SystemTextJson.Configurations;
using Microsoft.Extensions.Caching.Distributed;
using CoreMe.Infrastructure.Persistence.Cache;
using CoreMe.Application.Common.Interfaces.Services.Region;

namespace CoreMe.Infrastructure;

/// <summary>
/// Infrastructure 依赖注入
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddAuthorization() // 注册认证
            .AddAuthentication() // 注册授权
            .AddPersistenceForMyql(configuration) // 注册MySql数据持久化组件（FreeSql）
            .AddPersistenceForMongo(configuration) // 注册MongoDb持久化组件（MongoDB.Driver）
            .AddAddEasyCaching(configuration) // 注册缓存组件
            .AddIp2Region() // 注册IP地址定位
            .AddSignalR(); // 注册 SignalR 服务

        return services;
    }

    /// <summary>
    /// 配置Serilog日志组件
    /// </summary>
    /// <returns></returns>
    public static WebApplicationBuilder AddSerilog(this WebApplicationBuilder builder)
    {
        var mongoOptions = builder.Configuration.GetSection(AppConst.MongoSection)?.Get<MongoOptions>() ?? throw new ConfigurationErrorsException(AppConst.MongoSection + " is not null");

        builder.Logging.ClearProviders();
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console(LogEventLevel.Verbose, "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} <s:{SourceContext}>{NewLine}{Exception}")
            .WriteTo.MongoDBBson(cfg =>
            {
                var mongodb = new MongoClient(mongoOptions.ConnectionString).GetDatabase(mongoOptions.Database);
                cfg.SetMongoDatabase(mongodb);
                cfg.SetCollectionName(AppConst.SystemLogCollectionName);
            }, LogEventLevel.Information)
            .Enrich.FromLogContext()
#if !DEBUG
            // 配置日志等级
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("System", LogEventLevel.Information)
#endif
            .CreateLogger();
        builder.Logging.AddSerilog();

        return builder;
    }

    private static IServiceCollection AddAuthorization(this IServiceCollection services)
    {
        services.AddScoped<IAuthorizationService, AuthorizationService>();
        services.AddScoped<ICurrentUserProvider, CurrentUserProvider>();
        // services.AddSingleton<IPolicyEnforcer, PolicyEnforcer>();

        return services;
    }

    private static IServiceCollection AddAuthentication(this IServiceCollection services)
    {
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

        services
            .ConfigureOptions<JwtBearerTokenValidationConfiguration>()
            .AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>//配置JWT
            {
                options.Events = new JwtBearerEvents()
                {
                    OnAuthenticationFailed = context =>
                    {
                        //Token 过期
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Append("Token-Expired", "true");
                        }

                        return Task.CompletedTask;
                    },
                    OnChallenge = async context =>
                    {
                        context.HandleResponse();

                        string message;
                        ResultCode code;

                        if (context.Error == "invalid_token"
                            && !string.IsNullOrWhiteSpace(context.ErrorDescription)
                            && context.ErrorDescription.StartsWith("The token expired at"))//Token过期
                        {
                            message = "令牌过期";
                            code = ResultCode.TokenExpired;
                        }
                        else if (context.Error == "invalid_token"
                            && context.ErrorDescription.IsNullOrEmpty())//Token失效
                        {
                            message = "令牌失效";
                            code = ResultCode.TokenInvalidation;
                        }
                        else
                        {
                            message = "认证失败，请先登录！";
                            code = ResultCode.AuthenticationFailure;
                        }

                        context.Response.ContentType = "application/json";
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        await context.Response.WriteAsync(Result.Failure(message, code).ToString()!);
                    },

                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];

                        // If the request is for our hub...
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments(HubConst.ManagementHubEndpointRoute))
                        {
                            // Read the token out of the query string
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };
            });

        return services;
    }

    /// <summary>
    /// 注册IP地址定位
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddIp2Region(this IServiceCollection services)
    {
        var sp = services.BuildServiceProvider();
        var env = sp.GetRequiredService<IWebHostEnvironment>();
        string xdbPath = Path.Combine(env.WebRootPath, "Assets", "ip2region.xdb");

        services.AddSingleton<ISearcher>(new Searcher(CachePolicy.Content, xdbPath));
        services.AddSingleton<IRegionSearchService, RegionSearchService>();

        return services;
    }

    /// <summary>
    /// 注册缓存组件
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    private static IServiceCollection AddAddEasyCaching(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Redis") ?? string.Empty;
        services.AddEasyCaching(options => options.UseFreeRedis(frops =>
        {
            frops.DBConfig = new FreeRedisDBOptions
            {
                ConnectionStrings = [connectionString],
            };
            frops.SerializerName = "system-text-json";
        }).WithSystemTextJson("system-text-json"));

        // 注册EasyCaching分布式缓存
        services.AddSingleton<IDistributedCache, EasyCachingRedisDistributedCache>();

        return services;
    }
}
