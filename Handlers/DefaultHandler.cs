using System;
using System.Threading;
using System.Threading.Tasks;
using IBWT.Framework.Abstractions;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
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

            if (msg == null)
            {
                await context.Bot.Client.SendTextMessageAsync(
                    context.Update.CallbackQuery.From.Id,
                    ValeoKeyboardsService.DefaultKeyboard.Message,
                    ParseMode.Markdown,
                    replyMarkup: ValeoKeyboardsService.DefaultKeyboard.Markup
                );
                return;
            }
            // if message from InlineQuery
            if (msg.ReplyMarkup != null)
                return;


            try
            {
                if (context.Update.Type == UpdateType.CallbackQuery)
                    await context.Bot.Client.DeleteMessageAsync(
                        msg.Chat.Id,
                        msg.MessageId
                    );
            }
            catch (Exception e)
            {
                logger.LogError(e, "Cannot delete message before default");
            }

            var defaultMessage = await context.Bot.Client.SendTextMessageAsync(
                msg.Chat.Id,
                ValeoKeyboardsService.DefaultKeyboard.Message,
                ParseMode.Markdown,
                replyMarkup: ValeoKeyboardsService.DefaultKeyboard.Markup
            );
        }
    }
}