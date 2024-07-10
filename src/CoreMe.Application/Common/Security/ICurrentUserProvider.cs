namespace CoreMe.Application.Security;

public interface ICurrentUserProvider
{
    /// <summary>
    /// 获取当前登录用户
    /// </summary>
    /// <returns></returns>
    CurrentUser GetCurrentUser();

    /// <summary>
    /// 获取当前访客
    /// </summary>
    /// <returns></returns>
    long GetCurrentVisitor();

    /// <summary>
    /// 获取当前请求Ip
    /// </summary>
    /// <returns></returns>
    string GetClientIp();
}
