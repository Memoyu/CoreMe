using CoreMe.Core.Interface.IRepositories.Core;
using CoreMe.Service.Base;
using CoreMe.Service.Core.Permission.Input;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreMe.Service.Core.Permission;

public class RoleService : ApplicationSvc, IRoleService
{
    private readonly IRoleRepo _roleRepo;
    public RoleService(IRoleRepo roleRepo)
    {
        _roleRepo = roleRepo;
    }

    public async Task<List<RoleDto>> GetAllAsync()
    {
        var entitys = await _roleRepo.Select.Where(r => r.IsDeleted == false).ToListAsync();
        var dtos = entitys.Select(e => Mapper.Map<RoleDto>(e)).ToList();
        return dtos;
    }
}
