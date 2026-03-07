namespace CoreMe.Domain.Constants.Security.Permissions;

public static partial class Permissions
{
    [Description("系统日志")]
    public static class LoggerSystem
    {
        [Description("获取系统日志")]
        public const string Get = "get:logger:system";

        [Description("获取系统日志分页")]
        public const string Page = "page:logger:system";
    }
}
