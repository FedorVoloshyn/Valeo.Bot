using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using ValeoBot.Services.ValeoApi;
using ValeoBot.Services.ValeoApi.Models;

namespace Valeo.Bot.Services.ValeoKeyboards
{
    public static class ValeoDoctorsKeyboards
    {
        private static string photoFolder = "doctorsPhoto/";
        public static ValeoKeyboard Safonov { get { return new ValeoKeyboard
            {
                Message = "*Сафонов Денис Олегович*\nЛікар загальної практики - Сімейний лікар",
                Markup = new InlineKeyboardMarkup(new List<InlineKeyboardButton[]>
                {
                    new InlineKeyboardButton[]
                    {
                        InlineKeyboardButton.WithUrl("Записатись на прийом", "https://helsi.me/doctor/9c2f65a7-4c36-49ae-864b-a51bbcfe52f0")
                    },
                    new InlineKeyboardButton[]
                    {
                        InlineKeyboardButton.WithCallbackData("До списку лікарів", ValeoCommands.OurDoctors),
                    },
                    new InlineKeyboardButton[]
                    {
                        InlineKeyboardButton.WithCallbackData("Головне меню ↩️", ValeoCommands.Default),
                    },
                }),
                ImagePath = photoFolder + "safonov.jpg"
            };
        }}
        public static ValeoKeyboard Palivoda {get { return new ValeoKeyboard
            {
                Message = "*Паливода Дмитро Васильович*\nЛікар-терапевт",
                Markup = new InlineKeyboardMarkup(new List<InlineKeyboardButton[]>
                {    
                    new InlineKeyboardButton[]
                    {
                        InlineKeyboardButton.WithUrl("Записатись на прийом", "https://helsi.me/doctor/dd4d4f9c-0618-4d05-900d-627875bc7ddd")
                    },    
                    new InlineKeyboardButton[]
                    {
                        InlineKeyboardButton.WithCallbackData("До списку лікарів", ValeoCommands.OurDoctors),
                    },            
                    new InlineKeyboardButton[]
                    {
                        InlineKeyboardButton.WithCallbackData("Головне меню ↩️", ValeoCommands.Default),
                    },
                }),
                ImagePath = photoFolder + "palivoda.jpg"
            };}}
        public static ValeoKeyboard Makarchenko { get { return new ValeoKeyboard
            {
                Message = "*Макарченко Катерина Вікторівна*\nЛікар-педіатр",
                Markup = new InlineKeyboardMarkup(new List<InlineKeyboardButton[]>
                {
                    new InlineKeyboardButton[]
                    {
                        InlineKeyboardButton.WithCallbackData("Головне меню ↩️", ValeoCommands.Default),
                    },
                    new InlineKeyboardButton[]
                    {
                        InlineKeyboardButton.WithCallbackData("До списку лікарів", ValeoCommands.OurDoctors),
                    },
                    new InlineKeyboardButton[]
                    {
                        InlineKeyboardButton.WithUrl("Записатись на прийом", "https://helsi.me/doctor/dd4d4f9c-0618-4d05-900d-627875bc7ddd")
                    },
                }),
                ImagePath = photoFolder + "makarchenko.jpg"
            };}}
        public static ValeoKeyboard Kalita { get { return new ValeoKeyboard
            {
                Message = "*Калита Наталя Вікторівна*\nЛікар-терапевт",
                Markup = new InlineKeyboardMarkup(new List<InlineKeyboardButton[]>
                {
                    new InlineKeyboardButton[]
                    {
                        InlineKeyboardButton.WithUrl("Записатись на прийом", "https://helsi.me/doctor/757f686e-c28c-4ad5-acd1-71de4c3906d5")
                    },
                    new InlineKeyboardButton[]
                    {
                        InlineKeyboardButton.WithCallbackData("До списку лікарів", ValeoCommands.OurDoctors),
                    },
                    new InlineKeyboardButton[]
                    {
                        InlineKeyboardButton.WithCallbackData("Головне меню ↩️", ValeoCommands.Default),
                    },
                }),
                ImagePath = photoFolder + "kalita.jpg"
            };}}
        public static ValeoKeyboard Leonova { get { return new ValeoKeyboard
            {
                Message = "*Лєонова Оксана Олександрівна*\nЛікар-терапевт",
                Markup = new InlineKeyboardMarkup(new List<InlineKeyboardButton[]>
                {
                    new InlineKeyboardButton[]
                    {
                        InlineKeyboardButton.WithUrl("Записатись на прийом", "https://helsi.me/doctor/8db0a856-cb6e-480b-b9c8-37fbc6df9afe")
                    },
                    new InlineKeyboardButton[]
                    {
                        InlineKeyboardButton.WithCallbackData("До списку лікарів", ValeoCommands.OurDoctors),
                    },
                    new InlineKeyboardButton[]
                    {
                        InlineKeyboardButton.WithCallbackData("Головне меню ↩️", ValeoCommands.Default),
                    },
                }),
                ImagePath = photoFolder + "leonova.jpg"
            };}}
    }
}