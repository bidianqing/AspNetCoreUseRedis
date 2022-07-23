using AspNetCoreUseRedis.Factory;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.ConnectionMultiplexerFactory = async () =>
    {
        return await ConnectionMultiplexer.ConnectAsync("localhost:6379");
    };
});

builder.Services.AddSingleton(sp =>
{
    return ConnectionMultiplexer.Connect("localhost:6379");
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
