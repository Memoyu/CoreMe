namespace CoreMe.Application.Tokens.Common;

public record CreateTokenResult(
    long UserId,
    string Username,
    string AccessToken,
    string RefreshToken
);
