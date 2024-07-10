using CoreMe.Application.Security;

namespace CoreMe.Application.Common.Interfaces.Security;

public interface IJwtTokenGenerator
{
    /// <summary>
    /// 生成JWT Token
    /// </summary>
    /// <param name="user">用户信息</param>
    /// <returns></returns>
    JwtTokenDto GenerateToken(User user);

    /// <summary>
    /// 刷新JWT Token
    /// </summary>
    /// <param name="user">用户信息</param>
    /// <returns></returns>
    JwtTokenDto RefreshToken(User user);
}
