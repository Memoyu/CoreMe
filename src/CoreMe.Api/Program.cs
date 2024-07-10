using AspNetCoreRateLimit;
using CoreMe.Api;
using CoreMe.Domain.Events.Permissions;
using CoreMe.Infrastructure.Hubs;

var builder = WebApplication.CreateBuilder(args);

// ����serilog
builder.AddSerilog();

// Add services to the container.
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddPresentation(builder.Configuration);

var app = builder.Build();

// ��������������ɣ�����Ȩ������ͬ��
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

// ����
app.UseIpRateLimiting();

app.UseCors(AppConst.CorsPolicyName);

app.UseAuthorization();

app.MapControllers();

// SignalR�ս������
app.MapHubs();

app.Run();
