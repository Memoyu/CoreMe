/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Infrastructure.IRepositories
*   文件名称 ：IAuditBaseRepository.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2020-12-27 18:20:13
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using System;
using FreeSql;

namespace Memoyu.Infrastructure.IRepositories
{
    public interface IAuditBaseRepository<TEntity> : IBaseRepository<TEntity, Guid> where TEntity : class
    {
    }

    public interface IAuditBaseRepository<TEntity, TKey> : IBaseRepository<TEntity, TKey> where TEntity : class
    {

    }
}
