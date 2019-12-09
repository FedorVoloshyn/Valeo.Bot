using System;
using System.Collections.Generic;
using System.Globalization;
using Telegram.Bot.Types.ReplyMarkups;
using Valeo.Bot.Services.ValeoApi;
using Valeo.Bot.Services.ValeoApi.Models;

namespace Valeo.Bot.Services.ValeoKeyboards
{
    public class ValeoKeyboardsService
    {
        public static readonly ValeoKeyboard DefaultKeyboard = new ValeoKeyboard
        {
            Message = "Вітаємо у Valeo Diagnostic! Тут ви можете оформити запис на прийом до лікаря у нашій клінці. Натисніть *Записатися на прийом* для оформлення заявки.",
            Markup = new InlineKeyboardMarkup(new List<InlineKeyboardButton[]>
            {
                new InlineKeyboardButton[]
                {

                    InlineKeyboardButton.WithUrl("Записатися на прийом 📝", "https://helsi.me/find-by-organization/2fd443d4-ffaa-493c-872c-5a9322c3237a")
                },
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData("Наші лікарі 👨‍⚕️", "doctors::"),
                    InlineKeyboardButton.WithCallbackData("Залишити відгук ✍️", "feedback::")
                },
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData("Адреси 📍", "locations::"),
                    InlineKeyboardButton.WithCallbackData("Контакти 📞", "contacts::")
                },
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithSwitchInlineQuery("Поділитись 📢", "Звертайтесь до Валео Diagnostics! 🏥"),
                    InlineKeyboardButton.WithCallbackData("Про нас 🏥", "about::")
                }
            })
        };
        
        public static readonly ValeoKeyboard FailedKeyboard = new ValeoKeyboard
        {
            Message = "При обробці запиту сталася помился. Зв'яжіться з адміністратором або спробуйте повторити спробу пізніше.",
            Markup = new InlineKeyboardMarkup(new List<InlineKeyboardButton[]>
            {
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData("Записатись до лікаря", "defaut::"),
                }
            })
        };

        public ValeoKeyboard CreateDoctorsKeyboard(List<Doctor> doctors)
        {
            List<InlineKeyboardButton[]> rows = new List<InlineKeyboardButton[]>();
            List<InlineKeyboardButton> currentRow = new List<InlineKeyboardButton>();
            for (int i = 1; i < doctors.Count; i++)
            {
                currentRow.Add(InlineKeyboardButton.WithCallbackData($"{doctors[i].FirstName} {doctors[i].LastName}", $"{doctors[i].FirstName}|Times"));
                if (i % 2 == 0)
                {
                    rows.Add(currentRow.ToArray());
                    currentRow.Clear();
                }
            }
            rows.Add(new InlineKeyboardButton[]
            {
                InlineKeyboardButton.WithCallbackData("Назад", "back::"),
            });
            return new ValeoKeyboard() { Message = "Оберіть лікаря", Markup = new InlineKeyboardMarkup(rows) };
        }
        public ValeoKeyboard CreateTimesKeyboard(List<Time> times)
        {
            List<InlineKeyboardButton[]> rows = new List<InlineKeyboardButton[]>();
            List<InlineKeyboardButton> currentRow = new List<InlineKeyboardButton>();
            for (int i = 1; i < times.Count; i++)
            {
                string formatedTime = times[i].Value.ToString("g", CultureInfo.CreateSpecificCulture("es-ES")) 
                                    + " " 
                                    + times[i].Value.DayOfWeek.ToString();
                currentRow.Add(InlineKeyboardButton.WithCallbackData($"{formatedTime}", $"{formatedTime}|Save"));
                if (i % 2 == 0)
                {
                    rows.Add(currentRow.ToArray());
                    currentRow.Clear();
                }
            }
            rows.Add(new InlineKeyboardButton[]
            {
                InlineKeyboardButton.WithCallbackData("Назад", "back::"),
            });

            return new ValeoKeyboard() { Message = "Оберіть час", Markup = new InlineKeyboardMarkup(rows) };
        }
    }
}