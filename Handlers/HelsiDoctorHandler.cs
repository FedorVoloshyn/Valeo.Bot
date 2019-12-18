using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using IBWT.Framework.Abstractions;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Valeo.Bot.Services.HelsiAPI;
using Valeo.Bot.Services.HelsiAPI.Models;

namespace Valeo.Bot.Handlers
{
    public class HelsiDoctorHandler : IUpdateHandler
    {
        private const string photoFolder = "Resourses/Images/doctorsPhoto/";
        private static readonly ReadOnlyDictionary<string, DoctorInfo> _doctors = new ReadOnlyDictionary<string, DoctorInfo>(
            new Dictionary<string, DoctorInfo>()
            {
                { "safonov", new DoctorInfo { DoctorTitle = "*Сафонов Денис Олегович*\nЛікар загальної практики - Сімейний лікар",  ImagePath = photoFolder + "safonov.jpg", Url = "https://helsi.me/doctor/9c2f65a7-4c36-49ae-864b-a51bbcfe52f0" } },
                { "palivoda", new DoctorInfo { DoctorTitle = "*Паливода Дмитро Васильович*\nЛікар-терапевт",  ImagePath = photoFolder + "palivoda.jpg", Url = "https://helsi.me/doctor/dd4d4f9c-0618-4d05-900d-627875bc7ddd" } },
                { "makarchenko", new DoctorInfo { DoctorTitle = "*Макарченко Катерина Вікторівна*\nЛікар-педіатр",  ImagePath = photoFolder + "makarchenko.jpg", Url = "https://helsi.me/doctor/dd4d4f9c-0618-4d05-900d-627875bc7ddd" } },
                { "kalita", new DoctorInfo { DoctorTitle = "*Калита Наталя Вікторівна*\nЛікар-терапевт",  ImagePath =  photoFolder + "kalita.jpg", Url = "https://helsi.me/doctor/757f686e-c28c-4ad5-acd1-71de4c3906d5" } },
                { "leonova", new DoctorInfo { DoctorTitle = "*Лєонова Оксана Олександрівна*\nЛікар-терапевт",  ImagePath =  photoFolder + "leonova.jpg", Url = "https://helsi.me/doctor/8db0a856-cb6e-480b-b9c8-37fbc6df9afe" } }
            }
        );
        private readonly ILogger<HelsiDoctorHandler> logger;        
        private readonly IHelsiAPIService helsiApi;

        public HelsiDoctorHandler(
            ILogger<HelsiDoctorHandler> logger,
            IHelsiAPIService helsiApi
        )
        {
            this.logger = logger;
            this.helsiApi = helsiApi;
        }

        public async Task HandleAsync(
            IUpdateContext context, 
            UpdateDelegate next, 
            CancellationToken cancellationToken
        )
        {
            CallbackQuery cq = context.Update.CallbackQuery;


            Doctor doc = await helsiApi.GetDoctor(context.Items["Data"].ToString());

            string message = $"*{doc.LastName} {doc.FirstName}*\n{doc.Position.Name}";

            InlineKeyboardMarkup markup = new InlineKeyboardMarkup(new List<InlineKeyboardButton[]>
            {
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData("Записатись на прийом", $"doctortimes::1::{doc.ResourceId}")
                },
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData("До списку лікарів", "doctors::"),
                },
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData("Головне меню ↩️", "default::"),
                }
            });
            try
            {
                await context.Bot.Client.DeleteMessageAsync(
                    cq.Message.Chat.Id,
                    cq.Message.MessageId
                );
            }
            catch (Exception e)
            {
                logger.LogError(e, "Cannot delete message before doctor info");
            }

            try
            {
                await context.Bot.Client.SendPhotoAsync(
                    cq.Message.Chat.Id,
                    $"https://helsi.me/doctor/media/{doc.PhotoId}",
                    caption: message,
                    replyMarkup: markup,
                    parseMode: ParseMode.Markdown
                );
            }
            catch (ApiRequestException e) 
            {
                logger.LogError(e, $"Cannot find doctors-photo {doc.PhotoId}");
            
                await context.Bot.Client.SendTextMessageAsync(
                    cq.Message.Chat.Id,
                    doc.Position.Name,
                    replyMarkup: markup,
                    parseMode: ParseMode.Markdown
                );
            }

            
        }

        private class DoctorInfo
        {
            public string DoctorTitle { get; set; }
            public string ImagePath { get; set; }
            public string Url { get; set; }
        }
    }
}