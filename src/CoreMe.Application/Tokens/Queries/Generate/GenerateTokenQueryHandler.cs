namespace CoreMe.Application.Tokens.Queries.Generate;

public class GenerateTokenHandler(
    IBaseDefaultRepository<User> userRepo,
    IBaseDefaultRepository<UserIdentity> userIdentityRepo,
    IJwtTokenGenerator jwtTokenGenerator
    ) : IRequestHandler<GenerateTokenQuery, Result>
{
    public async Task<Result> Handle(GenerateTokenQuery request, CancellationToken cancellationToken)
    {
        var user = await userRepo.Where(u => u.Username.Equals(request.Username)).FirstAsync(cancellationToken) ??
            throw new ApplicationException("用户名或密码错误");
        
        var identity = await userIdentityRepo.Where(u => u.UserId == user.UserId).FirstAsync(cancellationToken);
        if (identity is null || !identity.Credential.Equals(EncryptUtil.Encrypt(request.Password)))
            throw new ApplicationException("用户名或密码错误");

        var token = jwtTokenGenerator.GenerateToken(user);

        // 更新最后一次登录时间
        user.LastLoginTime = DateTime.Now;
        await userRepo.UpdateAsync(user, cancellationToken);

        return Result.Success(new GenerateTokenResult(user.UserId, user.Username, token.AccessToken, token.RefreshToken));
    }
}
