using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Valeo.Bot.Services.ValeoKeyboards;
using ValeoBot.Services;

namespace ValeoBot.Models.Commands
{
    public class StartCommand : CommandBase
    {
        private readonly SessionService session;

        public StartCommand(
            SessionService session
            )
        {
            this.session = session;
        }
        public override async Task HandleAsync(IUpdateContext context, UpdateDelegate next, string[] args, CancellationToken cancellationToken = default)
        {
            Message msg = context.Update.Message;

            await session.AuthorizeUser(msg.Chat);

            await next(context);
        }
    }
}