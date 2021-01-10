/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Infrastructure.IRepositories.Test
*   文件名称 ：ITestRepository.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-03 8:19:45
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using Memoyu.Core.Domain.Base;
using Memoyu.Core.Domain.Entities.Test;

namespace Memoyu.Core.Domain.IRepositories.Test
{
    public interface ITestRepository:IAuditBaseRepository<TestEntity> 
    {
    }
}
