
using StackExchange.Redis;

namespace AspNetCoreUseRedis
{
    public class PopRedisMessageBackgroudService : BackgroundService
    {
        private readonly ConnectionMultiplexer _connectionMultiplexer;
        private readonly ILogger<PopRedisMessageBackgroudService> _logger;

        public PopRedisMessageBackgroudService(ConnectionMultiplexer connectionMultiplexer, ILogger<PopRedisMessageBackgroudService> logger)
        {
            _connectionMultiplexer = connectionMultiplexer;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // https://zhuanlan.zhihu.com/p/344269737
                // https://github.com/StackExchange/StackExchange.Redis/blob/main/docs/PipelinesMultiplexers.md
                var message = await _connectionMultiplexer.GetDatabase().ListRightPopAsync("list");
                if (message.HasValue)
                {
                    _logger.LogInformation($"Received message: {message}");

                    continue;
                }

                await Task.Delay(3000, stoppingToken);

            }
        }
    }
}
