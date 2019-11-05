using IBWT.Framework;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ValeoBot.Configuration.Entities;
using ValeoBot.Configuration.Entities.Logging;

namespace ValeoBot.Configuration
{
    public static class ConfigurationExtension
    {
        public static void AddConfigurationProvider(this IServiceCollection services, IConfiguration config, IHostingEnvironment env)
        {
            services.Configure<ConnectionStrings>(config.GetSection("ConnectionStrings"))
                .Configure<LoggingSettings>(config.GetSection("Logging"))
                .Configure<ValeoApiConfig>(config.GetSection("ValeoApi"))
                .Configure<SMTPConnection>(config.GetSection("STMPConnection"));

            if (env.IsDevelopment())
            {
                services.Configure<BotOptions>(config.GetSection("ValeoBotTest"));
            }
            else 
            {
                services.Configure<BotOptions>(config.GetSection("ValeoBot"));
            }
        }
        private static T GetConfiguration<T>(IConfiguration config, string Path) where T : class
        {
            return config.GetSection(Path).Get<T>();
        }
    }
}