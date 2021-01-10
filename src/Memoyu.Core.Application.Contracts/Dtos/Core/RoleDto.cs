﻿/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Core.Application.Contracts.Dtos.Core
*   文件名称 ：RoleDto.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-10 15:22:29
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using Memoyu.Core.Domain.Entities.System;
using System.Collections.Generic;

namespace Memoyu.Core.Application.Contracts.Dtos.Core
{
    public class RoleDto
    {
        public List<PermissionEntity> Permissions { get; set; }
        public string Name { get; set; }
        public string Info { get; set; }
        public bool IsStatic { get; set; }
    }
}
