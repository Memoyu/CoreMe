using Memoyu.Core.Common.Helper.LogHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Memoyu.Extensions.Middleware.Mid
{
    /// <summary>
    /// 中间件：记录请求IP
    /// </summary>
    public class IPLogMilddleware
    {
        //***************请求代理需要先注入IHttpContextAccessor，否者报错********************//
        private readonly RequestDelegate _requestDelegate;
        private readonly IConfiguration _configuration;

        public IPLogMilddleware(RequestDelegate requestDelegate, IConfiguration configuration)
        {
            _requestDelegate = requestDelegate;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var isEnabled = bool.Parse(_configuration.GetSection("Middleware:IPLog:Enabled").Value);

            if (isEnabled)
            {
                // 过滤，只有接口
                if (context.Request.Path.Value.Contains("api"))
                {
                    context.Request.EnableBuffering();

                    try
                    {
                        // 存储请求数据
                        var request = context.Request;
                        var requestInfo = JsonConvert.SerializeObject(new RequestInfo()
                        {
                            Ip = GetClientIP(context),
                            Url = request.Path.ToString().Trim().TrimEnd('/').ToLower(),
                            Datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                            Date = DateTime.Now.ToString("yyyy-MM-dd"),
                            Week = GetWeek(),
                        });

                        if (!string.IsNullOrEmpty(requestInfo))
                        {
                            // 自定义log输出
                            Parallel.For(0, 1, e =>
                            {
                                LogHelper.WriteLog("RequestIpInfoLog", new string[] { requestInfo + "," }, false);
                            });

                            request.Body.Position = 0;
                        }

                        await _requestDelegate(context);
                    }
                    catch (Exception)
                    {
                    }
                }
                else
                {
                    await _requestDelegate(context);
                }
            }
            else
            {
                await _requestDelegate(context);
            }
        }

        /// <summary>
        /// 获取当前星期
        /// </summary>
        /// <returns></returns>
        private string GetWeek()
        {
            string week = string.Empty;
            switch (DateTime.Now.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    week = "周一";
                    break;
                case DayOfWeek.Tuesday:
                    week = "周二";
                    break;
                case DayOfWeek.Wednesday:
                    week = "周三";
                    break;
                case DayOfWeek.Thursday:
                    week = "周四";
                    break;
                case DayOfWeek.Friday:
                    week = "周五";
                    break;
                case DayOfWeek.Saturday:
                    week = "周六";
                    break;
                case DayOfWeek.Sunday:
                    week = "周日";
                    break;
                default:
                    week = "N/A";
                    break;
            }
            return week;
        }

        /// <summary>
        /// 获取客户端IP
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetClientIP(HttpContext context)
        {
            var ip = context.Request.Headers["X-Forwarded-For"].ToString();
            if (string.IsNullOrEmpty(ip))
            {
                ip = context.Connection.RemoteIpAddress.ToString();
            }
            return ip;
        }

    }
}
