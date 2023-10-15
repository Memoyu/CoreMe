using CoreMe.Core.Common.Configs;
using CoreMe.Core.Domains.Entities.Core;
using CoreMe.Core.Interface.IRepositories.Core;
using CoreMe.Core.Security;
using CoreMe.Infrastructure.Repository.Base;
using FreeSql;
using Microsoft.Extensions.Options;

namespace CoreMe.Infrastructure.Repository.Core;

public class FileRepo : AuditBaseRepo<FileEntity>, IFileRepo
{
    public FileRepo(UnitOfWorkManager unitOfWorkManager, ICurrentUser currentUser) : base(unitOfWorkManager, currentUser)
    {
    }

    public string GetFileUrl(string path)
    {
        if (string.IsNullOrEmpty(path)) return "";
        if (path.StartsWith("http") || path.StartsWith("https"))//如果是完整地址
        {
            return path;
        }

        if (path.StartsWith("core"))//如果是本地初始资源
        {
            return Appsettings.FileStorage.LocalFileHost + path;
        }


        FileEntity file = base.Where(r => r.Path == path).First();
        if (file == null) return path;
        switch (file.Type)
        {
            case 1:
                return Appsettings.FileStorage.LocalFileHost + path;
            default:
                return Appsettings.FileStorage.LocalFileHost + path;
        }
    }
}
