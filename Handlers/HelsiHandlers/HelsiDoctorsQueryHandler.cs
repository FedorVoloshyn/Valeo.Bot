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
using Valeo.Bot.Services.HelsiAPI.Models;

namespace Valeo.Bot.Handlers
{
    public class HelsiDoctorsQueryHandler : IUpdateHandler
    {
        private const string Message = "Оберіть лікаря, до якого бажаєте записатись на прийом 👨‍⚕️👩‍⚕️";
        private readonly ILogger<HelsiDoctorsQueryHandler> logger;
        private readonly IHelsiAPIService helsiApi;

        public HelsiDoctorsQueryHandler(
            ILogger<HelsiDoctorsQueryHandler> logger,
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

            List<Doctor> doctors = await helsiApi.GetDoctors(15);
            await context.Bot.Client.SendTextMessageAsync(
                cq.Message.Chat.Id,
                Message,
                replyMarkup: CreateKeyboard(doctors),
                parseMode: ParseMode.Markdown
            );
        }

        private InlineKeyboardMarkup CreateKeyboard(List<Doctor> doctors)
        {
            List<InlineKeyboardButton[]> rows = new List<InlineKeyboardButton[]>();

            foreach (Doctor doctor in doctors)
            {
                rows.Add(new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData(doctor.FirstName + " " + doctor.LastName , $"doctors::{doctor.ResourceId}")
                });
            }

            rows.Add(new InlineKeyboardButton[]
            {
                InlineKeyboardButton.WithCallbackData("Головне меню ↩️", "back::")
            });
            return new InlineKeyboardMarkup(rows);
        }
    }
}