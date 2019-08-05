using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace ValeoBot.Models
{
    public class CallbackQueryHandler : IUpdateHandler
    {
        public async Task HandleAsync(IUpdateContext context, UpdateDelegate next, CancellationToken cancellationToken)
        {
            CallbackQuery cq = context.Update.CallbackQuery;

            await context.Bot.Client.EditMessageReplyMarkupAsync(cq.Message.Chat.Id, cq.Message.MessageId, new InlineKeyboardMarkup(
                    InlineKeyboardButton.WithCallbackData("Pong", "/ping")));

            await next(context);
        }
    }
}