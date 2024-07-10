using CoreMe.Application.Messages.Commands.Create;
using CoreMe.Application.Messages.Commands.Update;
using CoreMe.Application.Messages.Queries.Get;
using CoreMe.Application.Messages.Queries.Page;

namespace CoreMe.Api.Controllers
{
    /// <summary>
    /// 通知消息
    /// </summary>
    public class MessageController(ISender mediator) : ApiControllerBase
    {
        /// <summary>
        /// 创建消息
        /// </summary>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<Result> CreateAsync(CreateMessageCommand request)
        {
            return await mediator.Send(request);
        }

        /// <summary>
        /// 标为已读消息
        /// </summary>
        /// <returns></returns>
        [HttpPut("read")]
        public async Task<Result> ReadAsync(ReadMessageCommand request)
        {
            return await mediator.Send(request);
        }

        /// <summary>
        /// 获取未读消息数量
        /// </summary>
        /// <returns></returns>
        [HttpGet("unread/number")]
        public async Task<Result> UnreadNumberAsync([FromQuery] GetUnreadMessageNumberQuery request)
        {
            return await mediator.Send(request);
        }

        /// <summary>
        /// 获取消息分页
        /// </summary>
        /// <returns></returns>
        [HttpGet("page")]
        public async Task<Result> PageAsync([FromQuery] PageMessageQuery request)
        {
            return await mediator.Send(request);
        }
    }
}
