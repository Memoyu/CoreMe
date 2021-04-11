﻿using CoreMe.Core.Common.Configs;
using CoreMe.Core.Domains.Common;
using CoreMe.Core.Extensions;
using Microsoft.AspNetCore.Http;
using Serilog;
using Serilog.Events;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CoreMe.Core.AOP.Middleware
{
    /// <summary>
    /// 中间件：记录请求IP
    /// </summary>
    public class IPLogMilddleware
    {
        //***************请求代理需要先注入IHttpContextAccessor，否者报错********************//
        private readonly RequestDelegate _requestDelegate;

        public IPLogMilddleware(RequestDelegate requestDelegate)
        {
            _requestDelegate = requestDelegate;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var isEnabled = Appsettings.IpLogEnable;

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
                        var requestInfo = new RequestInfo()
                        {
                            Ip = GetClientIP(context),
                            Url = request.Path.ToString().Trim().TrimEnd('/').ToLower(),
                            Datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                            Date = DateTime.Now.ToString("yyyy-MM-dd"),
                            Week = GetWeek(),
                        }.ToJson();

                        if (!string.IsNullOrEmpty(requestInfo))
                        {
                            // 自定义log输出
                            Parallel.For(0, 1, e =>
                            {
                                WriteLog("RequestIpInfoLog", new string[] { requestInfo + "," }, false);
                            });

                            request.Body.Position = 0;
                        }

                        await _requestDelegate(context);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
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
        /// 获取客户端IP
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private static string GetClientIP(HttpContext context)
        {
            var ip = context.Request.Headers["X-Forwarded-For"].ToString();
            if (string.IsNullOrEmpty(ip))
            {
                ip = context.Connection.RemoteIpAddress.ToString();
            }
            return ip;
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
        /// 记录日志
        /// </summary>
        /// <param name="filename">写入日志文件名</param>
        /// <param name="messages">写入信息</param>
        /// <param name="IsHeader">是否加头部分割线</param>
        private static void WriteLog(string filename, string[] messages, bool IsHeader = true)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
                .WriteTo.File(Path.Combine($"Logs/Serilog/", $"{filename}.log"), rollingInterval: RollingInterval.Infinite, outputTemplate: "{Message}{NewLine}{Exception}")
                .CreateLogger();

            var now = DateTime.Now;
            string logContent = String.Join("\r\n", messages);
            if (IsHeader)
            {
                logContent = (
                   "--------------------------------\r\n" +
                   DateTime.Now + "|\r\n" +
                   String.Join("\r\n", messages) + "\r\n"
                   );
            }

            Log.Information(logContent);
            Log.CloseAndFlush();
        }
    }
}
