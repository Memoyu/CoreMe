using CoreMe.Domain.Enums;

namespace CoreMe.Domain.Events.Messages;

public record MessageNotificationEvent : IDomainEvent
{
    public List<long> ToUsers { get; set; } = [];

    public MessageType Type { get; set; }

    public long MessageId { get; set; }

    public string Content { get; set; } = string.Empty;
}
