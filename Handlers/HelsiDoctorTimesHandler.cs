using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using IBWT.Framework.Abstractions;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Valeo.Bot.Services.HelsiAPI;

namespace Valeo.Bot.Handlers
{
    public class HelsiDoctorTimesHandler : IUpdateHandler
    {
        private const string Message = "Оберіть лікаря, до якого бажаєте записатись на прийом.";
        private static readonly InlineKeyboardMarkup Markup =
            new InlineKeyboardMarkup(new List<InlineKeyboardButton[]>
            {
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData("Сім. лікар Сафонов Д.О.", "doctors::safonov"),
                },
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData("Терапевт Паливода Д.В.", "doctors::palivoda"),
                },
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData("Педіатр Макарченко К.В.", "doctors::makarchenko"),
                },
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData("Терапевт Калита Н.В.", "doctors::kalita"),
                },
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData("Терапевт Лєонова О.О.", "doctors::leonova"),
                },
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData("Головне меню ↩️", "back::"),
                },
            });
        private readonly ILogger<DoctorsQueryHandler> logger;
        private readonly IHelsiAPIService helsiApi;

        public HelsiDoctorTimesHandler(
            ILogger<DoctorsQueryHandler> logger,
            IHelsiAPIService helsiApi
        )
        {
            this.logger = logger;
            this.helsiApi = helsiApi;
        }

        public async Task HandleAsync(IUpdateContext context, UpdateDelegate next, CancellationToken cancellationToken)
        {
            CallbackQuery cq = context.Update.CallbackQuery;
            try
            {
                await context.Bot.Client.DeleteMessageAsync(
                    cq.Message.Chat.Id,
                    cq.Message.MessageId
                );
            }
            catch (Exception e)
            {
                logger.LogError(e, "Cannot delete message before default");
            }

            await context.Bot.Client.SendTextMessageAsync(
                cq.Message.Chat.Id,
                Message,
                replyMarkup: Markup,
                parseMode: ParseMode.Markdown
            );
        }
    }
}