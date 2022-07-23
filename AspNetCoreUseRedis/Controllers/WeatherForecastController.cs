using AspNetCoreUseRedis.Factory;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;

namespace AspNetCoreUseRedis.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IDistributedCache _cache;
        private readonly IDatabase _redis;
        private readonly IConnectionMultiplexer _connection;

        public WeatherForecastController(ILogger<WeatherForecastController> logger,
            IDistributedCache cache,
            ConnectionMultiplexer connectionMultiplexer,
            RedisConnectionFactory redisConnectionFactory)
        {
            _logger = logger;
            _cache = cache;
            _redis = connectionMultiplexer.GetDatabase();
            _connection = redisConnectionFactory.GetConnection();
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            //_redis.StringSet("name", "tom", TimeSpan.FromSeconds(20));

            //_cache.SetString("name", "tom", new DistributedCacheEntryOptions
            //{
            //    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
            //});

            _connection.GetDatabase().StringSet("name", "tom", TimeSpan.FromSeconds(20));

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