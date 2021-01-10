/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Infrastructure.Repositories.Test
*   文件名称 ：TestRepository.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-03 8:29:23
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using FreeSql;
using Memoyu.Core.Domain.Base.Impl;
using Memoyu.Core.Domain.Shared.Security;
using Memoyu.Core.Domain.Entities.Test;
using Memoyu.Core.Domain.IRepositories.Test;

namespace Memoyu.Core.Domain.Repositories.Test
{
    public class TestRepository : AuditBaseRepository<TestEntity>, ITestRepository
    {
        private readonly ICurrentUser _currentUser;
        public TestRepository(UnitOfWorkManager unitOfWorkManager, ICurrentUser currentUser) : base(unitOfWorkManager, currentUser)
        {
            _currentUser = currentUser;
        }
    }
}
