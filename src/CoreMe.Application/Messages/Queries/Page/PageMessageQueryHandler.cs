using CoreMe.Application.Messages.Common;
using CoreMe.Application.Security;

namespace CoreMe.Application.Messages.Queries.Page;

public class PageMessageQueryHandler(
    IMapper mapper,
    ICurrentUserProvider currentUserProvider,
    IBaseDefaultRepository<MessageUser> messageUserRepo
    ) : IRequestHandler<PageMessageQuery, Result>
{
    public async Task<Result> Handle(PageMessageQuery request, CancellationToken cancellationToken)
    {
        var userId = currentUserProvider.GetCurrentUser().Id;

        var userMessages = await messageUserRepo.Select
            .Include(m => m.Message)
            .Where(m => m.UserId == userId && m.MessageType == request.Type)
            .OrderByDescending(a => a.CreateTime)
            .ToPageListAsync(request, out var total, cancellationToken);

        var unreads = await messageUserRepo.Select
            .Where(m => m.UserId == userId && m.MessageType == request.Type && !m.IsRead)
            .CountAsync(cancellationToken);

        var messages = new List<MessageResult>();
        foreach (var userMessage in userMessages)
        {
            var message = mapper.Map<MessageResult>(userMessage.Message);
            message.IsRead = userMessage.IsRead;
            messages.Add(message);
        }

        var page = new MessagePageResult(messages)
        {
            Total = total,
            UnReads = (int)unreads
        };

        return Result.Success(page);

    }
}

