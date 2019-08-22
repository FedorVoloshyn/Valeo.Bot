using ValeoBot.Configuration.Entities;
using ValeoBot.Configuration.Entities.Logging;

namespace ValeoBot.Configuration
{
    public class ConfigProvider
    {
        private ConnectionStrings connectionString;
        private LoggingSettings logging;
        private BotConfig botConfig;
        private ValeoApiConfig valeoApi;
        public ConnectionStrings ConnectionStrings
        {
            get
            {
                return connectionString;
            }
            set
            {
                connectionString = value;
            }
        }
        public ValeoApiConfig ValeoApi
        {
            get
            {
                return valeoApi;
            }
            set
            {
                valeoApi = value;
            }
        }
        public LoggingSettings Logging
        {
            get
            {
                return logging;
            }
            set
            {
                logging = value;
            }
        }
        public BotConfig BotConfig
        {
            get
            {
                return botConfig;
            }
            set
            {
                botConfig = value;
            }
        }
    }
}