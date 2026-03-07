using CoreMe.Api;

var builder = WebApplication.CreateBuilder(args);

// 配置serilog
builder.AddSerilog();

// Add services to the container.
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddPresentation(builder.Configuration);

var app = builder.Build();

// 添加APP管道中间件
app.UseAppMiddleware();

app.Run();
