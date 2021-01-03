﻿/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Core.Domain.Shared.Base
*   文件名称 ：IAuditBaseRepository.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-03 16:00:54
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using FreeSql;
using System;

namespace Memoyu.Core.Domain.Shared.Base
{
    public interface IAuditBaseRepository<TEntity> : IBaseRepository<TEntity, Guid> where TEntity : class
    {
    }

    public interface IAuditBaseRepository<TEntity, TKey> : IBaseRepository<TEntity, TKey> where TEntity : class
    {

    }
}
