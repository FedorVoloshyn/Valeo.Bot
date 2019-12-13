using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using IBWT.Framework.Abstractions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Valeo.Bot.Services.ReviewCashService;
using IBWT.Framework.State.Providers;

namespace Valeo.Bot.Models
{
    public class FeedbackHandler : IUpdateHandler
    {
        private readonly ILogger<FeedbackHandler> _logger;
        private readonly IStateProvider stateProvider;

        public FeedbackHandler(
            ILogger<FeedbackHandler> logger,
            IStateProvider stateProvider
        )
        {
            this.stateProvider = stateProvider;
            this._logger = logger;
        }

        public async Task HandleAsync(IUpdateContext context, UpdateDelegate next, CancellationToken cancellationToken)
        {
            long chatId = GetChatIdFromUpdate(context.Update);

            if (context.Update.Message == null || String.IsNullOrEmpty(context.Update.Message.Text))
            {
                await context.Bot.Client.SendTextMessageAsync(
                    chatId,
                    "Схоже що ви відправили щось не те 😒\nБудь ласка, відправте *текстовим повідомленням* нам свій відгук стосовно обслуговування у нашій клініці."
                );
                return;
            }

            if (context.Update.Message.Text.Equals("Головне меню ↩️"))
            {
                stateProvider.GetState(chatId).StepForward("default");
                return;
            }
            var review = context.Update.Message.Text;
            await context.Bot.Client.SendTextMessageAsync(
                chatId,
                "Дякуємо за відгук стосовно обслуговування у нашій клініці!"
            );

            if(!_env.IsDevelopment())
                await _mailingService.SendEmailAsync(review);
            stateProvider.GetState(chatId).StepForward("default");

        }

        private long GetChatIdFromUpdate(Update update)
        {
            long chatId = 0;

            switch (update.Type)
            {
                case UpdateType.CallbackQuery:
                    chatId = update.CallbackQuery.From.Id;
                    break;
                case UpdateType.Message:
                    chatId = update.Message.From.Id;
                    break;
            }

            return chatId;
        }
    }
}