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
    public class LocationsQueryHandler : IUpdateHandler
    {
        private const string Message = "*Днiпро*\nМедичний центр *ВАЛЕО*\nвулица Рабоча, 148, VIII-б";

        private static readonly Location Location = new Location { Latitude = 48.4530569f, Longitude = 35.0029239f };
        private static readonly InlineKeyboardMarkup Markup =
            new InlineKeyboardMarkup(new List<InlineKeyboardButton[]>
            {
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData("Головне меню ↩️", "back::"),
                }
            });

        public async Task HandleAsync(IUpdateContext context, UpdateDelegate next, CancellationToken cancellationToken)
        {
            CallbackQuery cq = context.Update.CallbackQuery;

            await context.Bot.Client.SendLocationAsync(
                cq.Message.Chat.Id,
                Location.Latitude,
                Location.Longitude
            );
            await context.Bot.Client.SendTextMessageAsync(
                cq.Message.Chat.Id,
                Message,
                ParseMode.Markdown,
                replyMarkup: Markup
            );

        }
    }
}