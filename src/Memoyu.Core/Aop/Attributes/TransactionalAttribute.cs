/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Core.Aop.Attributes
*   文件名称 ：TransactionalAttribute.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2020-12-27 20:03:58
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using FreeSql;
using System;
using System.Data;

namespace Memoyu.Core.Aop.Attributes
{
    /// <summary>
    /// 事务
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class TransactionalAttribute : Attribute
    {
        /// <summary>
        /// 事务传播方式
        /// </summary>
        public Propagation Propagation { get; set; } = Propagation.Required;

        /// <summary>
        /// 事务隔离级别
        /// </summary>
        public IsolationLevel? IsolationLevel { get; set; }
    }
}
