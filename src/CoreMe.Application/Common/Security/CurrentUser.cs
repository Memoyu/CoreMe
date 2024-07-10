namespace CoreMe.Application.Security;

public record CurrentUser(
    long Id,
    string Username,
    string Email);
