using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ValeoBot.Configuration.Entities;
using ValeoBot.Configuration.Entities.Logging;

namespace ValeoBot.Configuration
{
    public class ConfigLoader
    {
        public ConfigProvider GetConfigProvider(IConfiguration config)
        {
            ConfigProvider configProvider = new ConfigProvider()
            {
                ConnectionStrings = GetConfiguration<ConnectionStrings>(config, "ConnectionStrings"),
                Logging = GetConfiguration<LoggingSettings>(config, "Logging"),
                BotSettings = GetConfiguration<BotSettings>(config, "BotSettings"),
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