using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Framework.Abstractions;
using ValeoBot.Services;

namespace Valeo.Bot.Models
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
            
            if(_authorizationService.IsAuthorized(context.Update))
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