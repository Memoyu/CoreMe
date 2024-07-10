using CoreMe.Domain.Enums;

namespace CoreMe.Domain.Entities;

/// <summary>
/// 消息内容表
/// </summary>
[Table(Name = "message")]
[Index("index_on_message_id", nameof(MessageId), false)]
[Index("index_on_message_user_id", nameof(UserId), false)]
public class Message : BaseAuditEntity
{
    /// <summary>
    /// 消息Id
    /// </summary>
    [Snowflake]
    [Description("消息Id")]
    [Column(CanUpdate = false, IsNullable = false)]
    public long MessageId { get; set; }

    /// <summary>
    /// 发送方Id
    /// </summary>
    [Description("发送方Id")]
    [Column(IsNullable = true)]
    public long UserId { get; set; }

    /// <summary>
    /// 消息类型
    /// </summary>
    [Description("消息类型")]
    [Column(IsNullable = false)]
    public MessageType MessageType { get; set; }

    /// <summary>
    /// 消息内容（结构化数据）
    /// </summary>
    [Description("消息内容")]
    [Column(StringLength = -2, IsNullable = false)]
    public string Content { get; set; } = string.Empty;
}
