namespace CoreMe.Application.Messages.Queries.Get;

[Authorize(Permissions = ApiPermission.Message.Get)]
public record GetMessageQuery(
    long MessageId
    ) : IAuthorizeableRequest<Result>;

public class GetMessageQueryValidator : AbstractValidator<GetMessageQuery>
{
    public GetMessageQueryValidator()
    {
        RuleFor(x => x.MessageId)
            .GreaterThan(0)
            .WithMessage("Id必须大于0");
    }
}
