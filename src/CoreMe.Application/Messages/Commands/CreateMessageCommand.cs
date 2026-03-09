using CoreMe.Application.Common.Security;
using CoreMe.Application.Messages.Common;
using CoreMe.Domain.Events.Messages;
using Microsoft.Extensions.Logging;

namespace CoreMe.Application.Messages.Commands;

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

public class CreateMessageCommandHandler(
    ILogger<CreateMessageCommandHandler> logger,
    ICurrentUserProvider currentUserProvider,
    IPublisher publisher
    ) : IRequestHandler<CreateMessageCommand, Result>
{
    public async Task<Result> Handle(CreateMessageCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUserProvider.GetCurrentUser().Id;

        try
        {
            await publisher.Publish(new CreateMessageEvent
            {
                UserId = userId,
                ToUsers = request.ToUsers,
                ToRoles = request.ToRoles,
                Content = new UserMessageContent
                {
                    Content = request.Content,
                }.ToJson()
            }, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "发送消息异常");
            return Result.Failure("发送消息异常");
        }

        return Result.Success();
    }
}
