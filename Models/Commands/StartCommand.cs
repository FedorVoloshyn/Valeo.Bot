using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Valeo.Bot.Models;

namespace ValeoBot.Models.Commands
{
    public class StartCommand : CommandBase
    {
        public override async Task HandleAsync(IUpdateContext context, UpdateDelegate next, string[] args, CancellationToken cancellationToken = default)
        {
            Message msg = context.Update.Message;

            await context.Bot.Client.SendTextMessageAsync(
                msg.Chat, 
                "Вітаємо у Valeo Diagnostic! Тут ви можете записатись на прийом " + 
                "до лікаря у нащій клінці. Натисніть 'Записатись до лікаря' для " +
                "оформлення заявки.", 
                replyMarkup: Keyboards.WelcomeKeyboard);
                    
            await next(context);
        }
    }
}