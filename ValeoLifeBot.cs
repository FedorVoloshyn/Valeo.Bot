using System;
using System.Net;
using System.Threading.Tasks;
using IBWT.Framework;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types;
using ValeoBot.Configuration.Entities;

namespace ValeoBot.Models
{
    public class ValeoLifeBot : BotBase
    {
        public ValeoLifeBot(IOptions<BotOptions> options) 
            : base(options.Value)
        {
        }
    }
}