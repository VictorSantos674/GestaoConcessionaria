using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;

namespace src.DesafioIntelectah
{
    public static class RedisConfig
    {
        public static void AddRedisCaching(this IServiceCollection services, string configuration)
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration;
                options.InstanceName = "GestaoConcessionaria_";
            });
        }
    }
}
