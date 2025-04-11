using StackExchange.Redis;

namespace AspNetCoreUseRedis
{
    public class SubscribeRedisMessageBackgroudService : BackgroundService
    {
        private readonly ConnectionMultiplexer _connectionMultiplexer;
        private readonly ILogger<SubscribeRedisMessageBackgroudService> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public SubscribeRedisMessageBackgroudService(ConnectionMultiplexer connectionMultiplexer, ILogger<SubscribeRedisMessageBackgroudService> logger, IServiceScopeFactory serviceScopeFactory)
        {
            _connectionMultiplexer = connectionMultiplexer;
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            await _connectionMultiplexer.GetSubscriber().SubscribeAsync(RedisChannel.Literal("messages"), async (channel, message) =>
            {
                _logger.LogInformation($"Received message: {message}");

                // https://www.milanjovanovic.tech/blog/using-scoped-services-from-singletons-in-aspnetcore
                using IServiceScope scope = _serviceScopeFactory.CreateScope();
                var demoService = scope.ServiceProvider.GetRequiredService<IDemoService>();

                await demoService.PrintMessage(message);
            });

            await base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {

                await Task.Delay(3000, stoppingToken);
            }
        }
    }
}
