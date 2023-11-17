using CoreMe.Core.Domains.Entities.Core;
using CoreMe.Core.Interface.IRepositories.Core;
using CoreMe.Core.Security;
using CoreMe.Infrastructure.Repository.Base;
using FreeSql;

namespace CoreMe.Infrastructure.Repository.Core;

public class IpLogRepo : AuditBaseRepo<IpLogEntity>, IIpLogRepo
{
    public IpLogRepo(UnitOfWorkManager unitOfWorkManager, ICurrentUser currentUser) : base(unitOfWorkManager, currentUser)
    {
    }
}
