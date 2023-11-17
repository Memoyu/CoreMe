using CoreMe.Core.Domains.Common.Enums.Base;
using CoreMe.Core.Domains.Entities.Core;
using CoreMe.Core.Interface.IDependency;
using CoreMe.Core.Interface.IRepositories.Core;
using CoreMe.Core.Security;
using FreeSql;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CoreMe.Service.Core.DataSeed;

public class DataSeedContributor : IDataSeedContributor, ISingletonDependency
{

    private readonly IRoleRepo _roleRepo;
    private readonly IPermissionRepo _permissionRepo;
    private readonly IRolePermissionRepo _rolePermissionRepo;
    private readonly ILogger<DataSeedContributor> _logger;
    public DataSeedContributor(
        ILoggerFactory loggerFactory,
        IRoleRepo roleRepo,
        IPermissionRepo permissionRepo,
        IRolePermissionRepo rolePermissionRepo
        )
    {
        _logger = loggerFactory.CreateLogger<DataSeedContributor>();
        _roleRepo = roleRepo;
        _permissionRepo = permissionRepo;
        _rolePermissionRepo = rolePermissionRepo;

    }

    public async Task InitAdministratorPermissionAsync()
    {
        var roles = await _roleRepo.Select.Where(r => r.Type == RoleType.Administrator.GetHashCode()).ToListAsync();
        if (!roles.Any()) return;

        var roleBIds = roles.Select(r => r.Id).ToList();
        List<PermissionEntity> pers = await _permissionRepo.Select.ToListAsync();//获取所有权限
        List<RolePermissionEntity> rolePers = await _rolePermissionRepo.Select.Where(rp => roleBIds.Contains(rp.RoleId)).ToListAsync();
        var needAddRolePers = new List<RolePermissionEntity>();
        foreach (var role in roles)
        {
            var currRolePers = rolePers.Where(rp => rp.RoleId == role.Id).ToList();
            var adds = pers.Where(p => !currRolePers.Any(crp => crp.PermissionId == p.Id)).Select(p => new RolePermissionEntity
            {
                // TODO：待加入雪花ID支持
                // BId = SnowFlake.NextId(),
                RoleId = role.Id,
                PermissionId = p.Id,
            }).ToList();
            if (adds.Any())
                needAddRolePers.AddRange(adds);
        }

        if (needAddRolePers.Any())
            await _rolePermissionRepo.InsertAsync(needAddRolePers);//插入全部的超级管理员角色权限

        _logger.LogInformation($"超级管理员权限：新增了{needAddRolePers.Count}条数据");
    }

    public async Task InitPermissionAsync(List<PermissionDefinition> permissions)
    {
        List<PermissionEntity> insertPermissions = new List<PermissionEntity>();//新增权限集合
        List<PermissionEntity> updatePermissions = new List<PermissionEntity>();//更新权限集合

        Expression<Func<RolePermissionEntity, bool>> rolePermissionExpression = u => false;
        Expression<Func<PermissionEntity, bool>> permissionExpression = u => false;

        List<PermissionEntity> allPermissions = await _permissionRepo.Select.ToListAsync();//已持久化的权限数据

        allPermissions.ForEach(per =>//过滤已持久化的权限数据，获得需要删除的权限、角色权限数据
        {
            if (permissions.All(r => r.Permission != per.Name))//持久化的权限数据是否存在于在本次获取到的权限数据
            {
                permissionExpression = permissionExpression.Or(r => r.Id == per.Id);//拼接表达式，权限Id
                rolePermissionExpression = rolePermissionExpression.Or(r => r.PermissionId == per.Id);//拼接表达式，角色权限Id
            }
        });

        int effectPerRows = await _permissionRepo.DeleteAsync(permissionExpression);//删除权限数据
        int effectRolePerRows = await _rolePermissionRepo.DeleteAsync(rolePermissionExpression);//删除角色权限数据
        _logger.LogInformation($"操 作 权 限 表：删除了{effectPerRows}条数据");
        _logger.LogInformation($"操作角色权限表：删除了{effectRolePerRows}条数据");

        permissions.ForEach(per =>//过滤本次获取到的权限数据，获得需要新增、更新的权限数据
        {
            PermissionEntity permissionEntity = allPermissions.FirstOrDefault(u => u.Module == per.Module && u.Name == per.Permission);//在已持久化的权限数据中获取符合条件数据
            if (permissionEntity == null)//如果权限数据为空，则可新增
            {
                insertPermissions.Add(new PermissionEntity(per.Permission, per.Module, per.Router));
            }
            else//否则
            {
                bool routerExist = allPermissions.Any(u => u.Module == per.Module && u.Name == per.Permission && u.Router == per.Router);//是否存在符合条件的数据
                if (!routerExist)//不存在则证明Router发生了改变，则更新Router，
                {
                    permissionEntity.Router = per.Router;
                    updatePermissions.Add(permissionEntity);
                }
            }
        });

        await _permissionRepo.InsertAsync(insertPermissions);
        _logger.LogInformation($"操 作 权 限 表：新增了{insertPermissions.Count}条数据");

        await _permissionRepo.UpdateAsync(updatePermissions);
        _logger.LogInformation($"操 作 权 限 表：更新了{updatePermissions.Count}条数据");

    }
}
