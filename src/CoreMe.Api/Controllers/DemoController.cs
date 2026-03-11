using CoreMe.Application.Demo;

namespace CoreMe.Api.Controllers
{
    /// <summary>
    /// demo
    /// </summary>
    /// <param name="mediator"></param>
    public class DemoController(ISender mediator) : ApiControllerBase
    {
        /// <summary>
        /// demo get
        /// </summary>
        /// <returns></returns>
        [HttpGet("get")]
        public async Task<Result> GetAsync([FromQuery] DemoQuery request)
        {
            return await mediator.Send(request);
        }
    }
}
