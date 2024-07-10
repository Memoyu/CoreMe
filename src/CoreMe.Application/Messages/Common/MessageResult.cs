namespace CoreMe.Application.Messages.Common
{
    public class MessageResult
    {
        public long MessageId { get; set; }

        public bool IsRead { get; set; }

        public MessageType MessageType { get; set; }

        public string Content { get; set; } = string.Empty;

        public DateTime CreateTime { get; set; }
    }
}
