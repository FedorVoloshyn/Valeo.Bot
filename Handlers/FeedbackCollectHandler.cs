using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using IBWT.Framework.Abstractions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Valeo.Bot.Services.ReviewCashService;
using IBWT.Framework.State.Providers;
using Microsoft.AspNetCore.Hosting;
using Valeo.Bot.Services;
using Valeo.Bot.Data.Repository;
using Valeo.Bot.Data.Entities;
using IBWT.Framework.Services.State;

namespace Valeo.Bot.Handlers
{
    public class FeedbackCollectHandler : IUpdateHandler
    {
        private readonly ILogger<FeedbackCollectHandler> logger;
        private readonly IStateCacheService stateProvider;
        private readonly IDataRepository<Feedback> feedbackRepository;
        private readonly IHostingEnvironment env;
        private readonly IMailingService mailingService;

        public FeedbackCollectHandler(
            ILogger<FeedbackCollectHandler> logger,
            IStateCacheService stateProvider,
            IDataRepository<Feedback> feedbackRepository,
            IHostingEnvironment env,
            IMailingService mailingService
        )
        {
            this.stateProvider = stateProvider;
            this.feedbackRepository = feedbackRepository;
            this.env = env;
            this.mailingService = mailingService;
            this.logger = logger;
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
                stateProvider.UpdateState(context, "default");
                return;
            }

            string reviewText = context.Update.Message.Text;
            logger.LogInformation($"Add new review = {reviewText}; chatId = {chatId}");

            Feedback review = new Feedback { ChatId = chatId, Text = reviewText };
            feedbackRepository.Add(review);
            await context.Bot.Client.SendTextMessageAsync(
                chatId,
                "Дякуємо за відгук стосовно обслуговування у нашій клініці!"
            );

            if(!env.IsDevelopment())
                await mailingService.SendEmailAsync(review);
            stateProvider.UpdateState(context, "default");

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