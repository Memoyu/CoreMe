namespace CoreMe.Application.Messages.Queries.Get;

[Authorize(Permissions = ApiPermission.Message.UnreadNumber)]
public record GetUnreadMessageNumberQuery() : IAuthorizeableRequest<Result>;

public class GetUnreadNumberQueryValidator : AbstractValidator<GetUnreadMessageNumberQuery>
{
    public GetUnreadNumberQueryValidator()
    {
    }
}
