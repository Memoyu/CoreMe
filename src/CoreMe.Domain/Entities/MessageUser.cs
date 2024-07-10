using CoreMe.Domain.Enums;

namespace CoreMe.Domain.Entities;

/// <summary>
/// 消息接收方表
/// </summary>
[Table(Name = "message_user")]
[Index("index_on_message_user_message_id", nameof(MessageId), false)]
[Index("index_on_message_user_user_id", nameof(UserId), false)]
public class MessageUser : BaseAuditEntity
{
    /// <summary>
    /// 消息Id
    /// </summary>
    [Description("消息Id")]
    [Column(IsNullable = false)]
    public long MessageId { get; set; }

    /// <summary>
    /// 接收方Id
    /// </summary>
    [Description("接收方Id")]
    [Column(IsNullable = false)]
    public long UserId { get; set; }

    /// <summary>
    /// 消息类型(主要做数量统计)
    /// </summary>
    [Description("消息类型(主要做数量统计)")]
    [Column(IsNullable = false)]
    public MessageType MessageType { get; set; }

    /// <summary>
    /// 是否已读
    /// </summary>
    [Description("是否已读")]
    [Column(IsNullable = false)]
    public bool IsRead { get; set; }

    /// <summary>
    /// 消息内容
    /// </summary>
    [Navigate(nameof(Message.MessageId), TempPrimary = nameof(MessageId))]
    public virtual Message Message { get; set; } = new();
}
