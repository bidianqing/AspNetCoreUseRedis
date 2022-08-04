using AspNetCoreUseRedis.Factory;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.ConnectionMultiplexerFactory = async () =>
    {
        var configuration = builder.Configuration.GetConnectionString("redis");
        return await ConnectionMultiplexer.ConnectAsync(configuration);
    };
});

builder.Services.AddSingleton(sp =>
{
    var configuration = builder.Configuration.GetConnectionString("redis");
    return ConnectionMultiplexer.Connect(configuration);
});

builder.Services.AddRedisFactory(options =>
{
    var wrappers = builder.Configuration.GetSection("RedisConnections").Get<ConnectionMultiplexerWrapper[]>();
    options.ConnectionMultiplexerWrappers = wrappers;
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
