using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Valeo.Bot.Services.ValeoKeyboards;

namespace ValeoBot.Models.Commands
{
    public class StartCommand : CommandBase
    {

        public override async Task HandleAsync(IUpdateContext context, UpdateDelegate next, string[] args, CancellationToken cancellationToken = default)
        {
            Message msg = context.Update.Message;

            await context.Bot.Client.SendTextMessageAsync(
                msg.Chat,
                ValeoKeyboardsService.DefaultKeyboard.Message,
                parseMode: ParseMode.Markdown,
                
                replyMarkup : ValeoKeyboardsService.DefaultKeyboard.Markup);

            await next(context);
        }
    }
}