namespace CoreMe.Application.Messages.Commands.Create;

[Authorize(Permissions = ApiPermission.Message.Create)]
public record CreateMessageCommand(
    List<long>? ToUsers,
    List<long>? ToRoles,
    string Content
    ) : IAuthorizeableRequest<Result>;

public class CreateMessageCommandValidator : AbstractValidator<CreateMessageCommand>
{
    public CreateMessageCommandValidator()
    {
        RuleFor(x => x.Content)
          .NotEmpty()
          .WithMessage("消息内容不能为空");

        RuleFor(x => x.Content)
          .MinimumLength(1)
          .MaximumLength(150)
          .WithMessage("消息内容长度在1-150个字符之间");

        RuleFor(x => x)
         .Must(x => (x.ToUsers ?? []).Count > 0 || (x.ToRoles ?? []).Count > 0)
         .WithMessage("接收用户或角色Id集合必须传一个");
    }
}
