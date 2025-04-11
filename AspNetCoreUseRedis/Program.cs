using AspNetCoreUseRedis;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddSingleton(sp =>
{
    var configuration = builder.Configuration.GetConnectionString("Redis");
    ConnectionMultiplexer connection = ConnectionMultiplexer.Connect(configuration);

    return connection;
});

builder.Services.AddHostedService<SubscribeRedisMessageBackgroudService>();
builder.Services.AddHostedService<PopRedisMessageBackgroudService>();

builder.Services.AddScoped<IDemoService, DemoService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapControllers();

app.Run();
