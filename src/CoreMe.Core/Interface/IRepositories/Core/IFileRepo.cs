using CoreMe.Core.Domains.Entities.Core;
using CoreMe.Core.Interface.IRepositories.Base;

namespace CoreMe.Core.Interface.IRepositories.Core
{
    public interface IFileRepo : IAuditBaseRepo<FileEntity>
    {
        string GetFileUrl(string path);
    }
}
