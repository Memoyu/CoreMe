using FreeSql;

namespace CoreMe.Application.Common.Interfaces.Persistence.Repositories;

public interface IBaseDefaultRepository<TEntity> : IBaseRepository<TEntity, long> where TEntity : class
{
}
