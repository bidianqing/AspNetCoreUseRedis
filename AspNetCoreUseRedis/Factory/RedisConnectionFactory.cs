using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace AspNetCoreUseRedis.Factory
{
    public class RedisConnectionFactory
    {
        private IEnumerable<ConnectionMultiplexerWrapper> connections;
        private readonly FactoryOptions factoryOptions;
        private readonly ILogger<RedisConnectionFactory> _logger;

        public RedisConnectionFactory(IOptionsMonitor<FactoryOptions> optionsAccessor, 
            ILogger<RedisConnectionFactory> logger)
        {
            _logger = logger;

            if (optionsAccessor == null)
            {
                throw new ArgumentNullException(nameof(optionsAccessor));
            }

            factoryOptions = optionsAccessor.CurrentValue;

            connections = factoryOptions.ConnectionMultiplexerWrappers;
            foreach (var item in connections)
            {
                item.ConnectionMultiplexer = ConnectionMultiplexer.Connect(item.ConnectionString);
                item.ConnectionMultiplexer.ErrorMessage += (sender, e) =>
                {
                    _logger.LogError("ErrorMessage");
                };
                item.ConnectionMultiplexer.ConnectionFailed += (sender, e) =>
                {
                    _logger.LogError("ConnectionFailed");
                };
                item.ConnectionMultiplexer.InternalError += (sender, e) =>
                {
                    _logger.LogError("InternalError");
                };
            }
        }

        public IConnectionMultiplexer GetConnection()
        {
            return connections.FirstOrDefault(u => u.ConnectionMultiplexer.IsConnected)?.ConnectionMultiplexer;
        }

        public IConnectionMultiplexer GetConnection(string name)
        {
            var connection = connections.FirstOrDefault(u => u.Name.Equals(name, StringComparison.OrdinalIgnoreCase))?.ConnectionMultiplexer;

            if (connection == null || !connection.IsConnected)
            {
                connection = this.GetConnection();
            }

            return connection;
        }
    }
}
