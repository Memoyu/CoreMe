using CoreMe.Core.Domains.Common.Base;
using CoreMe.Core.Domains.Common.Consts;
using FreeSql.DataAnnotations;
using System;
using System.ComponentModel;

namespace CoreMe.Core.Domains.Entities.Core;
/// <summary>
/// IP访问日志表
/// </summary>
[Table(Name = SystemConst.DbTablePrefix + "_ip_log")]
public class IpLogEntity : FullAduitEntity
{
    /// <summary>
    /// 客户端IP
    /// </summary>
    [Description("客户端IP")]
    public string Ip { get; set; }

    /// <summary>
    /// 访问路径
    /// </summary>
    [Description("访问路径")]
    public string Path { get; set; }

    /// <summary>
    /// 访问时间
    /// </summary>
    [Description("访问时间")]
    public DateTime VisitTime { get; set; }
}
