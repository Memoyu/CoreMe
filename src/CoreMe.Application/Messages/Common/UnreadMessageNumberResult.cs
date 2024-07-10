namespace CoreMe.Application.Messages.Common;

public record UnreadMessageNumberResult
{
    public int Total { get; set; }

    public int User { get; set; }
}
