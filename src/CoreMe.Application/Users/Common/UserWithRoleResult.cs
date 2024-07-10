using CoreMe.Application.Roles.Common;

namespace CoreMe.Application.Users.Common;

public class UserWithRoleResult : UserResult
{
    /// <summary>
    /// 用户角色
    /// </summary>
    public List<RoleListResult> Roles { get; set; } = [];
}
