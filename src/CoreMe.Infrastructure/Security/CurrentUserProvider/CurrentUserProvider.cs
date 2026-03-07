using System.IdentityModel.Tokens.Jwt;
using CoreMe.Application.Common.Security;
using Microsoft.AspNetCore.Http;

namespace CoreMe.Infrastructure.Security.CurrentUserProvider;

public class CurrentUserProvider(IHttpContextAccessor _httpContextAccessor) : ICurrentUserProvider
{
    public CurrentUser GetCurrentUser()
    {
        // 可能存在系统启动时数据同步时数据插入等场景。此时，用户认证信息为空。
        if (_httpContextAccessor.HttpContext is null ||
            _httpContextAccessor.HttpContext.User?.Claims?.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier) is null ||
            !long.TryParse(GetSingleClaimValue(ClaimTypes.NameIdentifier), out var id))
            return new CurrentUser(0, string.Empty, string.Empty);

        return new CurrentUser(
            id,
            GetSingleClaimValue(JwtRegisteredClaimNames.Name) ?? string.Empty,
            GetSingleClaimValue(ClaimTypes.Email) ?? string.Empty);
    }

    public long GetCurrentVisitor()
    {
        var visitorId = 0L;
        var context = _httpContextAccessor.HttpContext;
        if (context is null) return visitorId;

        var hasHeader = context.Request.Headers.TryGetValue("Visitor-Id", out var value);
        if (!hasHeader) return visitorId;

        var well = long.TryParse(value, out visitorId);
        return visitorId;
    }

    public string GetClientIp()
    {
        var context = _httpContextAccessor.HttpContext;
        if (context is null) return string.Empty;

        var ip = context.Request.Headers["X-Forwarded-For"].ToString();
        if (string.IsNullOrEmpty(ip))
        {
            if (context.Connection.RemoteIpAddress != null) ip = context.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }

        return ip;
    }

    private string? GetSingleClaimValue(string claimType) =>
        _httpContextAccessor.HttpContext!.User?.Claims?.FirstOrDefault(claim => claim.Type == claimType)?.Value;
}
