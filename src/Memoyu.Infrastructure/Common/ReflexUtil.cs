/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Core.Common
*   文件名称 ：ReflexUtil.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2020-12-29 0:36:56
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using FreeSql.DataAnnotations;
using Memoyu.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Memoyu.Infrastructure.Common
{
    public class ReflexUtil
    {
        /// <summary>
        /// 扫描 IEntity类所在程序集，反射得到类上有特性标签为TableAttribute 的所有类
        /// </summary>
        /// <returns></returns>
        public static Type[] GetTypesByTableAttribute()
        {
            List<Type> tableAssembies = new List<Type>();
            foreach (Type type in Assembly.GetAssembly(typeof(IEntity)).GetExportedTypes())
            {
                foreach (Attribute attribute in type.GetCustomAttributes())
                {
                    if (attribute is TableAttribute tableAttribute)
                    {
                        if (tableAttribute.DisableSyncStructure == false)
                        {
                            tableAssembies.Add(type);
                        }
                    }
                }
            };
            return tableAssembies.ToArray();
        }
    }
}
