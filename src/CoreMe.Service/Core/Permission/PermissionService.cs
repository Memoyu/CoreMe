﻿using CoreMe.Core.Domains.Entities.Core;
using CoreMe.Core.Interface.IRepositories.Core;
using CoreMe.Service.Base;
using CoreMe.Service.Core.Permission.Input;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreMe.Service.Core.Permission;

public class PermissionService : ApplicationService, IPermissionService
{
    private readonly IPermissionRepo _permissionRepo;
    private readonly IRolePermissionRepo _rolePermissionRepo;
    private readonly IUserRoleRepo _userRoleRepo;

    public PermissionService(IPermissionRepo permissionRepo, IRolePermissionRepo rolePermissionRepo, IUserRoleRepo userRoleRepo)
    {
        _permissionRepo = permissionRepo;
        _rolePermissionRepo = rolePermissionRepo;
        _userRoleRepo = userRoleRepo;
    }

    public async Task<IDictionary<string, IEnumerable<PermissionDto>>> GetAllStructual()
    {
        return (await _permissionRepo.Select.ToListAsync())
               .GroupBy(r => r.Module)
               .ToDictionary(
                   group => group.Key,
                   group =>
                       Mapper.Map<IEnumerable<PermissionDto>>(group.ToList())
               );
    }

    public async Task<bool> CheckAsync(string permission, long userId)
    {
        var roleIds = await _userRoleRepo.Select.Where(ur => ur.UserId == userId).ToListAsync(ur => ur.RoleId);
        PermissionEntity permissionEntity = await _permissionRepo.Where(r => r.Name == permission).FirstAsync();
        bool existPermission = await _rolePermissionRepo.Select
            .AnyAsync(r => roleIds.Contains(r.RoleId) && r.PermissionId == permissionEntity.Id);
        return existPermission;
    }
}
