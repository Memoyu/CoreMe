using CoreMe.Domain.Enums;

namespace CoreMe.Domain.Events.Messages;

public record CreateMessageEvent : IDomainEvent
{
    /// <summary>
    /// 发送方Id（用户Id或访客Id）
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    /// 接收方Id
    /// </summary>
    public List<long>? ToUsers { get; set; }

    /// <summary>
    /// 接收方角色
    /// </summary>
    public List<long>? ToRoles { get; set; }

    /// <summary>
    /// 消息类型
    /// </summary>
    public MessageType MessageType { get; set; }

    /// <summary>
    /// 消息内容（结构化数据）
    /// </summary>
    public string Content { get; set; } = string.Empty;
}
