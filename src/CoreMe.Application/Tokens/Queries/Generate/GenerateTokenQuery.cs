namespace CoreMe.Application.Tokens.Queries.Generate;

public record GenerateTokenQuery(string Username, string Password) : IRequest<Result>;

public class GenerateTokenQueryValidator : AbstractValidator<GenerateTokenQuery>
{
    public GenerateTokenQueryValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty()
            .WithMessage("用户名不能为空");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("密码不能为空");
    }
}

