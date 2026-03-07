namespace CoreMe.Domain.Constants;
public static class CacheKeyConst
{
    /// <summary>
    /// 前缀
    /// </summary>
    private const string prefix = "coreme";

    /// <summary>
    /// 用户刷新token
    /// </summary>
    /// <param name="refreshToken">刷新token</param>
    /// <returns></returns>
    public static string UserRefreshToken(string refreshToken) => $"{prefix}:user:refresh-token:{refreshToken}";
}
