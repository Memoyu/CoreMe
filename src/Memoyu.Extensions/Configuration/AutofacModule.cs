/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Extensions.Configuration
*   文件名称 ：AutofacModule.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2020-12-27 17:23:35
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using Autofac;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Memoyu.Extensions.Configuration
{
    public class AutofacModule : Module
    {
        private readonly IConfiguration _configuration;
        public AutofacModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        protected override void Load(ContainerBuilder builder)
        {
        }
    }
}
