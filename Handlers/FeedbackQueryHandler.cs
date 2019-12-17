﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using IBWT.Framework.Abstractions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Valeo.Bot.Handlers
{
    public class FeedbackQueryHandler : IUpdateHandler
    {
        private const string Message = "Напишіть ваші враження та зауваження про якість обслуговування у клініці.";
        private static readonly ReplyKeyboardMarkup Markup = new ReplyKeyboardMarkup()
        {
            Keyboard = new KeyboardButton[][]
            {
                new KeyboardButton[]
                {
                    new KeyboardButton("Головне меню ↩️")
                }
            },
            ResizeKeyboard = true,
            OneTimeKeyboard = true
        };

        // new ReplyKeyboardMarkup(new List<InlineKeyboardButton[]>
        // {
        //     new InlineKeyboardButton[]
        //     {
        //         InlineKeyboardButton.WithCallbackData("Головне меню ↩️", "back::"),
        //     }
        // });

        public async Task HandleAsync(IUpdateContext context, UpdateDelegate next, CancellationToken cancellationToken)
        {
            CallbackQuery cq = context.Update.CallbackQuery;

            await context.Bot.Client.SendTextMessageAsync(
                cq.Message.Chat.Id,
                Message,
                replyMarkup: Markup,
                parseMode: ParseMode.Markdown
            );
        }
    }
}