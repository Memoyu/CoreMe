﻿using CoreMe.Application.Permissions.Common;
using CoreMe.Application.Roles.Common;

namespace CoreMe.Application.Roles.Queries.Get;

public class GetRoleQueryHandler(
    IMapper mapper,
    IBaseDefaultRepository<Role> roleRepo,
    IBaseDefaultRepository<RolePermission> rolePermissionRepo
    ) : IRequestHandler<GetRoleQuery, Result>
{
    public async Task<Result> Handle(GetRoleQuery request, CancellationToken cancellationToken)
    {
        var role = await roleRepo.Select.Where(r => r.RoleId == request.RoleId).FirstAsync(cancellationToken) ?? throw new ApplicationException("角色不存在");
        var rolePermissions = await rolePermissionRepo.Select
            .Include(rp => rp.Permission)
            .Where(rp => rp.RoleId == request.RoleId)
            .ToListAsync(cancellationToken);

        var dto = mapper.Map<RoleResult>(role);
        dto.Permissions = rolePermissions.Where(rp => rp.Permission != null).Select(rp => mapper.Map<PermissionResult>(rp.Permission!)).ToList();

        return Result.Success(dto);
    }
}
