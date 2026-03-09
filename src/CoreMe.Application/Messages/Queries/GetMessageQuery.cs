namespace CoreMe.Application.Messages.Queries;

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

public class GetMessageQueryHandler(
    ) : IRequestHandler<GetMessageQuery, Result>
{
    public Task<Result> Handle(GetMessageQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
