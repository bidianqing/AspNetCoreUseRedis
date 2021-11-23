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

        public WeatherForecastController(ILogger<WeatherForecastController> logger,
            IDistributedCache cache,
            ConnectionMultiplexer connectionMultiplexer)
        {
            _logger = logger;
            _cache = cache;
            _redis = connectionMultiplexer.GetDatabase();
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            //_redis.StringSet("name", "tom", TimeSpan.FromSeconds(20));

            _cache.SetString("name", "tom", new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
            });



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