using FreeSql;
using System;

namespace CoreMe.Core.Interface.IRepositories.Base
{
    public interface IAuditBaseRepo<TEntity> : IBaseRepository<TEntity, long> where TEntity : class
    {
    }

    public interface IAuditBaseRepo<TEntity, TKey> : IBaseRepository<TEntity, TKey> where TEntity : class
    {

    }
}
