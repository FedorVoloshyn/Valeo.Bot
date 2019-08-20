using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Valeo.Bot.Services.ValeoKeyboards;
using ValeoBot.Services;

namespace ValeoBot.Models
{
    public class CallbackQueryHandler : IUpdateHandler
    {
        private readonly ResponseController _responseController;

        public CallbackQueryHandler(ResponseController responseController)
        {
            _responseController = responseController;
        }

        public async Task HandleAsync(IUpdateContext context, UpdateDelegate next, CancellationToken cancellationToken)
        {
            CallbackQuery cq = context.Update.CallbackQuery;

            ValeoKeyboard reply = await _responseController.UpdateUserStateAsync(cq.Message.Chat.Id, cq.Data);
            await context.Bot.Client.EditMessageTextAsync(
                cq.Message.Chat.Id,
                
                cq.Message.MessageId,
                reply.Message,
                ParseMode.Markdown);
            await context.Bot.Client.EditMessageReplyMarkupAsync(
                cq.Message.Chat.Id,
                cq.Message.MessageId,
                reply.Markup);

            await next(context);
        }
    }
}