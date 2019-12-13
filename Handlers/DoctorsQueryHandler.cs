using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using IBWT.Framework.Abstractions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Valeo.Bot.Handlers
{
    public class DoctorsQueryHandler : IUpdateHandler
    {
        private const string Message = "Оберіть лікаря, до якого бажаєте записатись на прийом.";
        private static readonly InlineKeyboardMarkup Markup =
            new InlineKeyboardMarkup(new List<InlineKeyboardButton[]>
            {
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithUrl("Сім. лікар Сафонов Д.О.", "doctors::safonov"),
                },
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithUrl("Терапевт Паливода Д.В.", "doctors::palivoda"),
                },
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithUrl("Педіатр Макарченко К.В.", "doctors::makarchenko"),
                },
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithUrl("Терапевт Калита Н.В.", "doctors::kalita"),
                },
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithUrl("Терапевт Лєонова О.О.", "doctors::leonova"),
                },
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData("Головне меню ↩️", "back::"),
                },
            });

        public async Task HandleAsync(IUpdateContext context, UpdateDelegate next, CancellationToken cancellationToken)
        {
            CallbackQuery cq = context.Update.CallbackQuery;

            await context.Bot.Client.EditMessageTextAsync(
                cq.Message.Chat.Id,
                cq.Message.MessageId,
                Message,
                replyMarkup: Markup,
                parseMode: ParseMode.Markdown
            );
        }
    }
}