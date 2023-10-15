﻿using CoreMe.Core.Domains.Common.Base;
using CoreMe.Core.Domains.Common.Consts;
using FreeSql.DataAnnotations;
using System;

namespace CoreMe.Core.Domains.Entities.Core;

/// <summary>
/// 权限表
/// </summary>
[Table(Name = SystemConst.DbTablePrefix + "_permission")]
public class PermissionEntity : FullAduitEntity
{
    public PermissionEntity()
    {

    }

    public PermissionEntity(string name, string module, string router)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Module = module ?? throw new ArgumentNullException(nameof(module));
        Router = router ?? throw new ArgumentNullException(nameof(router));
    }

    /// <summary>
    /// 所属权限、权限名称，例如：访问首页
    /// </summary>
    [Column(StringLength = 60)]
    public string Name { get; set; }

    /// <summary>
    /// 权限所属模块，例如：人员管理
    /// </summary>
    [Column(StringLength = 50)]
    public string Module { get; set; }

    /// <summary>
    /// 后台路由
    /// </summary>
    [Column(StringLength = 200)]
    public string Router { get; set; }
}
