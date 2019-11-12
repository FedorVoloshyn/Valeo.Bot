using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using IBWT.Framework.Abstractions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Valeo.Bot.Services.ValeoKeyboards;
using ValeoBot.Services;

namespace ValeoBot.Models.Commands
{
    public class StartCommand : CommandBase
    {
        private readonly AuthorizationService authorizationService;

        public StartCommand(
            AuthorizationService authorizationService
            )
        {
            this.authorizationService = authorizationService;
        }
        public override async Task HandleAsync(IUpdateContext context, UpdateDelegate next, string[] args, CancellationToken cancellationToken = default)
        {
            // await authorizationService.AuthorizeUser(context.Update.Message.Chat);
            await context.Bot.Client.SendTextMessageAsync(
                context.Update.Message.Chat.Id,
                ValeoKeyboardsService.DefaultKeyboard.Message,
                ParseMode.Markdown,
                replyMarkup : ValeoKeyboardsService.DefaultKeyboard.Markup
            );
            await next(context);
        }
    }
}