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
    }
}