﻿/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Core.Application.Core.Permission.Impl
*   文件名称 ：PermissionService.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-14 22:46:15
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using Memoyu.Core.Application.Base.Impl;
using Memoyu.Core.Application.Contracts.Dtos.Core;
using Memoyu.Core.Domain.Base;
using Memoyu.Core.Domain.Entities.Core;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Memoyu.Core.Application.Core.Permission.Impl
{
    public class PermissionService : ApplicationService, IPermissionService
    {
        private readonly IAuditBaseRepository<PermissionEntity, long> _permissionRepository;
        private IAuditBaseRepository<RolePermissionEntity, long> _rolePermissionRepository;

        public PermissionService(IAuditBaseRepository<PermissionEntity,long> permissionRepository , IAuditBaseRepository<RolePermissionEntity , long> rolePermissionRepository )
        {
            _permissionRepository = permissionRepository;
            _rolePermissionRepository = rolePermissionRepository;
        }

        public async Task<IDictionary<string, IEnumerable<PermissionDto>>> GetAllStructual()
        {
            return (await _permissionRepository.Select.ToListAsync())
                   .GroupBy(r => r.Module)
                   .ToDictionary(
                       group => group.Key,
                       group =>
                           Mapper.Map<IEnumerable<PermissionDto>>(group.ToList())
                   );
        }

        public async Task<bool> CheckAsync(string permission)
        {
            long[] roleIds = CurrentUser.Roles;
            PermissionEntity permissionEntity = await _permissionRepository.Where(r => r.Name == permission).FirstAsync();
            bool existPermission = await _rolePermissionRepository.Select
                .AnyAsync(r => roleIds.Contains(r.RoleId) && r.PermissionId == permissionEntity.Id);
            return existPermission;
        }
    }
}
