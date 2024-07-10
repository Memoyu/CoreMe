using CoreMe.Application.Messages.Common;
using CoreMe.Domain.Events.Messages;

namespace CoreMe.Application.Common.Mappings;

public class MessageRegister : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.ForType<CreateMessageEvent, Message>()
             .Map(d => d.MessageId, s => SnowFlakeUtil.NextId())
             .Map(d => d.UserId, s => s.UserId)
             .Map(d => d.MessageType, s => s.MessageType)
             .Map(d => d.Content, s => s.Content);

        config.ForType<CreateMessageEvent, MessageNotificationEvent>()
             .Map(d => d.Type, s => s.MessageType)
             .Map(d => d.Content, s => GetMessageContentFormat(s.UserId, s.MessageType, s.Content));

        config.ForType<Message, MessageResult>()
             .Map(d => d.MessageId, s => s.MessageId)
             .Map(d => d.MessageType, s => s.MessageType)
             .Map(d => d.Content, s => GetMessageContentFormat(s.UserId, s.MessageType, s.Content))
             .Map(d => d.CreateTime, s => s.CreateTime);
    }

    private string GetMessageTitle(MessageType type) => type switch
    {
        MessageType.User => "收到用户发来消息",
        _ => throw new NotImplementedException("未定义消息类型标题"),
    };

    private string GetMessageContentFormat(long userId, MessageType type, string content)
    {
        var formatContent = string.Empty;
        switch (type)
        {
            case MessageType.User:
                var userMessage = content.ToDesJson<UserMessageContent>() ?? throw new Exception("消息格式错误");
                var user = MapContext.Current.GetService<IBaseDefaultRepository<User>>().Select.Where(c => c.UserId == userId).First();
                formatContent = new UserMessageResult { Content = userMessage.Content, UserNickname = user.Nickname, UserAvatar = user.Avatar }.ToJson();
                break;
        }

        return formatContent;
    }
}
