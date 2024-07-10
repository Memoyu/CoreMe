namespace CoreMe.Application.Messages.Queries.Page;

[Authorize(Permissions = ApiPermission.Message.Page)]
public record PageMessageQuery : PaginationQuery, IAuthorizeableRequest<Result>
{
    public MessageType Type{ get; set; }
}

public class PageMessageQueryValidator : AbstractValidator<PageMessageQuery>
{
    public PageMessageQueryValidator()
    {
        RuleFor(x => x.Type)
           .IsInEnum()
           .WithMessage("消息类型错误");
    }
}
