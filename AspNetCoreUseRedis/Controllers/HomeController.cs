using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using System.Threading.Tasks;

namespace AspNetCoreUseRedis.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<HomeController> _logger;
        private readonly IDatabase _redis;
        private readonly IConnectionMultiplexer _connection;
        private readonly IEnumerable<IHostedService> _hostedServices;

        public HomeController(ILogger<HomeController> logger,
            ConnectionMultiplexer connectionMultiplexer, IEnumerable<IHostedService> hostedServices)
        {
            _logger = logger;
            _redis = connectionMultiplexer.GetDatabase();
            _connection = connectionMultiplexer;
            _hostedServices = hostedServices;
        }

        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            //_redis.StringSet("name", "tom", TimeSpan.FromSeconds(20));

            //_cache.SetString("name", "tom", new DistributedCacheEntryOptions
            //{
            //    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
            //});

            long l = await _connection.GetDatabase().PublishAsync(RedisChannel.Literal("messages"), "hello");

            await _connection.GetDatabase().ListLeftPushAsync("list", "list_value");

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}