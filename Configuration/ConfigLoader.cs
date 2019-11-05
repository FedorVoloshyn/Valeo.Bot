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
            services.Configure<ConnectionStrings>(config.GetSection("ConnectionStrings"))
                .Configure<LoggingSettings>(config.GetSection("Logging"))
                .Configure<BotConfig>(config.GetSection("ValeoBot"))
                .Configure<ValeoApiConfig>(config.GetSection("ValeoApi"))
                .Configure<SMTPConnection>(config.GetSection("STMPConnection"));
            
            ConfigProvider configProvider = new ConfigProvider()
            {
                ConnectionStrings = GetConfiguration<ConnectionStrings>(config, "ConnectionStrings"),
                Logging = GetConfiguration<LoggingSettings>(config, "Logging"),
                BotConfig = GetConfiguration<BotConfig>(config, "ValeoBot"),
                ValeoApi = GetConfiguration<ValeoApiConfig>(config, "ValeoApi"),
                STMPConnection = GetConfiguration<SMTPConnection>(config, "STMPConnection")
            };
            return configProvider;
        }

        private T GetConfiguration<T>(IConfiguration config, string Path) where T : class
        {
            return config.GetSection(Path).Get<T>();
        }

    }
}