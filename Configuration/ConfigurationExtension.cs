using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ValeoBot.Configuration
{
    public static class ConfigurationExtension
    {
        public static void AddConfigurationProvider(this IServiceCollection services, IConfiguration config)
        {
            ConfigLoader loader = new ConfigLoader();
            ConfigProvider provider = loader.GetConfigProvider(services, config);
            services.AddSingleton<ConfigProvider>(provider);
        }

    }
}