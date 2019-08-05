using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ValeoBot.Configuration.Entities;
using ValeoBot.Configuration.Entities.Logging;

namespace ValeoBot.Configuration
{
    public class ConfigLoader
    {
        public ConfigProvider GetConfigProvider(IServiceCollection services, IConfiguration config)
        {
            services.Configure<BotConfig>(config.GetSection("BotConfig"));
            ConfigProvider configProvider = new ConfigProvider()
            {
                ConnectionStrings = GetConfiguration<ConnectionStrings>(config, "ConnectionStrings"),
                Logging = GetConfiguration<LoggingSettings>(config, "Logging"),
                BotConfig = GetConfiguration<BotConfig>(config, "BotConfig"),
                ValeoApi = GetConfiguration<ValeoApi>(config, "ValeoApi")
            };
            return configProvider;
        }

        private T GetConfiguration<T>(IConfiguration config, string Path) where T : class
        {
            return config.GetSection(Path).Get<T>();
        }

    }
}