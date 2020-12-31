using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;

namespace Memoyu.Extensions.Middleware.Mid
{
    public static class IpLimitMilddleware
    {
        public static void UseIpLimitMilddleware(this IApplicationBuilder app, IConfiguration configuration)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));

            try
            {
                var isEnabled = bool.Parse(configuration.GetSection("Middleware:IpRateLimit:Enabled").Value);
                if (isEnabled)
                {
                    app.UseIpRateLimiting();
                }
            }
            catch (Exception e)
            {
                Log.Error($"添加IP限流发生异常.\n{e.Message}");
                throw;
            }
        }
    }
}
