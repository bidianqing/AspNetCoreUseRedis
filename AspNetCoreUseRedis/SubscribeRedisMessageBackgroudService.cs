using StackExchange.Redis;

namespace AspNetCoreUseRedis
{
    public class SubscribeRedisMessageBackgroudService : BackgroundService
    {
        private readonly ConnectionMultiplexer _connectionMultiplexer;
        private readonly ILogger<SubscribeRedisMessageBackgroudService> _logger;
        public SubscribeRedisMessageBackgroudService(ConnectionMultiplexer connectionMultiplexer, ILogger<SubscribeRedisMessageBackgroudService> logger)
        {
            _connectionMultiplexer = connectionMultiplexer;
            _logger = logger;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            await _connectionMultiplexer.GetSubscriber().SubscribeAsync(RedisChannel.Literal("messages"), (channel, message) =>
            {
                _logger.LogInformation($"Received message: {message}");
            });

            await base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation(DateTime.Now.ToString());

                await Task.Delay(3000, stoppingToken);
            }
        }
    }
}
