using CoreMe.Application.Common.Models;

namespace CoreMe.Application.Messages.Common
{
    public class MessagePageResult: PaginationResult<MessageResult>
    {
        public MessagePageResult(IReadOnlyList<MessageResult> items) : base(items)
        {
        }

        public int UnReads { get; set; }
    }
}
