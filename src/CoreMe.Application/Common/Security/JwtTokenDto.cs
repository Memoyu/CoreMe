namespace CoreMe.Application.Security;

public class JwtTokenDto
{
    public string AccessToken { get; set; } = string.Empty;

    public string RefreshToken { get; set; } = string.Empty;
}
