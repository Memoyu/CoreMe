using CoreMe.Core.Common.Configs;

namespace CoreMe.Core.Domains.Common.Consts
{
    public class SystemConst
    {
        /// <summary>
        /// 数据库表前缀
        /// </summary>
        public const string DbTablePrefix = "core_me";
        public static class Grouping
        {
            /// <summary>
            /// 前台客户端接口组
            /// </summary>
            public const string GroupName_v1 = "v1";

            /// <summary>
            /// 后台管理接口组
            /// </summary>
            public const string GroupName_v2 = "v2";

            /// <summary>
            /// 其他通用接口组
            /// </summary>
            public const string GroupName_v3 = "v3";

            /// <summary>
            /// 授权接口组
            /// </summary>
            public const string GroupName_v4 = "v4";
        }

        /// <summary>
        /// 默认角色
        /// </summary>
        public static class Role
        {
            /// <summary>
            /// 超级管理员
            /// </summary>
            public static int Administrator = 1;
            /// <summary>
            /// 普通管理员
            /// </summary>
            public static int Admin = 2;
            /// <summary>
            /// 用户
            /// </summary>
            public static int User = 3;
        }
    }
}
