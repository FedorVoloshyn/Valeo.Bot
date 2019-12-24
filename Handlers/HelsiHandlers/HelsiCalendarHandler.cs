using System.Timers;
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
using Telegram.Bot.Types.InlineQueryResults;
using IBWT.Framework.Services.State;
using System.Globalization;

namespace Valeo.Bot.Handlers
{
    public class HelsiCalendarHandler : IUpdateHandler
    {
        private const string Message = "Оберіть день візиту 👇:";

        private readonly ILogger<DoctorsQueryHandler> logger;
        private readonly IHelsiAPIService helsiApi;
        private readonly IStateCacheService stateProvider;

        public HelsiCalendarHandler(
            ILogger<DoctorsQueryHandler> logger,
            IHelsiAPIService helsiApi,
            IStateCacheService stateProvider
        )
        {
            this.logger = logger;
            this.helsiApi = helsiApi;
            this.stateProvider = stateProvider;
        }

        public async Task HandleAsync(IUpdateContext context, UpdateDelegate next, CancellationToken cancellationToken)
        {
            CallbackQuery cq = context.Update.CallbackQuery;

            string[] contextData = context.Items["Data"].ToString().Split("::");
            
            var keyboard = CalendarKeyboardBuilder(
                string.IsNullOrEmpty(contextData[0]) ?
                    DateTime.Now : Convert.ToDateTime(contextData[0]),
                contextData[1].ToString()
            );
            await context.Bot.Client.SendTextMessageAsync(
                cq.From.Id,
                Message,
                replyMarkup: keyboard,
                parseMode: ParseMode.Markdown
            );
        }
        private InlineKeyboardMarkup CalendarKeyboardBuilder(DateTime startDate, string doctorId)
        {
            var kb = new List<List<InlineKeyboardButton>>();
            List<InlineKeyboardButton> navigationRow = new List<InlineKeyboardButton>();
            if (startDate.Month <= DateTime.Now.Month)
            {
                navigationRow.Add(InlineKeyboardButton.WithCallbackData(" ", " "));
            }
            else
            {
                navigationRow.Add(InlineKeyboardButton.WithCallbackData(
                    "⬅️",
                    $"calendar::{startDate.AddMonths(-1).ToShortDateString()}::{doctorId}"));
            }
            navigationRow.Add(InlineKeyboardButton.WithCallbackData(
                    CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(startDate.Month).ToString(),
                    $"calendar::{startDate.ToShortDateString()}::{doctorId}"));
            if (startDate.Month >= DateTime.Now.AddDays(14).Month)
            {
                navigationRow.Add(InlineKeyboardButton.WithCallbackData(" ", " "));
            }
            else
            {
                navigationRow.Add(InlineKeyboardButton.WithCallbackData(
                    "➡️",
                    $"calendar::{startDate.AddMonths(1).ToShortDateString()}::{doctorId}"));
            }
            kb.Add(navigationRow);

            // List<InlineKeyboardButton> row = new List<InlineKeyboardButton>();
            // for (int i = 0; i < timeSlots.Count; i++)
            // {
            //     if(row == null)
            //         row = new List<KeyboardButton>();

            //     KeyboardButton button = new KeyboardButton(
            //         timeSlots[i].Start.ToShortTimeString()
            //     );
            //     row.Add(button);
            //     if ((i + 1) % 3 == 0)
            //     {
            //         kb.Add(row.ToArray());
            //         row = null;
            //     }
            // }
            // if(row != null)
            //     kb.Add(row.ToArray());

            kb.Add(new List<InlineKeyboardButton>(){ InlineKeyboardButton.WithCallbackData("Повернутись 🔙", "back::") });

            return new InlineKeyboardMarkup(kb);
        }
    }
}