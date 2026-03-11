using CoreMe.Application.Common.Interfaces.Services.App;

namespace CoreMe.Application.Demo;

public class DemoQuery : IAuthorizeableRequest<Result>;

internal class GetAddressQueryHandler(
    IDemoService demoService) : IRequestHandler<DemoQuery, Result>
{
    public async Task<Result> Handle(DemoQuery request, CancellationToken cancellationToken)
    {
        return Result.Success(demoService.GetUserId());
    }
}

