/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Infrastructure.IRepositories
*   文件名称 ：ITestRepository.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-03 8:19:45
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using Sr.Manager.Domain.Entities;
using Sr.Manager.Domain.Shared.Base;

namespace Sr.Manager.Domain.IRepositories
{
    public interface ITestRepository:IAuditBaseRepository<TestEntity> 
    {
    }
}
