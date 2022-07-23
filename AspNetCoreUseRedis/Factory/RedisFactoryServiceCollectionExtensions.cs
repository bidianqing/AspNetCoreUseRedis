namespace AspNetCoreUseRedis.Factory
{
    public static class RedisFactoryServiceCollectionExtensions
    {
        public static IServiceCollection AddRedisFactory(this IServiceCollection services, Action<FactoryOptions> setupAction)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (setupAction == null)
            {
                throw new ArgumentNullException(nameof(setupAction));
            }

            services.AddOptions();
            services.Configure(setupAction);
            services.AddSingleton(typeof(RedisConnectionFactory));

            return services;
        }
    }
}
