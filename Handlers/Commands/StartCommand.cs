using System.Threading;
using System.Threading.Tasks;
using IBWT.Framework.Abstractions;
using Microsoft.Extensions.Logging;
using Valeo.Bot.Data.Entities;
using Valeo.Bot.Data.Repository;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Valeo.Bot.Services.ValeoKeyboards;

namespace Valeo.Bot.Handlers
{
    public class StartCommand : CommandBase
    {
        private readonly ILogger<StartCommand> logger;

        public StartCommand(
            ILogger<StartCommand> logger
        )
        {
            this.logger = logger;
        }
        public override async Task HandleAsync(
            IUpdateContext context,
            UpdateDelegate next,
            string[] args,
            CancellationToken cancellationToken
        )
        {
            var msg = context.Update.Message;

            await context.Bot.Client.SendTextMessageAsync(
                context.Update.Message.Chat.Id,
                ValeoKeyboardsService.DefaultKeyboard.Message,
                ParseMode.Markdown,
                replyMarkup : ValeoKeyboardsService.DefaultKeyboard.Markup
            );

            //await next(context, cancellationToken);
        }
    }
}