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
        private readonly IDistributedCache _cache;
        private readonly IDatabase _redis;
        private readonly IConnectionMultiplexer _connection;

        public HomeController(ILogger<HomeController> logger,
            IDistributedCache cache,
            ConnectionMultiplexer connectionMultiplexer)
        {
            _logger = logger;
            _cache = cache;
            _redis = connectionMultiplexer.GetDatabase();
            _connection = connectionMultiplexer;
        }

        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            //_redis.StringSet("name", "tom", TimeSpan.FromSeconds(20));

            //_cache.SetString("name", "tom", new DistributedCacheEntryOptions
            //{
            //    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
            //});

            _connection.GetDatabase().StringSet("name", "tom", TimeSpan.FromSeconds(20));
            long l = await _connection.GetDatabase().PublishAsync(RedisChannel.Literal("messages"), "hello");

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