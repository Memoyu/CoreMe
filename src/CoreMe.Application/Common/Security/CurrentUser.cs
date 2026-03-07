namespace CoreMe.Application.Common.Security;

public record CurrentUser(
    long Id,
    string Username,
    string Email);
