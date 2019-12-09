using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using IBWT.Framework.Abstractions;
using Valeo.Bot.Services.HelsiAuthorization;
using Telegram.Bot.Types.Enums;

namespace Valeo.Bot.Handlers
{
    public class AuthorizationHandler : IUpdateHandler
    {
        private IAuthorization _authorizationService;
        private ILogger<AuthorizationHandler> _logger;
        public AuthorizationHandler(ILogger<AuthorizationHandler> logger, IAuthorization authorizationService) 
        {
            _logger = logger;
            _authorizationService = authorizationService;
        }
        public async Task HandleAsync(IUpdateContext context, UpdateDelegate next, CancellationToken cancellationToken = default)
        {
            var update = context.Update;
            
            long chatId = 0;

            switch (update.Type)
            {
                case UpdateType.CallbackQuery:
                    chatId = update.CallbackQuery.From.Id;
                    break;
                case UpdateType.Message:
                    chatId = update.Message.From.Id;
                    break;
                case UpdateType.InlineQuery:
                    chatId = update.InlineQuery.From.Id;
                    break;
                default:
                    throw new ArgumentException("Cannot authorize this type of message.");
            }

            if(_authorizationService.IsAuthorized(chatId))
            {
                await next(context);
            }
            else
            {
                await _authorizationService.AuthorizeUser(update.Message.Chat);
            }
        }
    }
}