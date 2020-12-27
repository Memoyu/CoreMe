/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Extensions.Middleware
*   文件名称 ：UnitOfWorkInterceptor.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2020-12-27 19:51:46
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memoyu.Extensions.Middleware
{
    public class UnitOfWorkInterceptor : IInterceptor
    {
        private readonly UnitOfWorkAsyncInterceptor asyncInterceptor;

        public UnitOfWorkInterceptor(UnitOfWorkAsyncInterceptor interceptor)
        {
            asyncInterceptor = interceptor;
        }

        public void Intercept(IInvocation invocation)
        {
            asyncInterceptor.ToInterceptor().Intercept(invocation);
        }
    }
}
