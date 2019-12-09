using System.Threading;
using System.Threading.Tasks;
using IBWT.Framework.Abstractions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Valeo.Bot.Services.ValeoKeyboards;

namespace Valeo.Bot.Handlers
{
    public class DefaultHandler : IUpdateHandler
    {
        public async Task HandleAsync(IUpdateContext context, UpdateDelegate next, CancellationToken cancellationToken)
        {
            Message msg = context.Update.Message ?? context.Update.CallbackQuery.Message;
            await context.Bot.Client.SendTextMessageAsync(
                msg.Chat.Id,
                ValeoKeyboardsService.DefaultKeyboard.Message,
                ParseMode.Markdown,
                replyMarkup : ValeoKeyboardsService.DefaultKeyboard.Markup
            );
        }
    }
}