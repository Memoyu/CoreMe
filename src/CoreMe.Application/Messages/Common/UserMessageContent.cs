namespace CoreMe.Application.Messages.Common;

public record UserMessageContent
{
    /// <summary>
    /// 消息内容
    /// </summary>
    public string Content { get; set; } = string.Empty;
}
