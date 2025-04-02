using StackExchange.Redis;

namespace AspNetCoreUseRedis
{
    public class ReceiveRedisMessageBackgroudService : BackgroundService
    {
        private readonly ConnectionMultiplexer _connectionMultiplexer;
        private readonly ILogger<ReceiveRedisMessageBackgroudService> _logger;
        public ReceiveRedisMessageBackgroudService(ConnectionMultiplexer connectionMultiplexer, ILogger<ReceiveRedisMessageBackgroudService> logger)
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
