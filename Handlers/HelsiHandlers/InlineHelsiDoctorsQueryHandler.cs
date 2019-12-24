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
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.ReplyMarkups;
using Valeo.Bot.Services.HelsiAPI;
using Valeo.Bot.Services.HelsiAPI.Models;

namespace Valeo.Bot.Handlers
{
    public class InlineHelsiDoctorsQueryHandler : IUpdateHandler
    {
        private const string Message = "Оберіть лікаря, до якого бажаєте записатись на прийом 👨‍⚕️👩‍⚕️";
        private readonly ILogger<InlineHelsiDoctorsQueryHandler> logger;
        private readonly IHelsiAPIService helsiApi;

        public InlineHelsiDoctorsQueryHandler(
            ILogger<InlineHelsiDoctorsQueryHandler> logger,
            IHelsiAPIService helsiApi
        )
        {
            this.logger = logger;
            this.helsiApi = helsiApi;
        }

        public async Task HandleAsync(IUpdateContext context, UpdateDelegate next, CancellationToken cancellationToken)
        {
            InlineQuery iq = context.Update.InlineQuery;
            await context.Bot.Client.SendChatActionAsync(iq.From.Id, ChatAction.Typing);  

            // await Task.Delay(500);  

            List<InlineQueryResultBase> results = new List<InlineQueryResultBase>();
            List<Doctor> doctors = await helsiApi.GetDoctors(15);
            for (int i = 0; i < doctors.Count; i++)
            {
                string description = doctors[i].Speciality.Count > 0 ? doctors[i].Speciality[0].DoctorSpeciality : "Доктор Valeo Diagnostics";
                description += $"\n{doctors[i].Division?.Name}";

                results.Add(new InlineQueryResultArticle(
                    id: i.ToString(),
                    title: $"{doctors[i].LastName} {doctors[i].FirstName}",
                    new InputTextMessageContent($"{doctors[i].LastName} {doctors[i].FirstName}")
                )
                {
                    Description = description,
                    ReplyMarkup = new InlineKeyboardMarkup(new List<InlineKeyboardButton[]>
                    {
                        new InlineKeyboardButton[]
                        {
                            InlineKeyboardButton.WithCallbackData(
                                $"Записатись на сьогодні {DateTime.Now.ToShortDateString()}", 
                                $"doctortimes::{doctors[i].ResourceId}::{DateTime.Now.ToShortDateString()}"),
                        },                        
                        new InlineKeyboardButton[]
                        {
                            InlineKeyboardButton.WithCallbackData(
                            $"Записатись на завтра {DateTime.Now.AddDays(1).ToShortDateString()}", 
                            $"doctortimes::{doctors[i].ResourceId}::{DateTime.Now.AddDays(1).ToShortDateString()}"),
                        },                        
                        new InlineKeyboardButton[]
                        {
                            InlineKeyboardButton.WithCallbackData(
                            $"Інший день 📅", 
                            $"calendar::{DateTime.Today.ToShortDateString()}::{doctors[i].ResourceId}"),
                            InlineKeyboardButton.WithCallbackData(
                            $"Головне меню ↩️", 
                            $"default::"),
                        },
                    }),
                    ThumbUrl = "https://mir-s3-cdn-cf.behance.net/project_modules/max_1200/72d39556402457.59ad88082d175.png" //$"https://helsi.me/media/{doctors[i].PhotoId}"
                });
            }
        

            await context.Bot.Client.AnswerInlineQueryAsync(
                context.Update.InlineQuery.Id,
                results,
                isPersonal: true,
                cacheTime: 0);
            return;
        }
    }
}