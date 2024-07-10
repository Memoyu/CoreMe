using System.IdentityModel.Tokens.Jwt;
using CoreMe.Application.Common.Models.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CoreMe.Infrastructure.Security.GenerateToken;

public class JwtTokenGenerator(IOptionsMonitor<AuthorizationSettings> authOptions) : IJwtTokenGenerator
{
    private readonly JwtOptions _jwtOptions = authOptions.CurrentValue?.Jwt ?? throw new Exception("未配置服务jwt授权信息");

    public JwtTokenDto GenerateToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Secret));
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(JwtRegisteredClaimNames.Name, user.Username),
            new(JwtRegisteredClaimNames.Email, user.Email),
        };

        var securityToken = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(_jwtOptions.ExpiryInMin)),
            signingCredentials: signingCredentials);

        var accessToken = new JwtSecurityTokenHandler().WriteToken(securityToken);
        string refreshToken = GenerateRefreshToken();

        return new JwtTokenDto { AccessToken = accessToken, RefreshToken = refreshToken };
    }

    public JwtTokenDto RefreshToken(User user)
    {
        if (user is null)
            throw new ArgumentException("用户信息不能为空");

        if (DateTime.Compare(user.LastLoginTime, DateTime.Now) > new TimeSpan(5, 0, 0, 0).Ticks)
            throw new InvalidOperationException("请重新登录");

        var jwtToken = GenerateToken(user);

        return jwtToken;
    }

    /// <summary>
    /// 生成RefreshToken
    /// </summary>
    /// <param name="size">长度</param>
    /// <returns></returns>
    private string GenerateRefreshToken(int size = 32)
    {
        var randomNumber = new byte[size];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
