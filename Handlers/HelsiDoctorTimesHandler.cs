using System.Linq;
using System.Text;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using IBWT.Framework.Abstractions;
using Microsoft.Extensions.Logging;
using IBWT.Framework.Pagination;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Valeo.Bot.Services.HelsiAPI;
using Valeo.Bot.Services.HelsiAPI.Models;
using Telegram.Bot.Types;

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

            string[] contextData = context.Items["Data"].ToString().Split("::");
            List<TimeSlot> timeSlots = await helsiApi.GetFreeTimeByDoctor(contextData[1], DateTime.Today);
            PaginatorBuilder<TimeSlot> pb = new PaginatorBuilder<TimeSlot>(10, 2, "doctortimes");

            pb.MessageBuilder(PaginatorMessageBuilder);

            PaginatorData pd = pb.Build(timeSlots.ToArray(), Int32.Parse(contextData[0]));


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
                pd.Message,
                replyMarkup: pd.ReplyMarkup,
                parseMode: ParseMode.Markdown
            );
        }

        private string PaginatorMessageBuilder<TimeSlot>(
            TimeSlot[] data, 
            int startIndex, 
            int itemsPerPage)
        {
            StringBuilder sb = new StringBuilder();

            for(int i = 0; i < data.Length && i < itemsPerPage; i++)
            {
                sb.Append($"{i + startIndex + 1}. {data[i].ToString()}{Environment.NewLine} [I'm an inline-style link](https://www.google.com) [Command Link](/google)");
            }
            
            return sb.ToString();
        }
    }
}