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
                    InlineKeyboardButton.WithUrl("Сім. лікар Сафонов Д.О.", "https://helsi.me/doctor/9c2f65a7-4c36-49ae-864b-a51bbcfe52f0"),
                },
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithUrl("Терапевт Паливода Д.В.", "https://helsi.me/doctor/dd4d4f9c-0618-4d05-900d-627875bc7ddd"),
                },
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithUrl("Педіатр Макарченко К.В.", "https://helsi.me/doctor/dec67dba-fc15-4d2f-971d-4dff8a5e6120"),
                },
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithUrl("Терапевт Калита Н.В.", "https://helsi.me/doctor/757f686e-c28c-4ad5-acd1-71de4c3906d5"),
                },
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithUrl("Терапевт Лєонова О.О.", "https://helsi.me/doctor/8db0a856-cb6e-480b-b9c8-37fbc6df9afe"),
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