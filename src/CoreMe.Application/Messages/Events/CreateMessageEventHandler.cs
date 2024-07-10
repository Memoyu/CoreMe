using CoreMe.Domain.Events.Messages;

namespace CoreMe.Application.Messages.Events;

public class CreateMessageEventHandler(
    IMapper mapper,
    IBaseDefaultRepository<UserRole> userRoleRepo,
    IBaseDefaultRepository<Message> messageRepo,
    IBaseDefaultRepository<MessageUser> messageUserRepo
    ) : INotificationHandler<CreateMessageEvent>
{
    public async Task Handle(CreateMessageEvent notification, CancellationToken cancellationToken)
    {
        var message = mapper.Map<Message>(notification);

        // 构建消息接收人集合
        var toUsers = notification.ToUsers ??= [];
        if (notification.ToRoles != null)
        {
            var userIds = await userRoleRepo.Select.Where(ur => notification.ToRoles.Contains(ur.RoleId)).ToListAsync(ur => ur.UserId, cancellationToken);
            toUsers.AddRange(userIds);
        }
        toUsers = toUsers.Distinct().ToList();

        if (toUsers.Count < 1) throw new ApplicationException("消息接收人不能为空");

        // 构建消息通知模型，并推送事件
        var toUserEntities = new List<MessageUser>();
        var @event = mapper.Map<MessageNotificationEvent>(notification);
        @event.ToUsers = toUsers;
        @event.MessageId = message.MessageId;
        message.AddDomainEvent(@event);
        toUserEntities = toUsers.Select(t => new MessageUser { MessageId = message.MessageId, UserId = t, MessageType = message.MessageType }).ToList();

        // 写入消息数据
        message = await messageRepo.InsertAsync(message, cancellationToken);
        await messageUserRepo.InsertAsync(toUserEntities, cancellationToken);

        if (message.Id == 0) throw new ApplicationException("保存消息失败");
    }
}
