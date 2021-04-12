﻿using CoreMe.Core.Domains.Common;
using CoreMe.Core.Domains.Common.Consts;
using CoreMe.Service.Core.Permission;
using CoreMe.Service.Core.Permission.Input;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreMe.Controllers.Core
{
    /// <summary>
    /// 权限管理
    /// </summary>
    [Route("api/admin/role")]
    [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v2)]
    public class RoleController : ApiControllerBase
    {
        private readonly IRoleService _roleService;
        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        /// <summary>
        /// 获取全部角色信息
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("get/all")]
        [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v2)]
        public async Task<ServiceResult<List<RoleDto>>> GetAllAsync()
        {
            var result = await _roleService.GetAllAsync();
            return ServiceResult<List<RoleDto>>.Successed(result);
        }
    }
}
