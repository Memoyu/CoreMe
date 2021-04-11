using AspNetCoreRateLimit;
using CoreMe.Core.Common.Configs;
using Microsoft.AspNetCore.Builder;
using Serilog;
using System;

namespace CoreMe.Core.AOP.Middleware
{
    public static class IpLimitMilddleware
    {
        public static void UseIpLimitMilddleware(this IApplicationBuilder app)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));

            try
            {
                var isEnabled = Appsettings.IpRateLimitEnable;
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
