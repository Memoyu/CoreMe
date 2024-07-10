using CoreMe.Domain.Enums;

namespace CoreMe.Domain.Entities;

/// <summary>
/// 用户身份认证表
/// </summary>
[Table(Name = "user_identity")]
[Index("index_on_identity_id", nameof(IdentityId), false)]
[Index("index_on_user_id", nameof(UserId), false)]
public class UserIdentity : BaseAuditEntity
{
    /// <summary>
    /// 用户Id
    /// </summary>
    [Snowflake]
    [Description("业务Id")]
    [Column(CanUpdate = false, IsNullable = false)]
    public long IdentityId { get; set; }

    /// <summary>
    /// 用户Id
    /// </summary>
    [Description("用户Id")]
    [Column(IsNullable = false)]
    public long UserId { get; set; }

    /// <summary>
    /// 认证类型
    /// </summary>
    [Description("认证类型， Password，GitHub、QQ、WeiXin等")]
    [Column(IsNullable = false)]
    public UserIdentityType IdentityType { get; set; }

    /// <summary>
    /// 认证者，例如 用户名,手机号，邮件等，
    /// </summary>
    [Description("认证者，例如 用户名,手机号，邮件等，")]
    [Column(StringLength = 24, IsNullable = false)]
    public string Identifier { get; set; } = string.Empty;

    /// <summary>
    /// 凭证，例如 密码,存OpenId、Id，同一IdentityType的OpenId的值是唯一的
    /// </summary>
    [Description("凭证，例如 密码,存OpenId、Id，同一IdentityType的OpenId的值是唯一的")]
    [Column(StringLength = 50, IsNullable = false)]
    public string Credential { get; set; } = string.Empty;
}
