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
        private readonly IStateCacheService stateProvider;

        public HelsiDoctorTimesHandler(
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
            List<TimeSlot> timeSlots = await helsiApi.GetFreeTimeByDoctor(contextData[0], Convert.ToDateTime(contextData[1].ToString()));

            if (timeSlots.Count == 0)
            {
                await context.Bot.Client.SendTextMessageAsync(
                    cq.From.Id,
                    "Нажаль доктор у цей день доктор не приймає",
                    replyMarkup: new InlineKeyboardMarkup(new InlineKeyboardButton[]
                    {
                        InlineKeyboardButton.WithSwitchInlineQueryCurrentChat("До листу лікарів ↩️"),
                        InlineKeyboardButton.WithCallbackData("Головне меню ↩️", "default::")
                    }),
                    parseMode: ParseMode.Markdown
                );
                return;
            }

            //PaginatorBuilder<TimeSlot> pb = new PaginatorBuilder<TimeSlot>(10, 3, "doctortimes");
            //pb.MessageBuilder(PaginatorMessageBuilder);

            //PaginatorData pd = pb.Build(timeSlots.ToArray(), Int32.Parse(contextData[0]));


            var keyboard = TimeSlotsKeyboardBuilder(timeSlots);
            await context.Bot.Client.SendTextMessageAsync(
                cq.From.Id,
                $"Обраний день - {contextData[1]}. \nТеперь оберіть час у клавіатурі знизу 👇",
                replyMarkup: keyboard,
                parseMode: ParseMode.Markdown
            );
        }
        private ReplyKeyboardMarkup TimeSlotsKeyboardBuilder(List<TimeSlot> timeSlots)
        {
            var kb = new List<KeyboardButton[]>();
            kb.Add(new KeyboardButton[]{ new KeyboardButton("Повернутись 🔙") });

            List<KeyboardButton> row = new List<KeyboardButton>();
            for (int i = 0; i < timeSlots.Count; i++)
            {
                if(row == null)
                    row = new List<KeyboardButton>();

                KeyboardButton button = new KeyboardButton(
                    timeSlots[i].Start.ToShortTimeString()
                );
                row.Add(button);
                if ((i + 1) % 3 == 0)
                {
                    kb.Add(row.ToArray());
                    row = null;
                }
            }
            if(row != null)
                kb.Add(row.ToArray());

            return new ReplyKeyboardMarkup()
            {
                OneTimeKeyboard = true,
                ResizeKeyboard = true,
                Keyboard = kb.ToArray()
            };
        }
    }
}