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

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _connectionMultiplexer.GetSubscriber().Subscribe(RedisChannel.Literal("messages"), (channel, message) =>
            {
                _logger.LogInformation($"Received message: {message}");
            });

            return base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.CompletedTask;
        }
    }
}
