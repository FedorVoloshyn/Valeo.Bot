using System.Threading;
using System.Threading.Tasks;
using IBWT.Framework.Abstractions;
using Telegram.Bot.Types;
using ValeoBot.Data.Entities;
using ValeoBot.Data.Repository;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types.Enums;
using Valeo.Bot.Services.ValeoKeyboards;
using ValeoBot.Services;
using Valeo.Bot.Services.ReviewCashService;

namespace ValeoBot.Models
{
    public class OrderUpdater : IUpdateHandler
    {
        private readonly AuthorizationService _authorizationService;
        private ILogger<OrderUpdater> _logger;
        private IDataRepository<Registration> _regRepository;
        private readonly IReviewCacheService _reviewCacheService;

        public OrderUpdater(
            IDataRepository<Registration> regRepository,
            AuthorizationService authorizationService,
            ILogger<OrderUpdater> logger,
            IReviewCacheService reviewCacheService
            )
        {
            _authorizationService = authorizationService;
            _logger = logger;
            _regRepository = regRepository;
            _reviewCacheService = reviewCacheService;
        }

        public async Task HandleAsync(IUpdateContext context, UpdateDelegate next, CancellationToken cancellationToken = default)
        {
            await ProcessRequest(context, cancellationToken);
        }

        public async Task ProcessRequest(IUpdateContext context, CancellationToken cancellationToken = default)
        {
            Message message = context.Update.Message;

            if (string.IsNullOrEmpty(message.Text))
                return;

            var reg = _regRepository.Get(message.Chat.Id);

            if (reg == null)
            {
                await _authorizationService.AuthorizeUser(message.Chat);
            }
            else if(_reviewCacheService.HasUnfinishedReview(message.Chat.Id))
            {
                _reviewCacheService.AddReviewText(message.Chat.Id, message.Text);
            }
            else
            {
                await context.Bot.Client.SendTextMessageAsync(
                    message.Chat.Id,
                    ValeoKeyboardsService.DefaultKeyboard.Message,
                    ParseMode.Markdown,
                    replyMarkup : ValeoKeyboardsService.DefaultKeyboard.Markup
                );
            }
            return;
        }
    }
}