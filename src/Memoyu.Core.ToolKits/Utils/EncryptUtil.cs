﻿/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Core.ToolKits.Utils
*   文件名称 ：EncryptUtil.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-03 11:54:11
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using System.Security.Cryptography;
using System.Text;

namespace Memoyu.Core.ToolKits.Utils
{
    public class EncryptUtil
    {
        /// <summary>
        /// 通过创建哈希字符串适用于任何 MD5 哈希函数 （在任何平台） 上创建 32 个字符的十六进制格式哈希字符串
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string Encrypt(string source)
        {
            using MD5 md5Hash = MD5.Create();
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(source));
            StringBuilder sBuilder = new StringBuilder();
            foreach (byte t in data)
            {
                sBuilder.Append(t.ToString("x2"));
            }

            string hash = sBuilder.ToString();
            return hash.ToUpper();
        }

        /// <summary>
        /// 验证source加密码后是否生成mdPwd
        /// </summary>
        /// <param name="mdPwd">Md5生成的代码</param>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool Verify(string mdPwd, string source)
        {
            return mdPwd == Encrypt(source).ToUpper();
        }
    }
}
