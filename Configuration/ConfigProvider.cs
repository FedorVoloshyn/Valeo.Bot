using ValeoBot.Configuration.Entities;
using ValeoBot.Configuration.Entities.Logging;

namespace ValeoBot.Configuration
{
    public class ConfigProvider
    {
        private ConnectionStrings connectionString;
        private LoggingSettings logging;
        private BotSettings botSettings;
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
        public BotSettings BotSettings
        {
            get
            {
                return botSettings;
            }
            set
            {
                botSettings = value;
            }
        }
    }
}