using System;
using System.Threading;
using System.Threading.Tasks;
using IBWT.Framework.Abstractions;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Valeo.Bot.Services.ValeoKeyboards;

namespace Valeo.Bot.Handlers
{
    public class DefaultHandler : IUpdateHandler
    {
        private readonly ILogger<DefaultHandler> logger;

        public DefaultHandler(
            ILogger<DefaultHandler> logger
        )
        {
            this.logger = logger;
        }

        public async Task HandleAsync(IUpdateContext context, UpdateDelegate next, CancellationToken cancellationToken)
        {
            Message msg = context.Update.Message ?? context.Update.CallbackQuery.Message;

            try
            {
                await context.Bot.Client.DeleteMessageAsync(
                    msg.Chat.Id,
                    msg.MessageId
                );
            }
            catch (Exception e)
            {
                logger.LogError(e, "Cannot delete message before default");
            }

            await context.Bot.Client.SendTextMessageAsync(
                msg.Chat.Id,
                ValeoKeyboardsService.DefaultKeyboard.Message,
                ParseMode.Markdown,
                replyMarkup : ValeoKeyboardsService.DefaultKeyboard.Markup
            );
        }
    }
}