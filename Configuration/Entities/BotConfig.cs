using Telegram.Bot.Framework.Abstractions;

namespace ValeoBot.Configuration.Entities
{
    public class BotConfig : IBotOptions
    {
        public string Username { get; set; }
        public string ApiToken { get; set; }
        public string WebhookDomain { get; set; }
        public string WebhookPath { get; set; }
    }
}