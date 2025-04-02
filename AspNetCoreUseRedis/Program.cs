using AspNetCoreUseRedis;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.ConnectionMultiplexerFactory = async () =>
    {
        var configuration = builder.Configuration.GetConnectionString("Redis");
        return await ConnectionMultiplexer.ConnectAsync(configuration);
    };
});

builder.Services.AddSingleton(sp =>
{
    var configuration = builder.Configuration.GetConnectionString("Redis");
    ConnectionMultiplexer connection = ConnectionMultiplexer.Connect(configuration);

    return connection;
});

builder.Services.AddHostedService<SubscribeRedisMessageBackgroudService>();
builder.Services.AddHostedService<PopRedisMessageBackgroudService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapControllers();

app.Run();
