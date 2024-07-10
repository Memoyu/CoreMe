using AspNetCoreRateLimit;
using CoreMe.Api;
using CoreMe.Domain.Events.Permissions;
using CoreMe.Infrastructure.Hubs;

var builder = WebApplication.CreateBuilder(args);

// 配置serilog
builder.AddSerilog();

// Add services to the container.
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddPresentation(builder.Configuration);

var app = builder.Build();

// 依赖容器构建完成，做数权限数据同步
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var publisher = services.GetRequiredService<IPublisher>();
    await publisher.Publish(new SyncPermissionEvent());
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi();
}

// 限流
app.UseIpRateLimiting();

app.UseCors(AppConst.CorsPolicyName);

app.UseAuthorization();

app.MapControllers();

// SignalR终结点配置
app.MapHubs();

app.Run();
