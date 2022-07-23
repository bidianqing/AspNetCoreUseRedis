using StackExchange.Redis;

namespace AspNetCoreUseRedis.Factory
{
    public class FactoryOptions
    {
        public ConnectionMultiplexerWrapper[] ConnectionMultiplexerWrappers { get; set; }
    }

    public class ConnectionMultiplexerWrapper
    {
        public string Name { get; set; }

        public int Sort { get; set; }

        public string ConnectionString { get; set; }

        public IConnectionMultiplexer ConnectionMultiplexer { get; set; }
    }
}
