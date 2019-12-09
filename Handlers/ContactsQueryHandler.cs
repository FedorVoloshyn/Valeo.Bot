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
    public class ContactsQueryHandler : IUpdateHandler
    {
        private const string Message = 
                        "__Контакти клініки *ВАЛЕО Diagnostics*__\n" + 
                        "\n*Сайт*: https://valeo.dp.ua/uk" + 
                        "\n*Телефон*: +380982323401" + 
                        "\n*Телефон*: +380952323400" + 
                        "\n*e-mail*: valeo.diagnostics@ukr.net";
        private static readonly InlineKeyboardMarkup Markup =
            new InlineKeyboardMarkup(new List<InlineKeyboardButton[]>
            {
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