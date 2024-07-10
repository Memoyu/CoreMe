namespace CoreMe.Application.Tokens.Common;

public record GenerateTokenResult(
    long UserId,
    string Username,
    string AccessToken,
    string RefreshToken
);
