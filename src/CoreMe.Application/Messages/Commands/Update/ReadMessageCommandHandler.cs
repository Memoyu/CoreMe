using CoreMe.Application.Security;

namespace CoreMe.Application.Messages.Commands.Update;

public class ReadMessageCommandHandler(
    ICurrentUserProvider currentUserProvider,
    IBaseDefaultRepository<MessageUser> messageUserRepo
    ) : IRequestHandler<ReadMessageCommand, Result>
{
    public async Task<Result> Handle(ReadMessageCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUserProvider.GetCurrentUser().Id;

        var messageIds = request.MessageIds ?? [];
        var userMessages = await messageUserRepo.Select
            .Where(m => m.UserId == userId && !m.IsRead)
            .WhereIf(request.Type.HasValue, m => m.MessageType == request.Type!.Value)
            .WhereIf(messageIds.Count != 0, m => messageIds.Contains(m.MessageId))
            .ToListAsync(cancellationToken);

        var updates = userMessages.Select(m =>
        {
            m.IsRead = true;
            return m;
        }).ToList();

        if (updates.Count != 0)
            await messageUserRepo.UpdateAsync(updates, cancellationToken);

        return Result.Success("消息标为已读成功");
    }
}
