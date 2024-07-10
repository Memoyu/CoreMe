namespace CoreMe.Application.Messages.Commands.Update;

[Authorize(Permissions = ApiPermission.Message.Read)]
public record ReadMessageCommand(
        MessageType? Type,
        List<long>? MessageIds
    ) : IAuthorizeableRequest<Result>;

public class ReadMessageCommandValidator : AbstractValidator<ReadMessageCommand>
{
    public ReadMessageCommandValidator()
    {
        RuleFor(x => x)
         .Must(x => x.Type.HasValue || (x.MessageIds ?? []).Count > 0)
         .WithMessage("消息类型或消息Id集合必须传一个");
    }
}
