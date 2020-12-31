using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memoyu.Core.Common.Helper.LogHelper
{
    public class LogHelper
    {

        /// <summary>
        /// 记录日常日志
        /// </summary>
        /// <param name="filename">写入日志文件名</param>
        /// <param name="messages">写入信息</param>
        /// <param name="IsHeader">是否加头部分割线</param>
        public static void WriteLog(string filename, string[] messages, bool IsHeader = true)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
                .WriteTo.File(Path.Combine($"log/Serilog/", $"{filename}.log"), rollingInterval: RollingInterval.Infinite, outputTemplate: "{Message}{NewLine}{Exception}")
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
