using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.Types;
using ValeoBot.Data.Entities;
using ValeoBot.Data.Repository;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types.Enums;
using Valeo.Bot.Services.ValeoKeyboards;
using ValeoBot.Services;

namespace ValeoBot.Models
{
    public class OrderUpdater : IUpdateHandler
    {
        private readonly AuthorizationService authorizationService;
        private ILogger<OrderUpdater> _logger;
        private IDataRepository<Registration> regRepository;

        public OrderUpdater(
            IDataRepository<Registration> regRepository,
            AuthorizationService authorizationService,
            ILogger<OrderUpdater> logger)
        {
            this.authorizationService = authorizationService;
            this._logger = logger;
            this.regRepository = regRepository;
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

            var reg = regRepository.Get(message.Chat.Id);

            if (reg == null)
            {
                await authorizationService.AuthorizeUser(message.Chat);
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