/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Core.Aop.Attributes
*   文件名称 ：CacheableAttribute.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2020-12-27 20:47:30
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using System;

namespace Memoyu.Core.Aop.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CacheableAttribute : Attribute
    {
        public CacheableAttribute()
        {
        }

        public CacheableAttribute(string cacheKey)
        {
            CacheKey = cacheKey;
        }

        public string CacheKey { get; set; }

    }
}
