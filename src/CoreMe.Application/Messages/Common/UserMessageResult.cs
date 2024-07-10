namespace CoreMe.Application.Messages.Common;

public record UserMessageResult : UserMessageContent
{
    /// <summary>
    /// 用户昵称
    /// </summary>
    public string UserNickname { get; set; } = string.Empty;

    /// <summary>
    /// 用户头像
    /// </summary>
    public string UserAvatar { get; set; } = string.Empty;
}
