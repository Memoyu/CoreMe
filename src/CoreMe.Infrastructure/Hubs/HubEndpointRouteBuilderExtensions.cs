using CoreMe.Application.Common.Hubs;
using Microsoft.AspNetCore.Builder;

namespace CoreMe.Infrastructure.Hubs;

public static class HubEndpointRouteBuilderExtensions
{
    public static WebApplication MapHubs(this WebApplication app)
    {
        app.MapHub<NotificationHub>(HubConst.ManagementHubEndpointRoute);
        return app;
    }
}
