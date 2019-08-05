using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Telegram.Bot.Framework;
using Telegram.Bot.Types;

namespace ValeoBot.Models
{
    public class ValeoLifeBot : BotBase
    {
        public ValeoLifeBot(IOptions<BotOptions<ValeoLifeBot>> options) 
        : base(options.Value) 
        { 
            
        }
    }
}