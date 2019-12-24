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
    public class HelsiDoctorAppyTimeHandler : IUpdateHandler
    {
        private const string Message = "Дякуємо, Вас було записано на {0}";        
        private const string MessageWait = "Обробляю, зачекайте ...";
        private static readonly InlineKeyboardMarkup Markup =
            new InlineKeyboardMarkup(new List<InlineKeyboardButton[]>
            {
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData("Головне меню ↩️", "default::"),
                }
            });
        private readonly ILogger<HelsiDoctorAppyTimeHandler> logger;
        private readonly IHelsiAPIService helsiApi;
        private readonly IStateCacheService stateProvider;

        public HelsiDoctorAppyTimeHandler(
            ILogger<HelsiDoctorAppyTimeHandler> logger,
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
            Message message = context.Update.Message;

            if(message.Text == "Повернутись 🔙")
            {
                await stateProvider.UpdateState(context, "default");
                stateProvider.InitUpdate(context);
                return;
            }


            // string[] contextData = context.Items["Data"].ToString().Split("::");

            await context.Bot.Client.SendChatActionAsync(message.From.Id, ChatAction.Typing); 
            await context.Bot.Client.SendTextMessageAsync(
                message.From.Id,
                MessageWait,
                replyMarkup: new ReplyKeyboardRemove(),
                parseMode: ParseMode.Markdown
            );

            await Task.Delay(1000);    

            await context.Bot.Client.SendTextMessageAsync(
                message.From.Id,
                string.Format(Message, message.Text),
                replyMarkup: Markup,
                parseMode: ParseMode.Markdown
            );
        }
    }
}