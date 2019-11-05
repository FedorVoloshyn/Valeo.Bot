using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using IBWT.Framework.Abstractions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Valeo.Bot.Services.ReviewCashService;

namespace ValeoBot.Models
{
    public class DataCollectFilterHandler : IUpdateHandler
    {
        private readonly ILogger<DataCollectFilterHandler> _logger;
        private readonly IReviewCacheService _reviewCacheService;

        public DataCollectFilterHandler(ILogger<DataCollectFilterHandler> logger, IReviewCacheService reviewCacheService)
        {
            _logger = logger;
            _reviewCacheService = reviewCacheService;
        }

        public async Task HandleAsync(IUpdateContext context, UpdateDelegate next, CancellationToken cancellationToken)
        {
            long chatId = GetChatIdFromUpdate(context.Update);
            var hasUncollectedData = _reviewCacheService.HasUnfinishedReview(chatId);

            if(hasUncollectedData && (context.Update.Message?.Location == null && context.Update.CallbackQuery == null))
            {
                await context.Bot.Client.SendTextMessageAsync(
                    chatId,
                    "Схоже що ви відправили щось не те 😒\nБудь ласка, відправте *текстовим повідомленням* нам свій відгук стосовно обслуговування у нашій клініці."
                );
            }
            else
            {
                await next(context, cancellationToken);
            }
        }

        private long GetChatIdFromUpdate(Update update)
        {
            long chatId = 0;

            switch(update.Type)
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